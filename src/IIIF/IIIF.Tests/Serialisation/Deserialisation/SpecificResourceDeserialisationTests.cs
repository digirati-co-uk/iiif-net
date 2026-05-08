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
        const string specificResource = """
        {
            "type": "SpecificResource",
            "source": "https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif",
            "selector": 
                {
                    "type": "SvgSelector",
                    "value": "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'><g><path d='M1002,1393 1002,1502 1564,1502 1564,1393 Z' /></g></svg>"
                }
        }
        """;

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
        const string specificResource = """
        {
            "type": "SpecificResource",
            "source": "https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif",
            "selector": [
                {
                    "type": "PointSelector",
                    "x": 100,
                    "y": 100
                },
                {
                    "type": "AudioContentSelector"
                }
            ]
        }
        """;

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
        const string specificResource = """
        {
            "type": "SpecificResource",
            "source": "https://iiif.library.leeds.ac.uk/canvases/nb5fj4k4_objects_372705_001.tif",
            "selector": [
                {
                    "type": "ImageApiSelector",
                    "region": "something"
                }
            ]
        }
        """;

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        result.Selector.First().As<ImageApiSelector>().Region.Should().Be("something");
    }

    [Fact]
    public void Deserialize_DeserializesFragmentSelector()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": {
        ""type"": ""FragmentSelector"",
        ""conformsTo"": ""http://www.w3.org/TR/media-frags/"",
        ""value"": ""xywh=0,0,100,200""
    }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        var fragmentSelector = result.Selector.First().As<FragmentSelector>();
        fragmentSelector.Value.Should().Be("xywh=0,0,100,200");
        fragmentSelector.ConformsTo.Should().Be("http://www.w3.org/TR/media-frags/");
    }

    [Fact]
    public void Deserialize_DeserializesFragmentSelector_WithoutConformsTo()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": {
        ""type"": ""FragmentSelector"",
        ""value"": ""t=30,60""
    }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        var fragmentSelector = result.Selector.First().As<FragmentSelector>();
        fragmentSelector.Value.Should().Be("t=30,60");
        fragmentSelector.ConformsTo.Should().BeNull();
    }

    [Fact]
    public void Deserialize_DeserializesFragmentSelector_InMultipleSelectorArray()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": [
        {
            ""type"": ""FragmentSelector"",
            ""conformsTo"": ""http://www.w3.org/TR/media-frags/"",
            ""value"": ""xywh=0,0,100,200""
        },
        {
            ""type"": ""SvgSelector"",
            ""value"": ""<svg xmlns='http://www.w3.org/2000/svg'><rect x='0' y='0' width='100' height='200'/></svg>""
        }
    ]
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(2);
        result.Selector.First().As<FragmentSelector>().Value.Should().Be("xywh=0,0,100,200");
        result.Selector.Last().As<SvgSelector>().Value.Should().Contain("<rect");
    }

    [Fact]
    public void Deserialize_DeserializesGeneralSelector_WithType()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": {
        ""type"": ""UnknownSelector"",
        ""foo"": ""bar"",
        ""baz"": 42
    }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        var generalSelector = result.Selector.First().As<GeneralSelector>();
        generalSelector.Type.Should().Be("UnknownSelector");
        generalSelector.AdditionalProperties.Should().ContainKey("foo")
            .WhoseValue.ToString().Should().Be("bar");
        generalSelector.AdditionalProperties.Should().ContainKey("baz")
            .WhoseValue.ToObject<int>().Should().Be(42);
    }

    [Fact]
    public void Deserialize_DeserializesGeneralSelector_WithoutType()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": {
        ""foo"": ""bar"",
        ""baz"": 42
    }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        var generalSelector = result.Selector.First().As<GeneralSelector>();
        generalSelector.Type.Should().BeNull();
        generalSelector.AdditionalProperties.Should().ContainKey("foo")
            .WhoseValue.ToString().Should().Be("bar");
        generalSelector.AdditionalProperties.Should().ContainKey("baz")
            .WhoseValue.ToObject<int>().Should().Be(42);
    }

    [Fact]
    public void Deserialize_DeserializesGeneralSelector_WithNestedObject()
    {
        // Arrange
        var specificResource = @"
{
    ""type"": ""SpecificResource"",
    ""source"": ""https://example.org/canvas/1"",
    ""selector"": {
        ""type"": ""UnknownSelector"",
        ""foo"": { ""bar"": ""baz"" }
    }
}
";

        // Act
        var result = JsonConvert.DeserializeObject<SpecificResource>(specificResource, DeserializerSettings);

        // Assert
        result.Selector.Should().HaveCount(1);
        var generalSelector = result.Selector.First().As<GeneralSelector>();
        generalSelector.AdditionalProperties.Should().ContainKey("foo")
            .WhoseValue.Value<string>("bar").Should().Be("baz");
    }
}