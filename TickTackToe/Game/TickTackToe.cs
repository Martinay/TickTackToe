using System;
using System.Collections.Generic;
using System.Linq;

namespace TickTackToe.Game
{
    public class TickTackToe
    {
        private readonly List<List<Player>> _field = new List<List<Player>>(3)
        {
            new List<Player>(3) {Player.Undefined, Player.Undefined, Player.Undefined},
            new List<Player>(3) {Player.Undefined, Player.Undefined, Player.Undefined},
            new List<Player>(3) {Player.Undefined, Player.Undefined, Player.Undefined}
        };

        private Player _currentPlayer = Player.Player0;
        private GameStatus _gameStatus = GameStatus.InGame;

        public Status GetStatus()
        {
            return new Status(_gameStatus, _currentPlayer, _field);
        }

        public MoveResult Move(Player player, int x, int y)
        {
            CheckInvalidInputParameterAndThrow(player, x, y);

            if (_field[y][x] != Player.Undefined)
            {
                _gameStatus = player == Player.Player0 ? GameStatus.Player1Won : GameStatus.Player0Won;
                UpdatePlayer(player);
                return MoveResult.Invalid;
            }

            _field[y][x] = player;

            UpdateGameIfEnded(player);

            UpdatePlayer(player);
            return MoveResult.Valid;
        }

        private void CheckInvalidInputParameterAndThrow(Player player, int x, int y)
        {
            if (_gameStatus != GameStatus.InGame)
                throw new InvalidOperationException("game already ended");

            if (player != _currentPlayer)
                throw new ArgumentException($"player: {player}");

            if (y > 2 || y < 0 || x > 2 || x < 0)
                throw new ArgumentOutOfRangeException($"y:{y} x:{x}");
        }

        private void UpdateGameIfEnded(Player player)
        {
            if (CheckHorizontalWin(player) ||
                CheckVerticalWin(player) ||
                CheckDiagonalTopLeftBottomRightWin(player) ||
                CheckDiagonalBottomLeftTopRightWin(player))
            {
                _gameStatus = player == Player.Player0 ? GameStatus.Player0Won : GameStatus.Player1Won;
                return;
            }

            if (_field.SelectMany(x => x).All(x => x != Player.Undefined))
                _gameStatus = GameStatus.Draw;
        }

        private bool CheckDiagonalTopLeftBottomRightWin(Player player)
        {
            return _field[0][0] == player &&
                   _field[1][1] == player &&
                   _field[2][2] == player;
        }

        private bool CheckDiagonalBottomLeftTopRightWin(Player player)
        {
            return _field[0][2] == player &&
                   _field[1][1] == player &&
                   _field[2][0] == player;
        }

        private bool CheckVerticalWin(Player player)
        {
            return _field.Select(x => x[0]).All(x => x == player) ||
                   _field.Select(x => x[1]).All(x => x == player) ||
                   _field.Select(x => x[2]).All(x => x == player);
        }

        private bool CheckHorizontalWin(Player player)
        {
            return _field.Any(x => x.All(y => y == player));
        }

        private void UpdatePlayer(Player player)
        {
            _currentPlayer = player == Player.Player0 ? Player.Player1 : Player.Player0;
        }
    }
}