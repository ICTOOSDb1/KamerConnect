using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    Match[]? GetMatchesById(Guid houseId);
    void AddMatch(Match match);
    void UpdateMatch(Match match);
    void DeleteMatch(Match match);
}