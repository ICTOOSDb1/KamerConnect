using System.Collections;
using System.Text.RegularExpressions;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    List<MatchRequest> GetMatches(Guid id);
}