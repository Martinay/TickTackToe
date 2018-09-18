using System;
using System.Threading;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.Runner.UnitTests
{
    [TestFixture]
    public class TimeboxedTrainerTests
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
            var trainer = GetTrainer(agent0.Object, agent1.Object, TimeSpan.FromSeconds(1));

            // Assert
            playerAgent0.Should().Be(Player.Player0);
            playerAgent1.Should().Be(Player.Player1);
        }

        [Test]
        public void IfTheTrainerIsCreate_ThenTheIsTrainingFlagIsSet()
        {
            // Arrange

            var agent0 = new Mock<IAgent>();
            var agent0IsTraining = false;
            agent0.SetupSet(x => x.IsTraining = It.IsAny<bool>()).Callback<bool>(value => agent0IsTraining = value);
            var agent1 = new Mock<IAgent>();
            var agent1IsTraining = false;
            agent1.SetupSet(x => x.IsTraining = It.IsAny<bool>()).Callback<bool>(value => agent1IsTraining = value);

            // Act
            var trainer = GetTrainer(agent0.Object, agent1.Object, TimeSpan.FromSeconds(1));

            // Assert
            agent0IsTraining.Should().BeTrue();
            agent1IsTraining.Should().BeTrue();
        }

        [Test]
        public void IfAgent0TakesTooLong_ThenTheTrainingShouldBeAborted()
        {
            // Arrange
            var agent0 = new Mock<IAgent>();
            agent0.Setup(x =>
                    x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), It.IsAny<MoveResult>(), It.IsAny<Move>()))
                .Callback(() => Thread.Sleep(TimeSpan.FromSeconds(20)));
            var agent1 = new Mock<IAgent>();

            var trainer = GetTrainer(agent0.Object, agent1.Object, TimeSpan.FromSeconds(2));

            // Act
            var trainingResult = trainer.Train(1);

            // Assert
            trainingResult.Should().Be(TimeboxedTrainingResult.Agent0TookTooLong);
        }

        [Test]
        public void IfAgent1TakesTooLong_ThenTheTrainingShouldBeAborted()
        {
            // Arrange
            var agent0 = new Mock<IAgent>();
            agent0.Setup(x =>
                x.GetNextMove(It.IsAny<Status>()))
                .Returns(new Move(0, 0));

            var agent1 = new Mock<IAgent>();
            agent1.Setup(x =>
                    x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), It.IsAny<MoveResult>(), It.IsAny<Move>()))
                .Callback(() => Thread.Sleep(TimeSpan.FromSeconds(20)));

            var trainer = GetTrainer(agent0.Object, agent1.Object, TimeSpan.FromSeconds(2));

            // Act
            var trainingResult = trainer.Train(1);

            // Assert
            trainingResult.Should().Be(TimeboxedTrainingResult.Agent1TookTooLong);
        }

        [Test]
        public void IfAnAgentShouldBeTrainedInTime_ThenTheGetMoveAndTheObserveMethodShouldBeCalled()
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

            var trainer = GetTrainer(agent0.Object, agent1.Object, TimeSpan.FromMinutes(10));

            // Act
            var trainingResult = trainer.Train(1);

            // Assert
            trainingResult.Should().Be(TimeboxedTrainingResult.Done);

            agent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Exactly(3));
            agent1.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Exactly(2));
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, It.IsAny<Move>()), Times.Exactly(5));
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move0), Times.Once);
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move1), Times.Once);
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move2), Times.Once);
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent1Move0), Times.Once);
            agent0.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent1Move1), Times.Once);
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, It.IsAny<Move>()), Times.Exactly(5));
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent1Move0), Times.Once);
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent1Move1), Times.Once);
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move0), Times.Once);
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move1), Times.Once);
            agent1.Verify(x => x.Observe(It.IsAny<Status>(), It.IsAny<Status>(), MoveResult.Valid, agent0Move2), Times.Once);
        }

        private TimeboxedTrainer GetTrainer(IAgent agent0, IAgent agent1, TimeSpan timeout)
        {
            return new TimeboxedTrainer(agent0, agent1, _mockedStartPlayerDeterminer.Object, timeout);
        }
    }
}
