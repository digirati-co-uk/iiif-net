using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace IIIF.Tests.Serialisation.Deserialisation;

public class ResourceBaseV3ConverterTests
{
    private readonly ResourceBaseV3Converter sut = new();
    
    [Theory]
    [InlineData("AnnotationCollection", typeof(AnnotationCollection))]
    [InlineData("AnnotationPage", typeof(AnnotationPage))]
    [InlineData("Agent", typeof(Agent))]
    [InlineData("Annotation", typeof(Annotation))]
    [InlineData("Collection", typeof(Collection))]
    [InlineData("Manifest", typeof(Manifest))]
    [InlineData("SpecificResource", typeof(SpecificResource))]
    [InlineData("TextualBody", typeof(TextualBody))]
    public void ReadJson_IdentifiesType_FromType(string type, Type expectedType)
    {
        var input = $"{{ \"type\": \"{type}\", \"id\": \"{Guid.NewGuid()}\", \"value\" : \"stuff\" }}";

        JsonConvert.DeserializeObject<ResourceBase>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from type");
    }
    
    [Theory]
    [InlineData("supplementing", typeof(SupplementingDocumentAnnotation))]
    [InlineData("painting", typeof(PaintingAnnotation))]
    [InlineData("classifying", typeof(TypeClassifyingAnnotation))]
    [InlineData("general", typeof(GeneralAnnotation))]
    public void ReadJson_IdentifiesType_FromMotivation(string type, Type expectedType)
    {
        var input = $"{{ \"motivation\": \"{type}\", \"id\": \"{Guid.NewGuid()}\" }}";

        JsonConvert.DeserializeObject<ResourceBase>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from motivation");
    }
    
    [Fact]
    public void ReadJson_IdentifiesType_FromId()
    {
        var input = $"{{ \"id\": \"{Guid.NewGuid()}\" }}";

        JsonConvert.DeserializeObject<ResourceBase>(input, sut)
            .Should().BeOfType(typeof(ClassifyingBody), "Concrete type identified from id");
    }
    
    [Fact]
    public void ReadJson_IdentifiesType_FromCustomType()
    {
        // Arrange
        var context = new Dictionary<string, Func<JObject, IResource>>()
        {
            ["testCustom"] = input => new TestCustomClass()
        };
        
        var input = $"{{ \"type\": \"testCustom\" }}";

        JsonConvert.DeserializeObject<ResourceBase>(input, new JsonSerializerSettings()
            {
                Converters = { new ResourceBaseV3Converter() },
                Context = new StreamingContext(StreamingContextStates.All, context)
            })
            .Should().BeOfType(typeof(TestCustomClass), "Concrete type identified from custom context");
    }

    private class TestCustomClass : ResourceBase
    {
        public override string Type { get; }
    }
}