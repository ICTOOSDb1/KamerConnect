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


    public List<Match> GetPendingMatchesById(Guid Id)
    {
        return _matchRepository.GetPendingMatchesById(Id);
    }

    public void UpdateStatusMatch(Match match, status status)
    {
        _matchRepository.UpdateStatusMatch(match, status);
    }
}