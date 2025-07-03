using System;
using IIIF.Auth.V1;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Extensions.NavPlace;
using IIIF.Search.V1;
using IIIF.Serialisation;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation.Deserialisation;

public class ResourceConverterTests
{
    private readonly ResourceConverter sut = new();

    [Theory]
    [InlineData("SearchService1", typeof(SearchService))]
    [InlineData("AuthLogoutService1", typeof(AuthLogoutService))]
    [InlineData("AuthTokenService1", typeof(AuthTokenService))]
    [InlineData("AutoCompleteService1", typeof(AutoCompleteService))]
    public void ReadJson_IdentifiesType_FromAtType(string atType, Type expectedType)
    {
        var input = $"{{ \"@type\": \"{atType}\", \"@id\": \"{Guid.NewGuid()}\"}}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from @type");
    }
    
    [Theory]
    [InlineData("iiif:Image", typeof(ImageService2))]
    [InlineData("ImageService2", typeof(ImageService2))]
    public void ReadJson_IdentifiesImageService2_FromAtType(string atType, Type expectedType)
    {
        var input = $"{{ \"@type\": \"{atType}\", \"@id\": \"{Guid.NewGuid()}\"}}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from @type");
    }
    
    [Theory]
    [InlineData("iiif:Image")]
    [InlineData("ImageService2")]
    public void ReadJson_ImageService2_WorksWithExpandedProfile(string type)
    {
        var jsonId = @$"
        {{
           ""@context"": ""http://iiif.io/api/image/2/context.json"",
           ""@id"": ""https://iiif-net/image.jpg"",
           ""@type"": ""{type}"",
           ""profile"": [
               ""http://iiif.io/api/image/2/level2.json"",
               {{
                   ""formats"": [""tif""],
                   ""qualities"": [""bitonal""],
                   ""supports"": [""regionByPx""]
               }}
           ],
           ""protocol"": ""http://iiif.io/api/image""
       }}
       ";
        
        var result = JsonConvert.DeserializeObject<IResource>(jsonId, sut, new ImageService2Converter());
        
        var imageService2 = result as ImageService2;
        imageService2.Profile.Should().Be("http://iiif.io/api/image/2/level2.json");
        imageService2.ProfileDescription.Formats.Should().Contain("tif");
    }
    
    [Theory]
    [InlineData("ImageService3", typeof(ImageService3))]
    [InlineData("AuthAccessService2", typeof(AuthAccessService2))]
    [InlineData("AuthAccessTokenError2", typeof(AuthAccessTokenError2))]
    [InlineData("AuthAccessTokenService2", typeof(AuthAccessTokenService2))]
    [InlineData("AuthLogoutService2", typeof(AuthLogoutService2))]
    [InlineData("AuthProbeService2", typeof(AuthProbeService2))]
    [InlineData("Sound", typeof(Sound))]
    [InlineData("Video", typeof(Video))]
    [InlineData("Image", typeof(Image))]
    [InlineData("Feature", typeof(Feature))]
    [InlineData("AnnotationCollection", typeof(AnnotationCollection))]
    [InlineData("AnnotationPage", typeof(AnnotationPage))]
    [InlineData("Agent", typeof(Agent))]
    [InlineData("Annotation", typeof(Annotation))]
    [InlineData("Collection", typeof(Collection))]
    [InlineData("Manifest", typeof(Manifest))]
    [InlineData("SpecificResource", typeof(SpecificResource))]
    public void ReadJson_IdentifiesType_FromType(string type, Type expectedType)
    {
        var input = $"{{ \"type\": \"{type}\", \"id\": \"{Guid.NewGuid()}\"}}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from type");
    }
    
    [Theory]
    [InlineData("http://iiif.io/api/auth/0/logout", typeof(IIIF.Auth.V0.AuthLogoutService))]
    [InlineData("http://iiif.io/api/auth/0/token", typeof(IIIF.Auth.V0.AuthTokenService))]
    [InlineData("http://iiif.io/api/auth/0/login", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/clickthrough", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/kiosk", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/external", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/logout", typeof(AuthLogoutService))]
    [InlineData("http://iiif.io/api/auth/1/token", typeof(AuthTokenService))]
    [InlineData("http://iiif.io/api/auth/1/login", typeof(AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/clickthrough", typeof(AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/kiosk", typeof(AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/external", typeof(AuthCookieService))]
    [InlineData("http://iiif.io/api/search/2/autocomplete", typeof(IIIF.Search.V2.AutoCompleteService))]
    [InlineData("http://iiif.io/api/search/1/autocomplete", typeof(AutoCompleteService))]
    [InlineData("http://iiif.io/api/search/2/search", typeof(IIIF.Search.V2.SearchService))]
    public void ReadJson_IdentifiesType_FromProfile(string profile, Type expectedType)
    {
        var input = $"{{ \"profile\": \"{profile}\", \"id\": \"{Guid.NewGuid()}\"}}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(expectedType, "Concrete type identified from profile");
    }

    [Fact]
    public void ReadJson_V2ServiceReference_IfAtTypeAndIdOnly()
    {
        var input = $"{{ \"@type\": \"ThisCanBeAnything\", \"@id\": \"{Guid.NewGuid()}\"}}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType<V2ServiceReference>();
    }
    
    [Fact]
    public void ReadJson_V2ExternalService_IfAtType_AndUnableToDetermine()
    {
        var jsonId = "{\"@type\": \"Text\", \"@id\": \"https://service-reference-test\", \"label\": \"test\" }";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType<IIIF.Presentation.V2.ExternalService>();
    }
    
    [Fact]
    public void ReadJson_ExternalService()
    {
        var input =
            "{\"id\": \"https://example.org/service/example\",\"type\": \"ExampleExtensionService\",\"profile\": \"https://example.org/docs/example-service.html\"}";
        var expected = new IIIF.Presentation.V3.ExternalService("ExampleExtensionService")
        {
            Id = "https://example.org/service/example",
            Profile = "https://example.org/docs/example-service.html",
        };

        var deserialised = JsonConvert.DeserializeObject<IResource>(input, sut);

        deserialised.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadJson_IdentifiesTextualBody_WhenTextualBodyNoValue()
    {
        var input = $"{{ \"type\": \"TextualBody\", \"id\": \"{Guid.NewGuid()}\" }}";
        
        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(typeof(TextualBody));
    }
    
    [Fact]
    public void ReadJson_IdentifiesTextualBody_WhenTextualBodyWithValue()
    {
        var input = $"{{ \"type\": \"TextualBody\", \"id\": \"{Guid.NewGuid()}\", \"value\" : \"stuff\" }}";

        JsonConvert.DeserializeObject<IResource>(input, sut)
            .Should().BeOfType(typeof(TextualBody));
    }
}