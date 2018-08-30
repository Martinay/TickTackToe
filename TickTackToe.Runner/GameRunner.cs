using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class GameRunner
    {
        private readonly IAgent _player0;
        private readonly IAgent _player1;
        private readonly Game.TickTackToe _game;

        public GameRunner(IAgent player0, IAgent player1, IStartPlayerDeterminer startPlayerDeterminer)
        {
            _player0 = player0;
            _player1 = player1;

            _game = new Game.TickTackToe(startPlayerDeterminer);
            Status = _game.GetStatus();
        }

        public Status Status { get; private set; }

        public bool MoveNext()
        {
            var move = Status.Player == Player.Player0 ? _player0.GetNextMove(Status) : _player1.GetNextMove(Status);

            _game.Move(Status.Player, move.X, move.Y);

            Status = _game.GetStatus();
            return Status.GameStatus == GameStatus.InGame;
        }
    }
}
