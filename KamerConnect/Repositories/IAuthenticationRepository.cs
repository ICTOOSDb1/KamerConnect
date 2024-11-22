namespace KamerConnect.Repositories;

public interface IAuthenticationRepository 
{
    void AddPassword(string personId, string password, string salt);
    byte[] GetSaltFromPerson(string email);
    void SaveSessionInDB(string personId, DateTime startingDate, string sessionToken);
    Task SaveSessionInLS(string personId, DateTime startingDate, string sessionToken);
    string GetPassword(string person_id);
}