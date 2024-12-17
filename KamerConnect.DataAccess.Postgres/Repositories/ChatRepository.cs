﻿using System.Data;
using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using KamerConnect.DataAccess.Postgres.Repositories;
using Npgsql;


namespace KamerConnect.DataAccess.Postgres.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly string _connectionString;

    public ChatRepository()
    {
        _connectionString = EnvironmentUtils.GetConnectionString();
    }

    public List<ChatMessage> GetChatMessages(Guid chatId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM chatmessages
                                                   WHERE chat = @chatId
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
                        if (chatMessage != null)
                        {
                            messages.Add(chatMessage);
                        }
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

                string updateQuery = $"""
                                      INSERT INTO chatmessages (
                                        sender, chat, message)
                                        VALUES (@personId::uuid, @chat::uuid, @message::text
                                      )
                                      """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@personId", message.SenderId);
                    command.Parameters.AddWithValue("@chatId",chatId );
                    command.Parameters.AddWithValue("@message", message.Message);
              

                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while updating chatmessage in DB: {e.Message}");
            throw;
        }
    }
    
    private ChatMessage ReadToChatMessage(DbDataReader reader)
    {
        return  new ChatMessage(
            reader.GetGuid(0),
            reader.GetGuid(2),
            reader.GetString(3),
            reader.GetDateTime(4)
        );
    }
    
    public void CreateChat(Guid chatId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string updateQuery = $"""
                                      INSERT INTO chat (
                                        chat_id, match_id)
                                        VALUES (@chatId::uuid, @match_id::uuid
                                      )
                                      """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@chatId", chatId);
                    command.Parameters.AddWithValue("@matchId", null);
                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while updating createChat in DB: {e.Message}");
            throw;
        }
    }

    public void AddPersonToChat(Guid chatId, Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string updateQuery = $"""
                                      INSERT INTO person_chat (
                                        person_id, chat_id)
                                        VALUES (@person_id::uuid, @chat_id::uuid
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

    public void CreateChatWithPersons(List<Guid> personIds)
    {
        Guid chatId = Guid.NewGuid();
        CreateChat(chatId);
        foreach (var personId in personIds)
        {
            AddPersonToChat(chatId, personId);
        }
    }
    
    public List<Guid> GetPersonIdsFromChat(Guid chatId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM person_chat
                                                   WHERE (chat_id = @chatId::uuid)
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@chatId", chatId);

                using (var reader = command.ExecuteReader())
                {
                    List<Guid> personIds = new List<Guid>();

                    while (reader.Read())
                    {
                            personIds.Add(reader.GetGuid(0));
                    }

                    return personIds;
                }
            }
        }
    }
}