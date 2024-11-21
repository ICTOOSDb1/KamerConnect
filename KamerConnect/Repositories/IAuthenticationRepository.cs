namespace KamerConnect.Repositories;

public interface IAuthenticationRepository 
{
    void AddPassword(Guid personId, string password, string salt);
    byte[] GetSaltFromPerson(string email);
    void SaveSession(Guid personId, DateTime startingDate, string sessionToken);
    string GetPassword(string person_id);
}