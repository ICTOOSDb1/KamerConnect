using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{

    List<Match> GetPendingMatchesById(Guid Id);
    List<Match> GetMatchesById(Guid Id);

    void UpdateStatusMatch(Match match, Status status);
    
    Guid CreateMatch(Match match);

}