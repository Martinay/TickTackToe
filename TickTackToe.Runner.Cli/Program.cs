using System;
using System.Linq;

namespace TickTackToe.Runner.Cli
{
    class Program
    {
        private const int Episodes = 10;

        static void Main()
        {
            var agent0 = new RandomAgent();
            var agent1 = new RandomAgent();
            var startPlayerDeterminer = new RandomStartPlayerDeterminer();

            var trainer = new Trainer(agent0, agent0, startPlayerDeterminer);
            trainer.Train(Episodes);

            var runner = new GameRunner(agent0, agent1, startPlayerDeterminer);

            var status = runner.RunGame();

            runner.Moves.ForEach(PrintMove);

            Console.WriteLine($"Game finished: {status.GameStatus}");
            Console.ReadLine();
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
