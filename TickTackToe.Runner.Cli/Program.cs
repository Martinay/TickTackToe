using System;
using System.Linq;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner.Cli
{
    class Program
    {
        private const int MaxTrainingEpisodes = 10;
        private const bool LimitTrainingTime = true;

        static void Main()
        {
            // Setup
            var maxTrainingTime = TimeSpan.FromMinutes(10);

            var agent0 = new RandomAgent();
            var agent1 = new RandomAgent();

            var startPlayerDeterminer = new RandomStartPlayerDeterminer();
            
            // Training
            Train(agent0, agent1, startPlayerDeterminer, maxTrainingTime);

            // Run trained agent
            var runner = new GameRunner(agent0, agent1, startPlayerDeterminer);

            var status = runner.RunGame();

            runner.Moves.ForEach(PrintMove);

            Console.WriteLine($"Game finished: {status.GameStatus}");
            Console.ReadLine();
        }

        private static void Train(IAgent agent0, IAgent agent1,
            IStartPlayerDeterminer startPlayerDeterminer, TimeSpan maxTrainingTime)
        {
            if (LimitTrainingTime)
            {
                var trainer = new TimeboxedTrainer(agent0, agent1, startPlayerDeterminer, maxTrainingTime);
                var result = trainer.Train(MaxTrainingEpisodes);
                Console.WriteLine($"Training result: {result}");
                Console.WriteLine();
            }
            else
            {
                var trainer = new Trainer(agent0, agent1, startPlayerDeterminer);
                trainer.Train(MaxTrainingEpisodes);
            }
        }

        private static void PrintMove(ExecutedMove executedMove)
        {
            Console.WriteLine($"######## Player {executedMove.Status.Player}");
            var rows = executedMove.Status.Field.Select(x => string.Join("  ", x));
            var field = string.Join(Environment.NewLine, rows);

            Console.WriteLine(field);
            Console.WriteLine($"Move : {executedMove.Move} result: {executedMove.MoveResult}");
            Console.WriteLine();
        }
    }
}
