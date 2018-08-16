using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TickTackToe.UnitTests
{
    [TestFixture]
    public class TickTackToeTests
    {

        [Test]
        public void IfPlayer0HasWonDiagonalBottomLeftTopRight_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 2, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 0, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player0Won);
            status.Player.Should().Be(Player.Player1);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 2);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer1HasWonDiagonalBottomLeftTopRight_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 2, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 0, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player1Won);
            status.Player.Should().Be(Player.Player0);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 2);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer0HasWonDiagonalTopLeftBottomRight_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player0Won);
            status.Player.Should().Be(Player.Player1);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 2);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer1HasWonDiagonalTopLeftBottomRight_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 2, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player1Won);
            status.Player.Should().Be(Player.Player0);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 2);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer0HasWonVertical_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 0, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 0, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player0Won);
            status.Player.Should().Be(Player.Player1);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 2);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer1HasWonVertical_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 0, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player1Won);
            status.Player.Should().Be(Player.Player0);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 2, 1);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer0HasWonHorizontal_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 0, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 0).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player0Won);
            status.Player.Should().Be(Player.Player1);

            // Act
            Action moveAction = () => game.Move(Player.Player1, 0, 0);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayer1HasWonHorizontal_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 0, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 2, 1).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player1Won);
            status.Player.Should().Be(Player.Player0);

            // Act
            Action moveAction = () => game.Move(Player.Player0, 0, 0);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfTheGameIsADraw_ThenTheResultShouldBeCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act && Assert
            game.Move(Player.Player0, 0, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 0, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 1, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 2, 0).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 1).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 0, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player1, 1, 2).Should().Be(MoveResult.Valid);
            game.Move(Player.Player0, 2, 2).Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Draw);
            status.Player.Should().Be(Player.Player1);
            
            // Act
            Action moveAction = () => game.Move(Player.Player1, 0, 0);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayerMakesAnMoveOnAnAlreadyAssignedField_ThenTheGameShouldEnd()
        {
            // Arrange
            var game = new TickTackToe();
            game.Move(Player.Player0, 0, 0);

            // Act
            var moveResult = game.Move(Player.Player1, 0, 0);

            // Assert
            moveResult.Should().Be(MoveResult.Invalid);

            var status = game.GetStatus();
            status.GameStatus.Should().Be(GameStatus.Player0Won);
        }
        
        [Test]
        public void IfGameEndedAndAPlayerMakesAnotherMove_ThenThereShouldBeAnException()
        {
            // Arrange
            var game = new TickTackToe();
            game.Move(Player.Player0, 0, 0);
            game.Move(Player.Player1, 0, 0);

            // Act
            Action moveAction = () => game.Move(Player.Player0, 0, 0);

            // Assert
            moveAction.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IfPlayerMakesAMoveOutOfBounds_ThenTheGameShouldThrowAnException()
        {
            // Arrange
            var game = new TickTackToe();

            // Act
            Action actionPositiveX = () => game.Move(Player.Player0, 0, 3);
            Action actionNegativeX = () => game.Move(Player.Player0, 0, -1);
            Action actionPostiveY = () => game.Move(Player.Player0, 3, 0);
            Action actionNegativeY = () => game.Move(Player.Player0, -1, 0);

            // Assert
            actionPositiveX.Should().Throw<ArgumentOutOfRangeException>();
            actionNegativeX.Should().Throw<ArgumentOutOfRangeException>();
            actionPostiveY.Should().Throw<ArgumentOutOfRangeException>();
            actionNegativeY.Should().Throw<ArgumentOutOfRangeException>();
        }
        
        [Test]
        public void IfWrongPlayerShouldMove_ThenThereShouldBeAnException()
        {
            // Arrange
            var game = new TickTackToe();

            // Act
            Action moveAction = () => game.Move(Player.Player1, 0, 1);

            // Assert
            moveAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void IfPlayerMakesAValidMove_ThenTheStatusIsCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act
            var moveResult = game.Move(Player.Player0, 0, 0);
            
            // Assert
            moveResult.Should().Be(MoveResult.Valid);

            var status = game.GetStatus();
            status.Player.Should().Be(Player.Player1);
            status.GameStatus.Should().Be(GameStatus.InGame);
            status.Field.Should().HaveCount(3);
            status.Field[0][0].Should().Be(Player.Player0);
            status.Field[0].Skip(1).Should().AllBeEquivalentTo(Player.Undefined);
            status.Field[1].Should().AllBeEquivalentTo(Player.Undefined);
            status.Field[2].Should().AllBeEquivalentTo(Player.Undefined);
        }

        [Test]
        public void IfGameIsInitialised_ThenTheStatusIsCorrect()
        {
            // Arrange
            var game = new TickTackToe();

            // Act
            var status = game.GetStatus();

            // Assert
            status.Player.Should().Be(Player.Player0);
            status.GameStatus.Should().Be(GameStatus.InGame);
            status.Field.Should().HaveCount(3);
            status.Field[0].Should().AllBeEquivalentTo(Player.Undefined);
            status.Field[1].Should().AllBeEquivalentTo(Player.Undefined);
            status.Field[2].Should().AllBeEquivalentTo(Player.Undefined);
        }
    }
}
