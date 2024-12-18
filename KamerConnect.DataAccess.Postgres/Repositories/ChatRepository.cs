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
                                                   SELECT id, sender_id, message, timestamp
                                                   FROM chat_messages
                                                   WHERE chat_id = @chatId
                                                   ORDER BY timestamp ASC
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

    public List<Person> GetPersons(Guid chatId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT p.*
                                                   FROM Person p
                                                   JOIN person_chat pc ON p.id = pc.person_id
                                                   WHERE pc.chat_id = @chatId
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@chatId", chatId);

                using (var reader = command.ExecuteReader())
                {
                    var persons = new List<Person>();

                    while (reader.Read())
                    {
                        var person = ReadToPerson(reader);
                        if (person != null)
                        {
                            persons.Add(person);
                        }
                    }

                    return persons;
                }
            }
        }
    }

    public Chat GetChatByMatchId(Guid matchId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM chat
                                                   WHERE match_id = @matchId
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@matchId", matchId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return ReadToChat(reader);
                }
            }
        }

        return null;
    }
    


    public void CreateMessage(ChatMessage message, Guid chatId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = $"""
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


                    command.ExecuteNonQuery();
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
        return new ChatMessage(
            reader.GetGuid(0),
            reader.GetGuid(1),
            reader.GetString(2),
            reader.GetDateTime(3)
        );
    }

    private Person ReadToPerson(DbDataReader reader)
    {
        return new Person
        (
            reader.GetString(1),
            reader.GetString(2),
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.GetString(4),
            reader.IsDBNull(5) ? null : reader.GetString(5),
            reader.GetDateTime(6),
            EnumUtils.Validate<Gender>(reader.GetString(7)),
            EnumUtils.Validate<Role>(reader.GetString(8)),
            reader.IsDBNull(9) ? null : reader.GetString(9),
            reader.GetGuid(0)
        );
    }

    private Chat ReadToChat(DbDataReader reader)
    {
        return new Chat
        (
            reader.GetGuid(0),
            reader.GetGuid(1)
        );
    }
}