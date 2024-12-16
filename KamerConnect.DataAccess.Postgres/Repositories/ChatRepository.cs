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

    public List<ChatMessage> getChatMessages(Guid matchId, Guid personId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   select *
                                                   from chatmessages
                                                   where match = @matchId;
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@personId", personId);
                command.Parameters.AddWithValue("@matchId", matchId);

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


    public void SendMessage(ChatMessage message)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = $"""
                                      INSERT INTO chatmessages (
                                        sender, match, message)
                                        VALUES (@personId::uuid, @matchId::uuid, @message::text
                                      )
                                      """;

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@personId", message.SenderId);
                    command.Parameters.AddWithValue("@matchId", message.MatchId);
                    command.Parameters.AddWithValue("@message", message.Message);
              

                    var result = command.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while updating Match in DB: {e.Message}");
            throw;
        }
    }
    
    private ChatMessage ReadToChatMessage(DbDataReader reader)
    {
        var chatMessage = new ChatMessage(
            reader.GetGuid(0),
            reader.GetGuid(1),
            reader.GetGuid(4),
            reader.GetString(2),
            reader.GetDateTime(3)
        );
        return chatMessage;
    }
}