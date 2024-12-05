using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{

    Match[]? GetMatchesByHouseId(Guid houseId);

    void UpdateMatch(Match match, status status);
   
}