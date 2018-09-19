using System;
using System.Collections.Generic;
using System.Linq;
using TickTackToe.Game;

namespace TickTackToe.Agent.Team3
{
    public class AwesomeAgent : IAgent
    {
        public AwesomeAgent()
        {
            _random = new Random();
        }
        private IDictionary<int, List<Result>> _dictionary = new Dictionary<int, List<Result>>();
        private Random _random;
        public Player Player { get; set; }
        public bool IsTraining { get; set; }
        public Move GetNextMove(Status status) => IsTraining ? GetTrainingMove(status) : GetRealMove(status);
        public void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move)
        {
            //if (oldStatus.GetFieldIdentifier() == currentStatus.GetFieldIdentifier())
            //    return;
            var score = GetScore(currentStatus, moveResult);
            //if (score > 0)
            //    Debugger.Break();
            var resultStatus = oldStatus.GetFieldIdentifier() == currentStatus.GetFieldIdentifier() ? null : oldStatus;
            var result = new Result { Move = move, Score = score, OldStatus = resultStatus };
            var oldFieldIdentifier = currentStatus.GetFieldIdentifier();
            if (_dictionary.TryGetValue(oldFieldIdentifier, out var results))
            {
                if (results.All(x => x.Move.GetIdentifier() != move.GetIdentifier()))
                {
                    results.Add(result);
                }
            }
            _dictionary[oldFieldIdentifier] = results ?? new List<Result> { result };
            if (moveResult != MoveResult.Invalid && currentStatus.GameStatus != GameStatus.InGame)
            {
                ApplyScoreToOldResults(currentStatus, score);
            }
        }

        private void ApplyScoreToOldResults(Status oldStatus, int score)
        {
            //if (score>0 )
            //    Debugger.Break();
            // Console.WriteLine(oldStatus.GetFieldIdentifier());                                                           

            if (_dictionary.TryGetValue(oldStatus.GetFieldIdentifier(), out var results))
            {
                foreach (var result in results)
                {
                    //if (score>0 )
                    //    Debugger.Break();
                    checked
                    {
                        result.Score += score;
                    }
                    if (result.OldStatus == null || result.OldStatus.Field.All(x => x.All(y => y == Player.Undefined)))
                        continue;

                    ApplyScoreToOldResults(result.OldStatus, score);
                }
            }
        }
        private Move GetRealMove(Status status)
        {
            var xy = _dictionary.Values.SelectMany(x => x).Select(x => x.Score).Where(x => x > int.MinValue && x != 0).ToList();
            _dictionary.TryGetValue(status.GetFieldIdentifier(), out var results);
            return results == null
                ? new Move(_random.Next(3), _random.Next(3))
                : results.OrderByDescending(x => x.Score).Select(x => x.Move).First();
        }
        private Move GetTrainingMove(Status status)
        {
            if (_dictionary.TryGetValue(status.GetFieldIdentifier(), out var results))
            {
                var movesDone = results.Select(x => x.Move).ToList();
                var unknownMoves = GetMoves().Where(x =>
                {
                    return movesDone.All(a => a.GetIdentifier() != x.GetIdentifier());
                }).ToList();

                var move = unknownMoves.FirstOrDefault();
                return move ?? GetRandomOf(movesDone);
            }
            return new Move(0, 0);
        }
        private Move GetRandomOf(IList<Move> moves)
        {
            var numberOfMoves = moves.Count;
            var index = _random.Next(numberOfMoves);
            return moves[index];
        }
        private IEnumerable<Move> GetMoves()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    yield return new Move(x, y);
                }
            }
        }
        private int GetScore(Status status, MoveResult moveResult)
        {
            var we = Player;
            if (moveResult == MoveResult.Invalid)
            {
                return -100;
            }
            if (we == Player.Player0 && status.GameStatus == GameStatus.Player0Won ||
                we == Player.Player1 && status.GameStatus == GameStatus.Player1Won)
            {
                return 1;
            }
            if (status.GameStatus == GameStatus.Draw)
            {
                return 0;
            }
            if (we == Player.Player0 && status.GameStatus == GameStatus.Player1Won ||
                we == Player.Player1 && status.GameStatus == GameStatus.Player0Won)
            {
                return -1;
            }
            if (status.GameStatus == GameStatus.InGame)
                return 0;
            throw new NotImplementedException();
        }
    }
    internal class Result
    {
        public int Score { get; set; }
        public Move Move { get; set; }
        public Status OldStatus { get; set; }
    }
}
