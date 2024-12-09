using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    List<Match>? GetMatchesById(Guid Id);
    void AddMatch(Match match);
    void UpdateMatch(Match match);
    void DeleteMatch(Match match);
}