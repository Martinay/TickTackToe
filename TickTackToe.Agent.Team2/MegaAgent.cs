using System;
using System.Collections.Generic;
using System.Linq;
using TickTackToe.Game;

namespace TickTackToe.Agent.Team2
{
    // by JM, CL
    public class MegaAgent : IAgent
    {
        class MoveState
        {
            public int Weight { get; set; }
            public Move Move { get; set; }
        }

        public Player Player { get; set; }
        public bool IsTraining { get; set; }

        private readonly Dictionary<Status, MoveState> _megaDict;
        private readonly Dictionary<Status, MoveState> _roundDict;

        public MegaAgent()
        {
            _megaDict = new Dictionary<Status, MoveState>(new StatusComparer());
            _roundDict = new Dictionary<Status, MoveState>(new StatusComparer());
        }

        public Move GetNextMove(Status status)
        {
            //if (status.Field.SelectMany(l => l.ToList()).All(f => f == Player.Undefined))
            //    return new Move(1, 1);

            var random = new Random((int)DateTime.Now.Ticks);

            if (!IsTraining)
            {
                if (_megaDict.ContainsKey(status))
                {
                    var move = _megaDict[status];

                    if (move.Weight > 0)
                        return move.Move;
                }
            }
            else
            {
                if (random.Next(1, 100) > 20 && _megaDict.ContainsKey(status))
                {
                    var move = _megaDict[status];

                    if (move.Weight >= 0)
                        return move.Move;
                }
            }

            bool isValidMove = false;
            // random feld
            var movex = random.Next(3);
            var movey = random.Next(3);

            isValidMove = status.Field[movex][movey] == Player.Undefined;

            if (!isValidMove)
            {
                int tempx;
                int tempy;

                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        tempy = movey + y;
                        tempy = tempy % 3;

                        tempx = movex + x;
                        tempx = tempx % 3;

                        isValidMove = status.Field[tempx][tempy] == Player.Undefined;
                        if (isValidMove)
                        {
                            movex = tempx;
                            movey = tempy;
                            break;
                        }

                    }

                    if (isValidMove)
                        break;
                }
            }

            return new Move(movex, movey);
        }

        public void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move)
        {
            if (currentStatus.GameStatus == GameStatus.InGame && oldStatus.Player != Player)
                return;

            var weight = WeightLastMove(currentStatus);


            if (moveResult == MoveResult.Valid)
            {

                _roundDict[oldStatus] = new MoveState { Move = move, Weight = weight };
            }

            if (currentStatus.GameStatus != GameStatus.InGame)
            {
                RoundCompleted(currentStatus, weight);
            }
        }

        private void RoundCompleted(Status currentStatus, int weight)
        {
            var hasChanged = AdjustWeight(currentStatus, ref weight);

            if (hasChanged)
            {
                foreach (var round in _roundDict.ToList())
                {
                    var oldmove = _roundDict[round.Key];
                    if (oldmove.Weight < weight)
                        _roundDict[round.Key] = new MoveState { Move = round.Value.Move, Weight = weight };
                }
            }

            MergeMegaDictIfNotExistOrUpdateIfHeavier(_roundDict);

            _roundDict.Clear();
        }

        private bool AdjustWeight(Status currentStatus, ref int weight)
        {
            var hasChanged = false;

            if (currentStatus.GameStatus == GameStatus.Player0Won && Player == Player.Player1)
            {
                hasChanged = true;
                weight--;
            }

            if (currentStatus.GameStatus == GameStatus.Player1Won && Player == Player.Player0)
            {
                hasChanged = true;
                weight--;
            }

            if (currentStatus.GameStatus == GameStatus.Player1Won && Player == Player.Player1)
            {
                hasChanged = true;
                weight++;
            }

            if (currentStatus.GameStatus == GameStatus.Player0Won && Player == Player.Player0)
            {
                hasChanged = true;
                weight++;
            }

            return hasChanged;
        }

        private void MergeMegaDictIfNotExistOrUpdateIfHeavier(Dictionary<Status, MoveState> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                var moveExist = _megaDict.ContainsKey(keyValuePair.Key);
                if (moveExist)
                {
                    var oldWeight = _megaDict[keyValuePair.Key].Weight;
                    if (keyValuePair.Value.Weight > oldWeight)
                    {
                        _megaDict[keyValuePair.Key] = new MoveState { Weight = keyValuePair.Value.Weight, Move = keyValuePair.Value.Move };
                    }
                }
                else
                {
                    _megaDict.Add(keyValuePair.Key, new MoveState { Weight = keyValuePair.Value.Weight, Move = keyValuePair.Value.Move });
                }
            }
        }

        private int WeightLastMove(Status status)
        {
            if (status.GameStatus == GameStatus.Player0Won && Player == Player.Player1)
            {
                return -1;
            }

            if (status.GameStatus == GameStatus.Player1Won && Player == Player.Player0)
            {
                return -1;
            }

            if (status.GameStatus == GameStatus.Player1Won && Player == Player.Player1)
            {
                return 1;
            }

            if (status.GameStatus == GameStatus.Player0Won && Player == Player.Player0)
            {
                return 1;
            }

            return status.GameStatus == GameStatus.Draw
                ? 1
                : 0;
        }
    }
}