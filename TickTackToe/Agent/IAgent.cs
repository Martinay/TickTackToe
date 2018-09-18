using TickTackToe.Game;

namespace TickTackToe.Agent
{
    public interface IAgent
    {
        Player Player { get; set; }
        bool IsTraining { get; set; }

        Move GetNextMove(Status status);
        void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move);
    }
}
