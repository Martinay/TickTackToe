﻿using System;
using TickTackToe.Game;

namespace TickTackToe.Runner
{
    public class RandomStartPlayerDeterminer : IStartPlayerDeterminer
    {
        public Player GetStartPlayer()
        {
            return new Random().Next(2) == 0 ? Player.Player0 : Player.Player1;
        }
    }
}