﻿using System.Collections.Generic;

namespace Betsolutions.Casino.SDK.Internal.TableGames.Backgammon.DTO.Tournament
{
    internal class TournamentPagingResult
    {
        public int TotalCount { get; set; }
        public IEnumerable<Tournament> Tournaments { get; set; }
    }
}
