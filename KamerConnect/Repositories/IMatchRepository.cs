﻿using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IMatchRepository
{
    List<Match>? GetMatchesById(Guid Id);
    void UpdateMatch(Match match, Status status);
    public Guid CreateMatch(Match match);
}