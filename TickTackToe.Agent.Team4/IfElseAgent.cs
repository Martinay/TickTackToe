using System;
using System.Collections.Generic;
using System.Linq;
using TickTackToe.Game;

namespace TickTackToe.Agent.Team4
{
    public class IfElseAgent : IAgent
    {
        public Player Player { get; set; }
        public bool IsTraining { get; set; }
        public Move GetNextMove(Status status)
        {
            if (status.Field[1][1] == Player.Undefined)
                return new Move(1, 1);
            if (status.Field[0][0] == Player.Undefined)
                return new Move(0, 0);
            if (status.Field[2][2] == Player.Undefined)
                return new Move(2, 2);
            if (status.Field[0][2] == Player.Undefined)
                return new Move(0, 2);
            if (status.Field[2][0] == Player.Undefined)
                return new Move(2, 0);
            if (status.Field[1][0] == Player.Undefined)
                return new Move(1, 0);
            if (status.Field[0][1] == Player.Undefined)
                return new Move(0, 1);
            if (status.Field[1][2] == Player.Undefined)
                return new Move(1, 2);
            if (status.Field[2][1] == Player.Undefined)
                return new Move(2, 1);

            return new Move(0,0);
        }

        public void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move)
        {
        }

        public WinState IsOtherCloseToWin(List<List<Player>> field)
        {
            var otherPlayer = Player == Player.Player0 ? Player.Player1 : Player.Player0;

            if (field[0].All(x => x == otherPlayer))
                return WinState.Row0;
            if (field[1].All(x => x == otherPlayer))
                return WinState.Row1;
            if (field[2].All(x => x == otherPlayer))
                return WinState.Row2;

            if (field.Select(x => x[0]).All(x => x == otherPlayer))
                return WinState.Column0;
            if (field.Select(x => x[0]).All(x => x == otherPlayer))
                return WinState.Column1;
            if (field.Select(x => x[0]).All(x => x == otherPlayer))
                return WinState.Column2;

            if (field[0][0] == otherPlayer && field[1][1] == otherPlayer && field[2][2] == otherPlayer)
                return WinState.VerticalTopRightToBottomLeft;

            if (field[0][2] == otherPlayer && field[1][1] == otherPlayer && field[2][0] == otherPlayer)
                return WinState.VerticalBottomRightToTopLeft;

            return WinState.None;
        }

        public enum WinState
        {
            Row0,
            Row1,
            Row2,

            Column0,
            Column1,
            Column2,

            VerticalTopRightToBottomLeft,
            VerticalBottomRightToTopLeft,

            None
        }
    }
}
