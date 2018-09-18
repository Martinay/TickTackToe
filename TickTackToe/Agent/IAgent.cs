using TickTackToe.Game;

namespace TickTackToe.Agent
{
    public interface IAgent
    {
        Move GetNextMove(Status status);
        void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move);
    }
}
