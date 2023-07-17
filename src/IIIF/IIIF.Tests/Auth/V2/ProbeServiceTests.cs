using System.Collections.Generic;
using FluentAssertions;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Auth.V2;

public class ProbeServiceTests
{
    [Fact]
    public void ProbeService_Serialises()
    {
        // Arrange
        var probe = new AuthProbeService2
        {
            Id = "https://example.org/resource1/probe",
            Label = new LanguageMap("en", "A probe Service"),
            ErrorHeading = new LanguageMap("en", "errorHeading value"),
            ErrorNote = new LanguageMap("en", "errorNote value")
        };

        // Act
        var json = probe.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""id"": ""https://example.org/resource1/probe"",
  ""type"": ""AuthProbeService2"",
  ""label"": {""en"":[""A probe Service""]},
  ""errorHeading"": {""en"":[""errorHeading value""]},
  ""errorNote"": {""en"":[""errorNote value""]}
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ProbeService_CanDeserialiseSerialised()
    {
        // Arrange
        var probe = new AuthProbeService2
        {
            Id = "https://example.org/resource1/probe",
            Label = new LanguageMap("en", "A probe Service"),
            ErrorHeading = new LanguageMap("en", "errorHeading value"),
            ErrorNote = new LanguageMap("en", "errorNote value")
        };
        
        var serialised = probe.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeService2>();
        deserialised.Should().BeEquivalentTo(probe);
    }

    [Fact]
    public void ProbeServiceResult_Can_Substitute_ImageService2()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IService>
            {
                new ImageService2
                {
                    Id = "https://example.org/imageService2"
                }
            },
            Heading = new LanguageMap("en", "heading value"),
            Note = new LanguageMap("en", "note value")
        };

        // Act
        var json = probe.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""@context"": ""http://iiif.io/api/auth/2/context.json"",
  ""type"": ""AuthProbeResult2"",
  ""status"": 401,
  ""substitute"": [
    {
      ""@id"": ""https://example.org/imageService2"",
      ""@type"": ""ImageService2""
    }
  ],
  ""heading"": {""en"":[""heading value""]},
  ""note"": {""en"":[""note value""]}
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteImageService2()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IService>
            {
                new ImageService2
                {
                    Id = "https://example.org/imageService2"
                }
            },
            Heading = new LanguageMap("en", "heading value"),
            Note = new LanguageMap("en", "note value")
        };

        var serialised = probe.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeResult2>();
        deserialised.Should().BeEquivalentTo(probe);
    }

    [Fact]
    public void ProbeService_Can_Substitute_ImageService3()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IService>
            {
                new ImageService3
                {
                    Id = "https://example.org/imageService3"
                }
            },
            Heading = new LanguageMap("en", "heading value"),
            Note = new LanguageMap("en", "note value")
        };

        // Act
        var json = probe.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""@context"": ""http://iiif.io/api/auth/2/context.json"",
  ""type"": ""AuthProbeResult2"",
  ""status"": 401,
  ""substitute"": [
    {
      ""id"": ""https://example.org/imageService3"",
      ""type"": ""ImageService3""
    }
  ],
  ""heading"": {""en"":[""heading value""]},
  ""note"": {""en"":[""note value""]}
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteImageService3()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IService>
            {
                new ImageService3
                {
                    Id = "https://example.org/imageService3"
                }
            },
            Heading = new LanguageMap("en", "heading value"),
            Note = new LanguageMap("en", "note value")
        };

        var serialised = probe.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeResult2>();
        deserialised.Should().BeEquivalentTo(probe);
    }
    
    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteVideo()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IService>
            {
                new Video
                {
                    Id = "https://example.org/video/12345/file.m3u8"
                }
            },
            Heading = new LanguageMap("en", "heading value"),
            Note = new LanguageMap("en", "note value")
        };

        var serialised = probe.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeResult2>();
        deserialised.Should().BeEquivalentTo(probe);
    }

    [Fact]
    public void ProbeService_Can_Provide_Location()
    {
        // Arrange
        var probeResult2 = new AuthProbeResult2
        {
            Status = 200,
            Location = new Video
            {
                Id = "https://example.org/video/12345/file.m3u8"
            }
        };

        // Act
        var json = probeResult2.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""@context"": ""http://iiif.io/api/auth/2/context.json"",
  ""type"": ""AuthProbeResult2"",
  ""status"": 200,
  ""location"": {
    ""id"": ""https://example.org/video/12345/file.m3u8"",
    ""type"": ""Video""
  }
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ProbeService_Can_Deserialise_WithLocation()
    {
        // Arrange
        var probeResult2 = new AuthProbeResult2
        {
            Status = 200,
            Location = new Video
            {
                Id = "https://example.org/video/12345/file.m3u8"
            }
        };

        var serialised = probeResult2.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeResult2>();
        deserialised.Should().BeEquivalentTo(probeResult2);
    }

    [Fact]
    public void ProbeService_Can_Provide_AccessService()
    {
        // Arrange
        var probe = new AuthProbeService2
        {
            Id = "https://example.org/resource1/probe",
            Label = new LanguageMap("en", "A probe service"),
            Service = new List<IService>()
            {
                new AuthAccessService2
                {
                    Id = "https://example.com/auth/access",
                    Profile = AuthAccessService2.ActiveProfile,
                    Label = new LanguageMap("en", "label value"),
                    Heading = new LanguageMap("en", "heading value"),
                    Note = new LanguageMap("en", "note value"),
                    ConfirmLabel = new LanguageMap("en", "confirmLabel value")
                }
            }
        };

        // Act
        var json = probe.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""id"": ""https://example.org/resource1/probe"",
  ""type"": ""AuthProbeService2"",
  ""label"": {""en"":[""A probe service""]},
  ""service"": [
    {
      ""id"": ""https://example.com/auth/access"",
      ""type"": ""AuthAccessService2"",
      ""profile"": ""active"",
      ""label"": {""en"":[""label value""]},
      ""confirmlabel"": {""en"":[""confirmLabel value""]},
      ""heading"": {""en"":[""heading value""]},
      ""note"": {""en"":[""note value""]}
    }
  ]
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ProbeService_Can_Deserialise_AccessService()
    {
        // Arrange
        var probe = new AuthProbeService2
        {
            Id = "https://example.org/resource1/probe",
            Label = new LanguageMap("en", "A probe service"),
            Service = new List<IService>
            {
                new AuthAccessService2
                {
                    Id = "https://example.com/auth/access",
                    Profile = AuthAccessService2.ActiveProfile,
                    Label = new LanguageMap("en", "label value"),
                    Heading = new LanguageMap("en", "heading value"),
                    Note = new LanguageMap("en", "note value"),
                    ConfirmLabel = new LanguageMap("en", "confirmLabel value")
                }
            }
        };

        var serialised = probe.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthProbeService2>();
        deserialised.Should().BeEquivalentTo(probe);
    }
}