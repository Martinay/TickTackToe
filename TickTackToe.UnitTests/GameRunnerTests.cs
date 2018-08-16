using FluentAssertions;
using Moq;
using NUnit.Framework;
using TickTackToe.Agent;
using TickTackToe.Game;

namespace TickTackToe.UnitTests
{
    [TestFixture]
    public class GameRunnerTests
    {
        [Test]
        public void IfARunnerIsInitialized_ThenAgent0ShouldDoTheFirstMove()
        {
            // Arrange
            var mockedAgent0 = new Mock<IAgent>();
            mockedAgent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(0,0));
            var mockedAgent1 = new Mock<IAgent>();
            var runner = GetRunner(mockedAgent0.Object, mockedAgent1.Object);

            // Act
            var canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Once);
            mockedAgent1.VerifyNoOtherCalls();
        }
        
        [Test]
        public void IfAgent0AndAgent1_ShouldBeCalledAfterEachOther()
        {
            // Arrange
            var mockedAgent0 = new Mock<IAgent>();
            mockedAgent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(0,0));
            var mockedAgent1 = new Mock<IAgent>();
            mockedAgent1.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(1,1));
            var runner = GetRunner(mockedAgent0.Object, mockedAgent1.Object);

            // Act
            var canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Once);
            mockedAgent1.VerifyNoOtherCalls();

            // Act
            canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.VerifyNoOtherCalls();
            mockedAgent1.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Once);
        }
        
        [Test]
        public void IfAgent0Won_ThenTheMoveNextMethodShouldReturnFalse()
        {
            // Arrange
            var mockedAgent0 = new Mock<IAgent>();
            mockedAgent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(0, 0));
            var mockedAgent1 = new Mock<IAgent>();
            mockedAgent1.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(1, 1));
            var runner = GetRunner(mockedAgent0.Object, mockedAgent1.Object);

            // Act
            var canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Once);
            mockedAgent1.VerifyNoOtherCalls();

            // Act
            canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.VerifyNoOtherCalls();
            mockedAgent1.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Once);
            
            // Act
            mockedAgent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(1, 0));
            canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Exactly(2));
            mockedAgent1.VerifyNoOtherCalls();
            
            // Act
            mockedAgent1.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(1, 2));
            canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeTrue();
            mockedAgent0.VerifyNoOtherCalls();
            mockedAgent1.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Exactly(2));
            
            // Act
            mockedAgent0.Setup(x => x.GetNextMove(It.IsAny<Status>())).Returns(new Move(2, 0));
            canContinue = runner.MoveNext();

            // Assert
            canContinue.Should().BeFalse();
            mockedAgent0.Verify(x => x.GetNextMove(It.IsAny<Status>()), Times.Exactly(3));
            mockedAgent1.VerifyNoOtherCalls();
        }

        private GameRunner GetRunner(IAgent agent0, IAgent agent1)
        {
            return new GameRunner(agent0, agent1);
        }
    }
}
