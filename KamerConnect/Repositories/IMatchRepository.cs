using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{

    List<Match> GetPendingMatchesById(Guid Id);

    void UpdateStatusMatch(Match match, status status);

}