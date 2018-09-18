using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class TimeboxedTrainer
    {
        private readonly IAgent _agent0;
        private readonly IAgent _agent1;
        private readonly IStartPlayerDeterminer _startPlayerDeterminer;
        private readonly TimeSpan _maxTime;
        private readonly Stopwatch _stopwatchAgent0;
        private readonly Stopwatch _stopwatchAgent1;

        public TimeboxedTrainer(IAgent agent0, IAgent agent1, IStartPlayerDeterminer startPlayerDeterminer, TimeSpan maxTime)
        {
            _agent0 = agent0;
            _agent1 = agent1;

            _agent0.IsTraining = true;
            _agent1.IsTraining = true;

            _agent0.Player = Player.Player0;
            _agent1.Player = Player.Player1;

            _startPlayerDeterminer = startPlayerDeterminer;
            _maxTime = maxTime;

            _stopwatchAgent0 = new Stopwatch();
            _stopwatchAgent1 = new Stopwatch();
        }

        public TimeboxedTrainingResult Train(int episodes)
        {
            _stopwatchAgent0.Reset();
            _stopwatchAgent1.Reset();

            for (var i = 0; i < episodes; i++)
            {
                var result = TrainEpisode();
                if (result != TimeboxedTrainingResult.Done)
                    return result;
            }

            return TimeboxedTrainingResult.Done;
        }

        private TimeboxedTrainingResult TrainEpisode()
        {
            var game = new Game.TickTackToe(_startPlayerDeterminer);
            var currentStatus = game.GetStatus();
            while (currentStatus.GameStatus == GameStatus.InGame)
            {
                var currentTrainer = currentStatus.Player == Player.Player0 ? _agent0 : _agent1;
                var otherTrainer = currentStatus.Player == Player.Player0 ? _agent1 : _agent0;
                var move = MeasureExecutionTimeAndStopAtMaxTime(currentTrainer.Player, () => currentTrainer.GetNextMove(currentStatus));

                if (move == null)
                    return currentStatus.Player == Player.Player0
                        ? TimeboxedTrainingResult.Agent0TookTooLong
                        : TimeboxedTrainingResult.Agent1TookTooLong;

                var moveResult = game.Move(currentStatus.Player, move.X, move.Y);

                var oldStatus = currentStatus;
                currentStatus = game.GetStatus();

                var inTime = MeasureExecutionTimeAndStopAtMaxTime(currentTrainer.Player, () => currentTrainer.Observe(oldStatus, currentStatus, moveResult, move));
                if (!inTime)
                    return currentTrainer.Player == Player.Player0
                        ? TimeboxedTrainingResult.Agent0TookTooLong
                        : TimeboxedTrainingResult.Agent1TookTooLong;

                inTime = MeasureExecutionTimeAndStopAtMaxTime(otherTrainer.Player, () => otherTrainer.Observe(oldStatus, currentStatus, moveResult, move));
                if (!inTime)
                    return otherTrainer.Player == Player.Player0
                        ? TimeboxedTrainingResult.Agent0TookTooLong
                        : TimeboxedTrainingResult.Agent1TookTooLong;
            }

            return TimeboxedTrainingResult.Done;
        }

        private bool MeasureExecutionTimeAndStopAtMaxTime(Player player, Action action)
        {
            var inTime = MeasureExecutionTimeAndStopAtMaxTime(player, () =>
            {
                action();
                return true;
            });
            return inTime;
        }

        private T MeasureExecutionTimeAndStopAtMaxTime<T>(Player player, Func<T> func)
        {
            var stopwatch = player == Player.Player0 ? _stopwatchAgent0 : _stopwatchAgent1;
            var timeLeft = _maxTime - stopwatch.Elapsed;

            stopwatch.Start();
            var result = func();
            stopwatch.Stop();
            return stopwatch.Elapsed > timeLeft ? default(T) : result;
        }
    }
}
