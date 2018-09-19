using System;
using System.Linq;
using TickTackToe.Agent;
using TickTackToe.Agent.Team1;
using TickTackToe.Agent.Team2;
using TickTackToe.Agent.Team3;
using TickTackToe.Agent.Team4;
using TickTackToe.Game;

namespace TickTackToe.Runner.Cli
{
    class Program
    {
        private const int MaxTrainingEpisodes = 100;
        private const int TournamentRuns = 1000;
        private const bool LimitTrainingTime = true;

        static void Main()
        {
            // Setup
            var maxTrainingTime = TimeSpan.FromMinutes(2);

            var agent0 = new RandomAgent();
            var agent1 = new IfElseAgent();

            var startPlayerDeterminer = new RandomStartPlayerDeterminer();
            
            // Training
            Train(agent0, agent1, startPlayerDeterminer, maxTrainingTime);

            // Run trained agent
            var runner = new GameRunner(agent0, agent1, startPlayerDeterminer);

            var status = runner.RunGame();

            runner.Moves.ForEach(PrintMove);

            Console.WriteLine($"Game finished: {status.GameStatus}");

            // Running tournament
            Console.WriteLine("starting tournament mode");
            var tournamentRunner = new TournamentRunner(agent0, agent1, startPlayerDeterminer);
            var result = tournamentRunner.RunGame(TournamentRuns);
            Console.WriteLine($"player 0 : {result.Player0Won} player 1 : {result.Player1Won} draw : {result.Draw}");

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
