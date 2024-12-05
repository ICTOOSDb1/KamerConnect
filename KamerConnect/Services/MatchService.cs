using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class MatchService
{
    private IMatchRepository _matchRepository;

    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public Match? GetMatchById(Guid PersonId)
    {
        return _matchRepository.GetMatchById(PersonId);
    }

    public Match[]? GetMatchesByHouseId(Guid houseId)
    {
        return _matchRepository.GetMatchesByHouseId(houseId);
    }

    public void AddMatch(Match match)
    {
        _matchRepository.AddMatch(match);
    }

    public void UpdateMatch(Match match, status status)
    {
        _matchRepository.UpdateMatch(match , status);
    }

    public void DeleteMatch(Match match)
    {
        _matchRepository.DeleteMatch(match);
    }
}