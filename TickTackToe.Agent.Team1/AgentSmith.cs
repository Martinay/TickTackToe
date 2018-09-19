using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkCSharp;
using NeuralNetworkCSharp.ActivationFunctions;
using NeuralNetworkCSharp.InputFunctions;
using TickTackToe.Game;

namespace TickTackToe.Agent.Team1
{
    // by rk, dh, dh
    public class AgentSmith : IAgent
    {
        private readonly SimpleNeuralNetwork _network;

        private readonly Random _random = new Random(Environment.TickCount);
        private readonly Stack<StatusMove> _lastMoves = new Stack<StatusMove>();
        private readonly List<TrainDataOneGame> _trainDatas = new List<TrainDataOneGame>();
        private int _totalGames;

        public Player Player { get; set; }
        public bool IsTraining { get; set; }

        public AgentSmith()
        {
            _network = new SimpleNeuralNetwork(11);
            var layerFactory = new NeuralLayerFactory();
            _network.AddLayer(layerFactory.CreateNeuralLayer(10, new RectifiedActivationFuncion(), new WeightedSumFunction()));
            _network.AddLayer(layerFactory.CreateNeuralLayer(1, new SigmoidActivationFunction(1), new WeightedSumFunction()));
        }

        public Move GetNextMove(Status status)
        {
            var validMoves = GetValidMoves(status.Field);

            if (IsTraining)
            {
                // random move
                var index = _random.Next(validMoves.Count);
                var move = validMoves[index];
                _lastMoves.Push(new StatusMove(status, move));
                return move;
            }

            var rates = new Dictionary<Move, double>();

            var fieldValues = GetInputs(status.Field);
            foreach (var validMove in validMoves)
            {
                fieldValues[9] = validMove.X;
                fieldValues[10] = validMove.Y;

                _network.PushInputValues(fieldValues);
                var rate = _network.GetOutput()[0];
                rates.Add(validMove, rate);
            }

            var bestMove = rates
                .OrderByDescending(x => x.Value)
                .First();
            return bestMove.Key;
        }

        public void Observe(Status oldStatus, Status currentStatus, MoveResult moveResult, Move move)
        {
            if (moveResult == MoveResult.Invalid)
                return;// throw new InvalidOperationException("Got invalid move!!!");

            if (!IsTraining)
                return;

            if (oldStatus.Player == Player)
                return;

            switch (currentStatus.GameStatus)
            {
                case GameStatus.InGame:
                    return;
                case GameStatus.Player0Won:
                    if (Player == Player.Player0)
                    {
                        // we win
                        GameFinished(1000);
                    }
                    else
                    {
                        // we lose
                        GameFinished(-500);
                    }
                    break;
                case GameStatus.Player1Won:
                    if (Player == Player.Player1)
                    {
                        // we win
                        GameFinished(1000);
                    }
                    else
                    {
                        // we lose
                        GameFinished(-500);
                    }
                    break;
                case GameStatus.Draw:
                    GameFinished(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _lastMoves.Clear();
        }

        private void GameFinished(int weight)
        {
            _totalGames++;

            var inputData = _lastMoves
                .Select(x =>
                {
                    var fieldValues = GetInputs(x.Status.Field);
                    fieldValues[9] = x.Move.X;
                    fieldValues[10] = x.Move.Y;
                    return fieldValues;
                })
                .ToArray();

            _trainDatas.Add(new TrainDataOneGame(inputData, weight > 0));

            if (_trainDatas.Count > 0 && _trainDatas.Count % 100 == 0)
            {
                foreach (var trainDataOneGame in _trainDatas)
                {
                    var expectedData = Enumerable.Range(0, trainDataOneGame.Input.Length)
                        .Select(x => new double[] { trainDataOneGame.Won ? 0 : 1 })
                        .ToArray();
                    _network.PushExpectedValues(expectedData);

                    _network.Train(trainDataOneGame.Input, 1);
                }
            }
        }

        private double[] GetInputs(List<List<Player>> field)
        {
            var inputs = new double[11];
            for (var outerindex = 0; outerindex < field.Count; outerindex++)
            {
                var outer = field[outerindex];
                for (var innerIndex = 0; innerIndex < outer.Count; innerIndex++)
                {
                    double input;
                    var value = outer[innerIndex];
                    switch (value)
                    {
                        case Player.Undefined:
                            input = 0;
                            break;
                        default:
                            input = Player == value ? 1 : -1;
                            break;
                    }

                    var position = outerindex * 3 + innerIndex;
                    inputs[position] = input;
                }
            }

            return inputs;
        }

        private List<Move> GetValidMoves(List<List<Player>> field)
        {
            var validMoves = new List<Move>();
            for (var outerindex = 0; outerindex < field.Count; outerindex++)
            {
                var outer = field[outerindex];
                for (var innerIndex = 0; innerIndex < outer.Count; innerIndex++)
                {
                    var value = outer[innerIndex];
                    if (value == Player.Undefined)
                        validMoves.Add(new Move(innerIndex, outerindex));
                }
            }

            return validMoves;
        }

        public void Dump()
        {
            Console.WriteLine("TrainDatas entries: " + _trainDatas.Count);
            Console.WriteLine("TotalGames: " + _totalGames);
        }

        private class StatusMove
        {
            public StatusMove(Status status, Move move)
            {
                Status = status;
                Move = move;
            }

            public Status Status { get; }
            public Move Move { get; }
        }

        private class TrainDataOneGame
        {
            public TrainDataOneGame(double[][] input, bool won)
            {
                Input = input;
                Won = won;
            }

            public double[][] Input { get; set; }
            public bool Won { get; set; }
        }
    }
}
