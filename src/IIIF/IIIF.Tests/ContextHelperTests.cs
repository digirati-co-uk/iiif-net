using System;
using System.Collections.Generic;

namespace IIIF.Tests;

public class ContextHelperTests
{
    [Fact]
    public void EnsureContext_AddsContext_IfNoExisting()
    {
        // Arrange
        var jsonLdBase = new TestJsonLdBase();
        const string customContext = "http://my-custom-context";
        
        // Act
        jsonLdBase.EnsureContext(customContext);
        
        // Assert
        jsonLdBase.Context.Should().BeOfType<string>()
            .And.Subject.Should().Be(customContext);
    }
    
    [Fact]
    public void EnsureContext_AddsContext_IfOneExisting_WithoutReordering()
    {
        // Arrange
        const string context = "http://existing-context";
        const string customContext = "http://my-custom-context";
        var jsonLdBase = new TestJsonLdBase { Context = context };

        var expected = new List<string> { context, customContext };
        
        // Act
        jsonLdBase.EnsureContext(customContext);
        
        // Assert
        (jsonLdBase.Context as List<string>).Should().ContainInOrder(expected);
    }

    [Theory]
    [InlineData(IIIF.Presentation.Context.Presentation2Context)]
    [InlineData(IIIF.Presentation.Context.Presentation3Context)]
    [InlineData(IIIF.ImageApi.V2.ImageService2.Image2Context)]
    [InlineData(IIIF.ImageApi.V3.ImageService3.Image3Context)]
    public void EnsureContext_AlwaysReordersIIIFContextsToLast_IfOthersAddedAfter(string iiifContext)
    {
        // Arrange
        const string customContext = "http://my-custom-context";
        var jsonLdBase = new TestJsonLdBase { Context = iiifContext };

        var expected = new List<string> { customContext, iiifContext };
        
        // Act
        jsonLdBase.EnsureContext(customContext);
        
        // Assert
        (jsonLdBase.Context as List<string>).Should().ContainInOrder(expected);
    }
    
    [Theory]
    [InlineData(IIIF.Presentation.Context.Presentation2Context)]
    [InlineData(IIIF.Presentation.Context.Presentation3Context)]
    [InlineData(IIIF.ImageApi.V2.ImageService2.Image2Context)]
    [InlineData(IIIF.ImageApi.V3.ImageService3.Image3Context)]
    public void EnsureContext_AlwaysReordersIIIFContextsToLast_IfMultipleAdds(string iiifContext)
    {
        // Arrange
        const string context = "http://existing-context";
        const string customContext = "http://my-custom-context";
        var jsonLdBase = new TestJsonLdBase { Context = context };

        var expected = new List<string> { context, customContext, iiifContext };
        
        // Act
        jsonLdBase.EnsureContext(iiifContext);
        jsonLdBase.EnsureContext(customContext);
        
        // Assert
        (jsonLdBase.Context as List<string>).Should().ContainInOrder(expected);
    }

    [Theory]
    [MemberData(nameof(SampleContexts))]
    public void EnsureContext_Throws_IfMultipleIIIFContextsAdded(string first, string second)
    {
        // Arrange
        var jsonLdBase = new TestJsonLdBase { Context = first };

        // Act
        Action action = () => jsonLdBase.EnsureContext(second);
        
        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    public static IEnumerable<object[]> SampleContexts =>
        new List<object[]>
        {
            new object[] { IIIF.Presentation.Context.Presentation2Context, IIIF.Presentation.Context.Presentation3Context },
            new object[] { IIIF.ImageApi.V2.ImageService2.Image2Context, IIIF.ImageApi.V3.ImageService3.Image3Context },
            new object[] { IIIF.Presentation.Context.Presentation2Context, IIIF.ImageApi.V3.ImageService3.Image3Context },
            new object[] { IIIF.ImageApi.V3.ImageService3.Image3Context , IIIF.Presentation.Context.Presentation3Context },
        };

    private class TestJsonLdBase : JsonLdBase
    {
    }
}