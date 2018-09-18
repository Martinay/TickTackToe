using System;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner.Cli
{
    public class RandomAgent : IAgent
    {
        public bool IsTraining { get; set; }

        public Move GetNextMove(Status status)
        {
            var random = new Random();
            return new Move(random.Next(3), random.Next(3));
        }

        public void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move)
        {
        }
    }
}