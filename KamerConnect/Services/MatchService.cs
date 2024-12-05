using System.Collections;
using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect.Services;

public abstract class MatchService
{
    private IMatchRepository _matchRepository;

    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;   
    }
    public List<MatchRequest> GetMatchRequests(IMatchRepository matchRepository)
    {
        _matchRepository.GetMatches(new Guid());
        return new List<MatchRequest>();
    }
}