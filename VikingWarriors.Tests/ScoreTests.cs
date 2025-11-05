using Xunit;
using VikingWarriors.GameObjects;
using Microsoft.Xna.Framework;

namespace VikingWarriors.Tests;

public class ScoreTests
{
    [Fact]
    public void AddScore_ShouldAddHundredPoints_WhenPlayerCollectsCoin()
    {
        // Arrange
        var score = new Score(null, Vector2.Zero);
        int initialScore = score.CurrentScore;
        int pointsToAdd = 100;

        // Act
        score.AddScore(pointsToAdd);

        // Assert
        Assert.Equal(initialScore + pointsToAdd, score.CurrentScore);
    }
}
