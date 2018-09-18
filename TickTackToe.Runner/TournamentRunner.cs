using System;
using System.Collections.Generic;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class TournamentRunner
    {
        private readonly IAgent _player0;
        private readonly IAgent _player1;
        private readonly IStartPlayerDeterminer _startPlayerDeterminer;

        public TournamentRunner(IAgent player0, IAgent player1, IStartPlayerDeterminer startPlayerDeterminer)
        {
            _player0 = player0;
            _player1 = player1;
            
            _startPlayerDeterminer = startPlayerDeterminer;
        }
        
        public TournamentResult RunGame(int iterations)
        {
            var tournamentResult = new TournamentResult();
            for (var i = 0; i < iterations; i++)
            {
                var runner = new GameRunner(_player0, _player1, _startPlayerDeterminer);
                var endStatus = runner.RunGame();
                Console.WriteLine($"Game {i} ended {endStatus.GameStatus}");
                switch (endStatus.GameStatus)
                {
                    case GameStatus.Player0Won:
                        tournamentResult.Player0Won++;
                        break;
                    case GameStatus.Player1Won:
                        tournamentResult.Player1Won++;
                        break;
                    case GameStatus.Draw:
                        tournamentResult.Draw++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return tournamentResult;
        }
    }

    public class TournamentResult
    {
        public int Player0Won { get; set; }
        public int Player1Won { get; set; }
        public int Draw { get; set; }
    }
}
