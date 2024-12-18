using System.Data;
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

    private PersonRepository _personRepository;

    public ChatRepository(PersonRepository personRepository)
    {
        _connectionString = EnvironmentUtils.GetConnectionString();
        this._personRepository = personRepository;
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

    private ChatMessage ReadToChatMessage(DbDataReader reader)
    {
        return new ChatMessage(
            reader.GetGuid(0),
            reader.GetGuid(2),
            reader.GetString(3),
            reader.GetDateTime(4)
        );
    }

    private void CreateChat(Guid chatId)
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

    public void Create(List<Guid> personIds)
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
                        Guid chatId = Guid.NewGuid();
                        CreateChat(chatId);
                        foreach (var personId in personIds)
                        {
                            AddPersonToChat(chatId, personId);
                        }
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
            throw;
        }
    }

    private List<Guid> GetChatIdsFromPerson(Guid personId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT DISTINCT id
                                                   FROM person_chat pc
                                                   LEFT JOIN chat c ON pc.chat_id = c.id
                                                   WHERE (pc.person_id = @personId::uuid)
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@personId", personId);

                using (var reader = command.ExecuteReader())
                {
                    List<Guid> chatIds = new List<Guid>();

                    while (reader.Read())
                    {
                        chatIds.Add(reader.GetGuid(0));
                    }

                    return chatIds;
                }
            }
        }
    }

    private Guid? GetMatchIdFromChat(Guid chatId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT match_id
                                                   FROM chat c
                                                   WHERE (c.id = @chatId::uuid)
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@chatId", chatId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetGuid(0);
                    }
                }
            }
        }

        return null;
    }

    public List<Chat> GetChatsFromPersonId(Guid personId)
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
                        List<Guid> chatIds = GetChatIdsFromPerson(personId);
                        List<Chat> chats = new List<Chat>();

                        for (int i = 0; i < chatIds.Count; i++)
                        {
                            Guid? matchId = GetMatchIdFromChat(chatIds[i]);
                            List<Person> personsInChat = _personRepository.GetPersonsFromChatId(chatIds[i]);

                            chats[i] = new Chat(chatIds[i], matchId, personsInChat);
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
            Console.WriteLine($"Error while retrieving chats: {e.Message}");
            throw;
        }
    }
}