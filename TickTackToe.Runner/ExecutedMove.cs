using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class ExecutedMove
    {
        public ExecutedMove(Player player, Move move)
        {
            Player = player;
            Move = move;
        }

        public Move Move { get; }
        public Player Player { get; }
    }
}