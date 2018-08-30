using System.Collections.Generic;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class GameRunner
    {
        private readonly IAgent _player0;
        private readonly IAgent _player1;
        private readonly IStartPlayerDeterminer _startPlayerDeterminer;

        public GameRunner(IAgent player0, IAgent player1, IStartPlayerDeterminer startPlayerDeterminer)
        {
            _player0 = player0;
            _player1 = player1;
            _startPlayerDeterminer = startPlayerDeterminer;
        }
        
        public List<ExecutedMove> Moves { get; set; }

        public Status RunGame()
        {
            var game = new Game.TickTackToe(_startPlayerDeterminer);
            Moves = new List<ExecutedMove>();
            bool canContinue;
            do
            {
                canContinue = MoveNext(game);
            } while (canContinue);

            return game.GetStatus();
        }

        private bool MoveNext(Game.TickTackToe game)
        {
            var status = game.GetStatus();
            var move = status.Player == Player.Player0 ? _player0.GetNextMove(status) : _player1.GetNextMove(status);

            Moves.Add(new ExecutedMove(status.Player, move));

            game.Move(status.Player, move.X, move.Y);

            status = game.GetStatus();
            return status.GameStatus == GameStatus.InGame;
        }
    }
}
