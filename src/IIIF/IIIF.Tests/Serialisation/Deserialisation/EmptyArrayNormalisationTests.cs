using System.Collections.Generic;
using IIIF.Presentation.V3;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation.Deserialisation;

public class EmptyArrayNormalisationTests
{
    private static JsonSerializerSettings DeserializerSettings { get; } = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new PrettyIIIFContractResolver(),
        Formatting = Formatting.Indented,
        Converters = IIIFSerialiserX.DeserializerSettings.Converters
    };

    [Fact]
    public void Deserialize_IgnoresEmptyArrayForSingletonObjectProperty()
    {
        // Arrange
        const string manifest = """
        {
          "id": "https://example.org/manifest/1",
          "type": "Manifest",
          "label": {
            "en": [ "Test manifest" ]
          },
          "placeholderCanvas": []
        }
        """;

        // Act
        var result = JsonConvert.DeserializeObject<Manifest>(manifest, DeserializerSettings);

        // Assert
        result.Should().NotBeNull();
        result!.PlaceholderCanvas.Should().BeNull();
    }

    [Fact]
    public void Deserialize_PreservesEmptyArrayForListProperty()
    {
        // Arrange
        const string canvas = """
        {
          "id": "https://example.org/canvas/1",
          "type": "Canvas",
          "annotations": []
        }
        """;

        // Act
        var result = JsonConvert.DeserializeObject<Canvas>(canvas, DeserializerSettings);

        // Assert
        result.Should().NotBeNull();
        result!.Annotations.Should().NotBeNull();
        result.Annotations.Should().BeEmpty();
    }
}