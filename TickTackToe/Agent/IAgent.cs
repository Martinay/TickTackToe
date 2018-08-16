using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe
{
    public interface IAgent
    {
        Move GetNextMove(Status status);
        void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move);
    }
}
