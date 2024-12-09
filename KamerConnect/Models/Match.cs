using System.ComponentModel.DataAnnotations;

namespace KamerConnect.Models;

public class Match
{
    public Guid personId { get; set; }
    public Guid matchId { get; set; }
    public Guid houseId { get; set; }
    public Status Status { get; set; }
    public string motivation { get; set; }

    public Match(Guid matchId, Guid personId, Guid houseId, Status status, string motivation)
    {
        this.personId = personId;
        this.matchId = matchId;
        this.houseId = houseId;
        Status = status;
        this.motivation = motivation;
    }
}

public enum Status
{
    [Display(Name = "Geaccepteerd")]
    Accepted,
    [Display(Name = "In afwachting")]
    Pending,
    [Display(Name = "Afgewezen")]
    Rejected
}