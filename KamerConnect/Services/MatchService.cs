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

 

    public List<Match>? GetMatchesById(Guid Id)
    {
        return _matchRepository.GetMatchesById(Id);
    }
    

    public void UpdateMatch(Match match, status status)
    {
        _matchRepository.UpdateMatch(match , status);
    }
    
}