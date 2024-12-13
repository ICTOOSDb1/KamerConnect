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

    public Match GetMatchById(Guid matchId)
    {
        return _matchRepository.GetMatchById(matchId);
    }
    public List<Match> GetMatchesById(Guid Id)
    {
        return _matchRepository.GetMatchesById(Id);
    }

    public List<Match> GetPendingMatchesById(Guid Id)
    {
        return _matchRepository.GetPendingMatchesById(Id);
    }

    public void UpdateStatusMatch(Match match, Status status)
    {
        _matchRepository.UpdateStatusMatch(match, status);
    }

    public Guid CreateMatch(Match match)
    {
        return _matchRepository.CreateMatch(match);
    }

}