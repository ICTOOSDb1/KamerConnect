using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IAuthenticationRepository 
{
    void AddPassword(string? personId, string password, string salt);
    byte[]? GetSaltFromPerson(string email);
    void SaveSession(string personId, DateTime startingDate, string sessionToken);
    void UpdateSessionDate(string sessionToken);
    void RemoveSession(string? sessionToken);
    Session? GetSession(string personId);
    Session? GetSessionWithLocalToken(string? localSessionToken);
    string? GetPassword(string person_id);
}