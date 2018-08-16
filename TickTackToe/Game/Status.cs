using System.Collections.Generic;

namespace TickTackToe.Game
{
    public class Status
    {
        public Status(GameStatus gameStatus, Player player, List<List<Player>> field)
        {
            GameStatus = gameStatus;
            Player = player;
            Field = field;
        }

        public GameStatus GameStatus { get; }
        public Player Player { get; }
        public List<List<Player>> Field { get; }
    }
}