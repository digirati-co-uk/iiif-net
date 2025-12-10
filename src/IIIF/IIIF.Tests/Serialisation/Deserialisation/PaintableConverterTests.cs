using System;
using System.Runtime.Serialization;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation.Deserialisation;

public class PaintableConverterTests
{
    private readonly PaintableConverter sut = new();
    
    [Fact]
    public void ReadJson_ThrowsMeaningful_IfTypeNotKnown()
    {
        var input = "{\"type\":\"foo\"}";

        Action action = () => JsonConvert.DeserializeObject<IPaintable>(input, sut);
        action.Should().ThrowExactly<SerializationException>()
            .WithMessage($"Unable to identify IPaintable, 'type'=foo: 'id' undefined");
    }
    
    [Fact]
    public void ReadJson_ThrowsMeaningful_IfTypeNotKnown_WithId()
    {
        var input = "{\"type\":\"foo\",\"id\":\"https://bar\"}";

        Action action = () => JsonConvert.DeserializeObject<IPaintable>(input, sut);
        action.Should().ThrowExactly<SerializationException>()
            .WithMessage($"Unable to identify IPaintable, 'type'=foo: 'id'=https://bar");
    }
    
    [Theory]
    [InlineData("Sound", typeof(Sound))]
    [InlineData("Audio", typeof(Sound))]
    [InlineData("Video", typeof(Video))]
    [InlineData("Image", typeof(Image))]
    [InlineData("Canvas", typeof(Canvas))]
    [InlineData("Choice", typeof(PaintingChoice))]
    [InlineData("SpecificResource", typeof(SpecificResource))]
    public void ReadJson_IdentifiesType_FromType(string type, Type expected)
    {
        var jsonId = $"{{\"type\":\"{type}\"}}";
        
        var result = JsonConvert.DeserializeObject<IPaintable>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }
    
    [Fact]
    public void ReadJson_IdentifiesType_TextualBodyValue()
    {
        var jsonId = "{\"type\":\"TextualBody\",\"value\":\"TextualBodyValue\"}";
        
        var expected = new TextualBody("TextualBodyValue");
        
        var result = JsonConvert.DeserializeObject<IPaintable>(jsonId, sut);

        result.Should().BeEquivalentTo(expected);
    }
}