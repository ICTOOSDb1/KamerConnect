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

    public Match[]? GetMatchesById(Guid houseId)
    {
        return _matchRepository.GetMatchesById(houseId);
    }

    public void AddMatch(Match match)
    {
        _matchRepository.AddMatch(match);
    }

    public void UpdateMatch(Match match)
    {
        _matchRepository.UpdateMatch(match);
    }

    public void DeleteMatch(Match match)
    {
        _matchRepository.DeleteMatch(match);
    }
}