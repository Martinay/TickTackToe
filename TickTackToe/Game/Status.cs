using System;
using System.Collections.Generic;
using System.Text;

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

        private int GetFieldIdentifier()
        {
            //Copyright R2D
            var sb = new StringBuilder();
            foreach (var outer in Field)
            foreach (var inner in outer)
                sb.Append((int)inner);

            return sb.ToString().GetHashCode();
        }
    }
}