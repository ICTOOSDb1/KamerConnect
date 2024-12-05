using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    Match? GetMatchById(Guid id);
    Match[]? GetMatchesByHouseId(Guid houseId);
    void AddMatch(Match match);
    void UpdateMatch(Match match);
    void DeleteMatch(Match match);
}