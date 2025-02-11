﻿using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly string _connectionString;

    private readonly PersonService _personService;

    public ChatRepository(PersonService personService)
    {
        _connectionString = EnvironmentUtils.GetConnectionString();
        _personService = personService;
    }

    public List<ChatMessage> GetChatMessages(Guid chatId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM chat_messages
                                                   WHERE chat_id = @chatId
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@chatId", chatId);

                using (var reader = command.ExecuteReader())
                {
                    var messages = new List<ChatMessage>();

                    while (reader.Read())
                    {
                        var chatMessage = ReadToChatMessage(reader);
                        if (chatMessage != null) messages.Add(chatMessage);
                    }

                    return messages;
                }
            }
        }
    }


    public void CreateMessage(ChatMessage message, Guid chatId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var updateQuery = """
                                  INSERT INTO chat_messages (
                                    sender_id, chat_id, message)
                                    VALUES (@personId::uuid, @chatId::uuid, @message::text
                                  )
                                  """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@personId", message.SenderId);
                    command.Parameters.AddWithValue("@chatId", chatId);
                    command.Parameters.AddWithValue("@message", message.Message);


                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while creating message in DB: {e.Message}");
            throw;
        }
    }

    public List<Chat> GetChatsFromPerson(Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var chats = GetEmptyChatsFromPerson(personId);
                        chats = GetPersonsAndMessagesFromChats(chats);
                        return chats;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while retrieving chats from person Id: {e.Message}");
            throw;
        }
    }

    private ChatMessage ReadToChatMessage(DbDataReader reader)
    {
        return new ChatMessage(
            reader.GetGuid(0),
            reader.GetGuid(4),
            reader.GetString(2),
            reader.GetDateTime(3)
        );
    }

    private void CreateChat(Guid chatId, Guid? matchId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var updateQuery = """
                                  INSERT INTO chat (
                                    id, match_id)
                                    VALUES (@chatId::uuid, @matchId::uuid
                                  )
                                  """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@chatId", chatId);
                    command.Parameters.AddWithValue("@matchId", matchId);
                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while creating chat in DB: {e.Message}");
            throw;
        }
    }

    private void AddPersonToChat(Guid chatId, Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var updateQuery = """
                                  INSERT INTO person_chat (
                                    person_id, chat_id)
                                    VALUES (@person_id::uuid, @chatId::uuid
                                  )
                                  """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@person_id", personId);
                    command.Parameters.AddWithValue("@chatId", chatId);
                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while adding person to chat in DB: {e.Message}");
            throw;
        }
    }

    public void Create(Chat chat)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var chatId = chat.ChatId;
                        CreateChat(chatId, chat.MatchId);

                        var personInChat = chat.PersonsInChat;

                        foreach (var person in personInChat) AddPersonToChat(chatId, person.Id);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while creating chats: {e.Message}");
        }
    }

    private List<Chat> GetEmptyChatsFromPerson(Guid personId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT DISTINCT id, match_id
                                                   FROM person_chat pc
                                                   LEFT JOIN chat c ON pc.chat_id = c.id
                                                   WHERE (pc.person_id = @personId::uuid)
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@personId", personId);

                using (var reader = command.ExecuteReader())
                {
                    var chats = new List<Chat>();

                    while (reader.Read())
                    {
                        var chat = new Chat(
                            reader.GetGuid(0),
                            reader.IsDBNull(1) ? null : reader.GetGuid(1),
                            new List<Person>(),
                            new List<ChatMessage>()
                        );
                        chats.Add(chat);
                    }

                    return chats;
                }
            }
        }
    }

    private List<Chat> GetPersonsAndMessagesFromChats(List<Chat> chats)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var chat in chats)
                        {
                            var personsInChat = _personService.GetPersonsByChatId(chat.ChatId);
                            var chatMessages = GetChatMessages(chat.ChatId);

                            chat.PersonsInChat = personsInChat;
                            chat.Messages = chatMessages;
                        }

                        return chats;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while retrieving persons and messages for chat: {e.Message}");
            throw;
        }
    }
}