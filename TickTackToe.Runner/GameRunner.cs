﻿using System.Collections.Generic;
using TickTackToe.Agent;
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

            _player0.Player = Player.Player0;
            _player1.Player = Player.Player1;

            _player0.IsTraining = false;
            _player1.IsTraining = false;

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

            var moveResult = game.Move(status.Player, move.X, move.Y);
            Moves.Add(new ExecutedMove(status, move, moveResult));

            status = game.GetStatus();
            return status.GameStatus == GameStatus.InGame;
        }
    }
}
