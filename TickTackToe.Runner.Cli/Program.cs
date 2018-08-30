using System;

namespace TickTackToe.Runner.Cli
{
    class Program
    {
        static void Main()
        {
            var agent0 = new RandomAgent();
            var agent1 = new RandomAgent();
            var startPlayerDeterminer = new RandomStartPlayerDeterminer();

            var trainer = new Trainer(agent0, agent0, startPlayerDeterminer);
            trainer.Train(5);

            var runner = new GameRunner(agent0, agent1, startPlayerDeterminer);
            bool canContinue;
            do
            {
                canContinue = runner.MoveNext();
                Console.WriteLine("nextMove");
            } while (canContinue);

            Console.WriteLine($"Game finished {runner.Status.GameStatus}");
            Console.ReadLine();
        }
    }
}
