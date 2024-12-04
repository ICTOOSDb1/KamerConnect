namespace KamerConnect.Models;

public class MatchRequest
{
    public Guid PersonId { get; set; }
    public Guid HouseId { get; set; }
    public string ProfileImagePath { get; set; }
    
    public MatchRequest(Guid personId, Guid houseId, string profileImagePath)
    {
        PersonId = personId;
        HouseId = houseId;
        ProfileImagePath = profileImagePath;
    }
}