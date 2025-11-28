using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;

namespace IIIF.Tests.Presentation.V3.Content;

public class VideoTests
{
    [Fact]
    public void Video_SerialiseOrder()
    {
        var sound = new Video { Duration = 500.3, Id = "https://example.org/video", Width = 200, Height = 300 };
        var expected = @"{
  ""id"": ""https://example.org/video"",
  ""type"": ""Video"",
  ""width"": 200,
  ""height"": 300,
  ""duration"": 500.3
}";
        var actual = sound.AsJson();
        
        actual.ShouldMatchJson(expected);
    }
}