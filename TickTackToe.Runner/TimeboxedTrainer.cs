using System;
using System.Diagnostics;
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
                var agent = currentStatus.Player == Player.Player0 ? _agent0 : _agent1;
                var move = MeasureExecutionTimeAndStopAtMaxTime(currentStatus.Player, () => agent.GetNextMove(currentStatus));

                if (move == null)
                    return currentStatus.Player == Player.Player0
                        ? TimeboxedTrainingResult.Agent0TookTooLong
                        : TimeboxedTrainingResult.Agent1TookTooLong;

                var moveResult = game.Move(currentStatus.Player, move.X, move.Y);

                var oldStatus = currentStatus;
                currentStatus = game.GetStatus();

                var inTime = MeasureExecutionTimeAndStopAtMaxTime(currentStatus.Player, () => agent.Observe(oldStatus, currentStatus, moveResult, move));
                if (!inTime)
                    return currentStatus.Player == Player.Player0
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
            var task = Task.Run(func);
            var finishedInTime = Task.WaitAll(new Task[] {task}, timeLeft);
            stopwatch.Stop();
            if (!finishedInTime)
                return default(T);

            return task.Result;
        }
    }
}
