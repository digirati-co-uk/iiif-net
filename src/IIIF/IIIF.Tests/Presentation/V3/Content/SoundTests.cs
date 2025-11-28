using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;

namespace IIIF.Tests.Presentation.V3.Content;

public class SoundTests
{
    [Fact]
    public void Sound_SerialiseOrder()
    {
        var sound = new Sound { Duration = 500.3, Id = "https://example.org/sound" };
        var expected = @"{
  ""id"": ""https://example.org/sound"",
  ""type"": ""Sound"",
  ""duration"": 500.3
}";
        var actual = sound.AsJson();
        
        actual.ShouldMatchJson(expected);
    }
}