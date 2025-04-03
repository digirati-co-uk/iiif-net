using System;
using System.Collections.Generic;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Serialisation;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation;

public class ServiceConverterTests
{
    private readonly ServiceConverter sut = new();

    [Theory]
    [InlineData("SearchService1", typeof(IIIF.Search.V1.SearchService))]
    [InlineData("AutoCompleteService1", typeof(IIIF.Search.V1.AutoCompleteService))]
    public void ReadJson_KnownSearch1Services_FromType(string type, Type expected)
    {
        var jsonId = $"{{\"@type\": \"{type}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }
    
    [Theory]
    [InlineData("AuthLogoutService1", typeof(IIIF.Auth.V1.AuthLogoutService))]
    [InlineData("AuthTokenService1", typeof(IIIF.Auth.V1.AuthTokenService))]
    public void ReadJson_KnownAuth1Services_FromType(string type, Type expected)
    {
        var jsonId = $"{{\"@type\": \"{type}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }

    [Theory]
    [InlineData("iiif:Image", "'iiif:Image' is type for presentation 2")]
    [InlineData("ImageService2", "'ImageService2' is type when rendered on presentation 3")]
    public void ReadJson_ImageService2_FromType(string type, string because)
    {
        var jsonId = $"{{\"@type\": \"{type}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);

        result.Should().BeOfType<ImageService2>(because);
    }
    
    [Fact]
    public void ReadJson_ImageService2_WorksWIthExpandedProfile()
    {
        var jsonId = @"
        {
           ""@context"": ""http://iiif.io/api/image/2/context.json"",
           ""@id"": ""https://iiif-net/image.jpg"",
           ""@type"": ""iiif:Image"",
           ""profile"": [
               ""http://iiif.io/api/image/2/level2.json"",
               {
                   ""formats"": [""tif""],
                   ""qualities"": [""bitonal""],
                   ""supports"": [""regionByPx""]
               }
           ],
           ""protocol"": ""http://iiif.io/api/image""
       }
       ";
        
        var result = JsonConvert.DeserializeObject<ImageService2>(jsonId, sut, new ImageService2Converter());
        result.Profile.Should().Be("http://iiif.io/api/image/2/level2.json");
        result.ProfileDescription.Formats.Should().Contain("tif");
    }
    
    [Theory]
    [InlineData("AuthAccessService2", typeof(IIIF.Auth.V2.AuthAccessService2))]
    [InlineData("AuthAccessTokenService2", typeof(IIIF.Auth.V2.AuthAccessTokenService2))]
    [InlineData("AuthLogoutService2", typeof(IIIF.Auth.V2.AuthLogoutService2))]
    [InlineData("AuthProbeService2", typeof(IIIF.Auth.V2.AuthProbeService2))]
    public void ReadJson_KnownAuth2Services_FromType(string type, Type expected)
    {
        var jsonId = $"{{\"type\": \"{type}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }
    
    [Fact]
    public void ReadJson_ImageService3_FromType()
    {
        var jsonId = "{\"type\": \"ImageService3\"}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);

        result.Should().BeOfType<ImageService3>();
    }
    
    [Theory]
    [InlineData(IIIF.Auth.V1.AuthLogoutService.AuthLogout1Profile, typeof(IIIF.Auth.V1.AuthLogoutService))]
    [InlineData(IIIF.Auth.V1.AuthTokenService.AuthToken1Profile, typeof(IIIF.Auth.V1.AuthTokenService))]
    [InlineData(IIIF.Auth.V0.AuthLogoutService.AuthLogout0Profile, typeof(IIIF.Auth.V0.AuthLogoutService))]
    [InlineData(IIIF.Auth.V0.AuthTokenService.AuthToken0Profile, typeof(IIIF.Auth.V0.AuthTokenService))]
    [InlineData("http://iiif.io/api/auth/0/login", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/clickthrough", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/kiosk", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/0/external", typeof(IIIF.Auth.V0.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/login", typeof(IIIF.Auth.V1.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/clickthrough", typeof(IIIF.Auth.V1.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/kiosk", typeof(IIIF.Auth.V1.AuthCookieService))]
    [InlineData("http://iiif.io/api/auth/1/external", typeof(IIIF.Auth.V1.AuthCookieService))]
    public void ReadJson_KnownAuthServices_FromProfile(string profile, Type expected)
    {
        var jsonId = $"{{\"profile\": \"{profile}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }
    
    [Theory]
    [InlineData(IIIF.Search.V2.AutoCompleteService.AutoComplete2Profile, typeof(IIIF.Search.V2.AutoCompleteService))]
    [InlineData(IIIF.Search.V1.AutoCompleteService.AutoCompleteService1Profile, typeof(IIIF.Search.V1.AutoCompleteService))]
    [InlineData(IIIF.Search.V2.SearchService.Search2Profile, typeof(IIIF.Search.V2.SearchService))]
    public void ReadJson_KnownSearchServices_FromProfile(string profile, Type expected)
    {
        var jsonId = $"{{\"profile\": \"{profile}\"}}";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType(expected);
    }

    [Fact]
    public void ReadJson_V2ServiceReference_IfTypeAndIdOnly()
    {
        var jsonId = "{\"@type\": \"AuthCookieService1\", \"@id\": \"https://service-reference-test\" }";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType<V2ServiceReference>();
    }
    
    [Fact]
    public void ReadJson_FallsBackTo_V2ExternalService_IfAtType_AndUnableToDetermine()
    {
        var jsonId = "{\"@type\": \"Text\", \"@id\": \"https://service-reference-test\", \"label\": \"test\" }";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType<IIIF.Presentation.V2.ExternalService>();
    }
    
    [Fact]
    public void ReadJson_FallsBackTo_V3ExternalService_IfType_AndUnableToDetermine()
    {
        var jsonId = "{\"type\": \"Text\", \"id\": \"https://service-reference-test\", \"label\": { \"none\": [\"test\"]} }";
        
        var result = JsonConvert.DeserializeObject<IService>(jsonId, sut);
        
        result.Should().BeOfType<IIIF.Presentation.V3.ExternalService>();
    }
}