using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Selectors;
using IIIF.Serialisation;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation.Deserialisation;

public class SpecificResourceDeserialisationTests
{
    private static JsonSerializerSettings DeserializerSettings { get; set; } = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new PrettyIIIFContractResolver(),
        Formatting = Formatting.Indented,
        Converters = new List<JsonConverter>
        {
            new SelectorConverter()
        }
    };
    
    [Fact]
    public void Deserialize_DeserializesSingleSelector()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif"",
    ""selector"": 
        {
            ""type"": ""SvgSelector"",
            ""value"": ""<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'><g><path d='M1002,1393 1002,1502 1564,1502 1564,1393 Z' /></g></svg>""
        }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        result.Selector.First().As<SvgSelector>().Value.Should().Contain("M1002,1393 1002,1502");
    }
    
    [Fact]
    public void Deserialize_DeserializesMultipleSelector()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif"",
    ""selector"": [
        {
            ""type"": ""PointSelector"",
            ""x"": 100,
            ""y"": 100
        },
        {
            ""type"": ""AudioContentSelector"",
        }
    ]
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(2);
        result.Selector.First().As<PointSelector>().X.Should().Be(100);
        result.Selector.Last().As<AudioContentSelector>().Should().NotBeNull();
    }
    
    [Fact]
    public void Deserialize_DeserializesMultipleSelectorWithSingleSelector()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif"",
    ""selector"": [
        {
            ""type"": ""ImageApiSelector"",
            ""region"": ""something""
        }
    ]
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        result.Selector.First().As<ImageApiSelector>().Region.Should().Be("something");
    }
}