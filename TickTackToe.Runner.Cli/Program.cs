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

            var status = runner.RunGame();

            runner.Moves.ForEach(x => Console.WriteLine($"{x.Player} : {x.Move}"));

            Console.WriteLine($"Game finished: {status.GameStatus}");
            Console.ReadLine();
        }
    }
}
