﻿using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class Trainer
    {
        private readonly IAgent _agent0;
        private readonly IAgent _agent1;
        private readonly IStartPlayerDeterminer _startPlayerDeterminer;

        public Trainer(IAgent agent0, IAgent agent1, IStartPlayerDeterminer startPlayerDeterminer)
        {
            _agent0 = agent0;
            _agent1 = agent1;

            _agent0.IsTraining = true;
            _agent1.IsTraining = true;

            _agent0.Player = Player.Player0;
            _agent1.Player = Player.Player1;

            _startPlayerDeterminer = startPlayerDeterminer;
        }

        public void Train(int episodes)
        {
            for (var i = 0; i < episodes; i++)
            {
                TrainEpisode();
            }
        }

        private void TrainEpisode()
        {
            var game = new Game.TickTackToe(_startPlayerDeterminer);
            var currentStatus = game.GetStatus();
            while (currentStatus.GameStatus == GameStatus.InGame)
            {
                var currentAgent = currentStatus.Player == Player.Player0 ? _agent0 : _agent1;
                var otherAgent = currentStatus.Player == Player.Player0 ? _agent1 : _agent0;
                var move = currentAgent.GetNextMove(currentStatus);

                var moveResult = game.Move(currentStatus.Player, move.X, move.Y);

                var oldStatus = currentStatus;
                currentStatus = game.GetStatus();

                currentAgent.Observe(oldStatus, currentStatus, moveResult, move);
                otherAgent.Observe(oldStatus, currentStatus, moveResult, move);
            }
        }
    }
}
