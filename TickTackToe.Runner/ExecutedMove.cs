using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class ExecutedMove
    {
        public ExecutedMove(Status status, Move move, MoveResult moveResult)
        {
            Status = status;
            Move = move;
            MoveResult = moveResult;
        }

        public Move Move { get; }
        public MoveResult MoveResult { get; }
        public Status Status { get; }
    }
}