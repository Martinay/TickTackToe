using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner.UnitTests
{
    [TestFixture]
    public class GameRunnerTests
    {
        private Mock<IStartPlayerDeterminer> _mockedStartPlayerDeterminer;

        [SetUp]
        public void Setup()
        {
            _mockedStartPlayerDeterminer = new Mock<IStartPlayerDeterminer>();
            _mockedStartPlayerDeterminer.Setup(x => x.GetStartPlayer()).Returns(Player.Player0);
        }
        
        [Test]
        public void IfTheTrainerIsCreated_ThenThePlayerIsSetCorrectly()
        {
            // Arrange
            var agent0 = new Mock<IAgent>();
            var playerAgent0 = Player.Undefined;
            agent0.SetupSet(x => x.Player = It.IsAny<Player>()).Callback<Player>(value => playerAgent0 = value);

            var agent1 = new Mock<IAgent>();
            var playerAgent1 = Player.Undefined;
            agent1.SetupSet(x => x.Player = It.IsAny<Player>()).Callback<Player>(value => playerAgent1 = value);

            // Act
            var trainer = GetRunner(agent0.Object, agent1.Object);

            // Assert
            playerAgent0.Should().Be(Player.Player0);
            playerAgent1.Should().Be(Player.Player1);
        }

        [Test]
        public void IfTheTrainerIsCreated_ThenTheIsTrainingFlagIsUnset()
        {
            // Arrange

            var agent0 = new Mock<IAgent>();
            var agent0IsTraining = false;
            agent0.SetupSet(x => x.IsTraining = It.IsAny<bool>()).Callback<bool>(value => agent0IsTraining = value);
            var agent1 = new Mock<IAgent>();
            var agent1IsTraining = false;
            agent1.SetupSet(x => x.IsTraining = It.IsAny<bool>()).Callback<bool>(value => agent1IsTraining = value);

            // Act
            var trainer = GetRunner(agent0.Object, agent1.Object);

            // Assert
            agent0IsTraining.Should().BeFalse();
            agent1IsTraining.Should().BeFalse();
        }

        [Test]
        public void IfRunGameIsExecuted_ThenItShouldRunTillTheEnd()
        {
            // Arrange
            var agent0Counter = 0;
            var agent0 = new Mock<IAgent>();
            var agent0Move0 = new Move(0, 0);
            var agent0Move1 = new Move(1, 0);
            var agent0Move2 = new Move(2, 0);

            agent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(() =>
            {
                Move move;
                switch (agent0Counter)
                {
                    case 0:
                        move = agent0Move0;
                        break;
                    case 1:
                        move = agent0Move1;
                        break;
                    case 2:
                        move = agent0Move2;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
                agent0Counter++;
                return move;
            });

            var agent1Counter = 0;
            var agent1Move0 = new Move(0, 1);
            var agent1Move1 = new Move(1, 1);

            var agent1 = new Mock<IAgent>();
            agent1.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(() =>
            {
                Move move;
                switch (agent1Counter)
                {
                    case 0:
                        move = agent1Move0;
                        break;
                    case 1:
                        move = agent1Move1;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
                agent1Counter++;
                return move;
            });
            var runner = GetRunner(agent0.Object, agent1.Object);

            // Act
            var status = runner.RunGame();

            // Assert
            status.GameStatus.Should().Be(GameStatus.Player0Won);
            runner.Moves.Should().HaveCount(5);

            var firstMove = runner.Moves.First();
            firstMove.Status.Player.Should().Be(Player.Player0);
            firstMove.Move.Should().Be(agent0Move0);
            var secondMove = runner.Moves[1];
            secondMove.Status.Player.Should().Be(Player.Player1);
            secondMove.Move.Should().Be(agent1Move0);
            var thirdMove = runner.Moves[2];
            thirdMove.Status.Player.Should().Be(Player.Player0);
            thirdMove.Move.Should().Be(agent0Move1);
            var forthMove = runner.Moves[3];
            forthMove.Status.Player.Should().Be(Player.Player1);
            forthMove.Move.Should().Be(agent1Move1);
            var fifthMove = runner.Moves[4];
            fifthMove.Status.Player.Should().Be(Player.Player0);
            fifthMove.Move.Should().Be(agent0Move2);

        }

        private GameRunner GetRunner(IAgent agent0, IAgent agent1)
        {
            return new GameRunner(agent0, agent1, _mockedStartPlayerDeterminer.Object);
        }
    }
}
