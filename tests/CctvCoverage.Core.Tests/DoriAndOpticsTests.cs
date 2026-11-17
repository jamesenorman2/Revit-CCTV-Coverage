using CctvCoverage.Core.Optics;
using CctvCoverage.Core.Standards;
using Xunit;

namespace CctvCoverage.Core.Tests;

// Reference camera used throughout: 1920 px wide, 90° HFOV → tan(45°) = 1, so
// max slant distance = 1920 / (2 · PPM). Easy to check by hand.
public class DoriAndOpticsTests
{
    private const int Px = 1920;
    private static readonly double Hfov90 = Lens.DegToRad(90);

    [Theory]
    [InlineData(DoriLevel.Identify, 3.84)]
    [InlineData(DoriLevel.Recognise, 7.68)]
    [InlineData(DoriLevel.Observe, 15.36)]
    [InlineData(DoriLevel.Detect, 38.4)]
    [InlineData(DoriLevel.Monitor, 76.8)]
    public void MaxSlantDistance_matches_hand_calculation(DoriLevel level, double expected)
    {
        var d = PixelDensity.MaxSlantDistance(Px, Hfov90, DoriLevels.RequiredPixelsPerMetre(level));
        Assert.Equal(expected, d, 6);
    }

    [Fact]
    public void AtDistance_is_inverse_of_MaxSlantDistance()
    {
        Assert.Equal(250, PixelDensity.AtDistance(Px, Hfov90, 3.84), 6);
    }

    [Theory]
    [InlineData(3.84, 2.0, 3.2780482)]
    [InlineData(1.0, 2.0, 0.0)] // band never reaches the target plane
    public void PlanDistanceFromSlant(double slant, double drop, double expected)
    {
        Assert.Equal(expected, PixelDensity.PlanDistanceFromSlant(slant, drop), 6);
    }

    [Fact]
    public void Hfov_from_focal_length()
    {
        // 6.4 mm sensor width, 3.2 mm lens → 2·atan(1) = 90°
        Assert.Equal(90, Lens.RadToDeg(Lens.HorizontalFovFromFocalLength(3.2, 6.4)), 6);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(180.0)]
    [InlineData(200.0)]
    public void Rejects_invalid_hfov(double degrees)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PixelDensity.AtDistance(Px, Lens.DegToRad(degrees), 5));
    }

    [Theory]
    [InlineData("identify", DoriLevel.Identify)]
    [InlineData(" Recognize ", DoriLevel.Recognise)]
    [InlineData("DETECT", DoriLevel.Detect)]
    public void Parses_levels(string text, DoriLevel expected) => Assert.Equal<DoriLevel?>(expected, DoriLevels.TryParse(text));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("banana")]
    public void Parse_returns_null_for_unknown(string? text) => Assert.Null(DoriLevels.TryParse(text));
}
