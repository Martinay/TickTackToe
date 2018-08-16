using System;

namespace TickTackToe.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var agent0 = new RandomAgent();
            var agent1 = new RandomAgent();

            var trainer = new Trainer(agent0, agent0);
            trainer.Train(5);

            var runner = new GameRunner(agent0, agent1);
            bool canContinue;
            do
            {
                canContinue = runner.MoveNext();
                Console.WriteLine("nextMove");
            } while (canContinue);
            
            Console.ReadLine();
        }
    }
}
