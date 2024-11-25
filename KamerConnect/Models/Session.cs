namespace KamerConnect.Models;

public class Session
{
    public Session(string sessionToken, DateTime startingDate, Guid personId)
    {
        this.sessionToken = sessionToken;
        this.startingDate = startingDate;
        this.personId = personId;
    }

    public string sessionToken { get; set; }
    public DateTime startingDate { get; set; }
    public Guid personId { get; set; }
}