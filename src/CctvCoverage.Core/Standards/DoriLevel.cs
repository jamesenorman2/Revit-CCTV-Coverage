namespace CctvCoverage.Core.Standards;

/// <summary>
/// Observation categories from BS EN IEC 62676-4 (pixel density on the target).
/// Ordered from the LEAST demanding to the MOST demanding.
/// </summary>
public enum DoriLevel
{
    Monitor = 0,
    Detect = 1,
    Observe = 2,
    Recognise = 3,
    Identify = 4,
    Inspect = 5,
}

public static class DoriLevels
{
    /// <summary>
    /// Minimum pixels per metre (horizontal) for each level, per EN 62676-4:2015 Table/Annex.
    /// If your organisation uses different values (e.g. Identify 250 vs. 300), change them HERE only.
    /// </summary>
    public static double RequiredPixelsPerMetre(DoriLevel level) => level switch
    {
        DoriLevel.Monitor => 12.5,
        DoriLevel.Detect => 25,
        DoriLevel.Observe => 62.5,
        DoriLevel.Recognise => 125,
        DoriLevel.Identify => 250,
        DoriLevel.Inspect => 1000,
        _ => throw new ArgumentOutOfRangeException(nameof(level)),
    };

    /// <summary>All levels, most demanding first (the order bands are built outwards from the camera).</summary>
    public static IReadOnlyList<DoriLevel> MostDemandingFirst { get; } = new[]
    {
        DoriLevel.Inspect, DoriLevel.Identify, DoriLevel.Recognise,
        DoriLevel.Observe, DoriLevel.Detect, DoriLevel.Monitor,
    };

    /// <summary>Case-insensitive parse that also accepts US spelling "Recognize". Returns null if unknown/blank.</summary>
    public static DoriLevel? TryParse(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var t = text.Trim();
        if (t.Equals("Recognize", StringComparison.OrdinalIgnoreCase)) return DoriLevel.Recognise;
        return Enum.TryParse<DoriLevel>(t, ignoreCase: true, out var level) ? level : null;
    }
}
