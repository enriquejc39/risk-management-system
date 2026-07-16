using RiskManagement.Core.Entities;
using Xunit;

namespace RiskManagement.UnitTests;

public class RiskTests
{
    [Theory]
    [InlineData(5, 5, 25, "Critical")]
    [InlineData(4, 4, 16, "High")]
    [InlineData(3, 3, 9, "Medium")]
    [InlineData(2, 2, 4, "Low")]
    [InlineData(1, 1, 1, "VeryLow")]
    public void RiskScore_CalculatesCorrectLevel(int probability, int impact, int expectedScore, string expectedLevel)
    {
        var risk = new Risk
        {
            Probability = probability,
            Impact = impact
        };

        Assert.Equal(expectedScore, risk.RiskScore);
        Assert.Equal(expectedLevel, risk.Level.ToString());
    }

    [Fact]
    public void Risk_NewRisk_ShouldHaveDraftStatus()
    {
        var risk = new Risk();

        Assert.Equal(RiskStatus.Draft, risk.Status);
    }
}
