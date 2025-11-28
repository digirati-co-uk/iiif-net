namespace IIIF.Tests;

public static class StringAssertionX
{
    /// <summary>
    /// Uses .Should().Be() fluent assertion but handles possible line ending differences
    /// </summary>
    public static void ShouldMatchJson(this string json, string expected, string because = "",
        params object[] becauseArgs)
    {
        json.Replace("\r\n", "\n").Should().Be(expected.Replace("\r\n", "\n"), because, becauseArgs);
    }
}