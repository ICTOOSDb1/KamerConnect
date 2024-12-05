using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    Match? GetMatchById(Guid id);
    Match[]? GetMatchesByHouseId(Guid houseId);
    void AddMatch(Match match);
    void UpdateMatch(Match match, status status);
    void DeleteMatch(Match match);
}