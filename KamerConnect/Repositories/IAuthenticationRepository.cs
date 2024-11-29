using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IAuthenticationRepository
{

    void AddPassword(Guid personId, string password, string salt);
    byte[]? GetSaltFromPerson(string email);
    void SaveSession(Guid personId, DateTime startingDate, string sessionToken);
    void UpdateSessionDate(string sessionToken);
    void RemoveSession(string sessionToken);
    Session GetSession(Guid personId);
    Session? GetSessionWithLocalToken(string? localSessionToken);
    string? GetPassword(Guid person_id);
}