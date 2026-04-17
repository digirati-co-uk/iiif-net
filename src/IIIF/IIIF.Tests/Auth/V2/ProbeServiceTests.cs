using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;


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
        var json = probe.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["id"]?.ToString().Should().Be("https://example.org/resource1/probe");
        jsonToken["type"]?.ToString().Should().Be("AuthProbeService2");
        jsonToken["label"]?["en"]?.Values<string>().Should().Contain("A probe Service");
        jsonToken["errorHeading"]?["en"]?.Values<string>().Should().Contain("errorHeading value");
        jsonToken["errorNote"]?["en"]?.Values<string>().Should().Contain("errorNote value");
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
        deserialised.Should().BeEquivalentTo(probe, options => options.PreferringRuntimeMemberTypes());
    }

    [Fact]
    public void ProbeServiceResult_Can_Substitute_ImageService2()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IResource>
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
        var json = probe.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        jsonToken["type"]?.ToString().Should().Be("AuthProbeResult2");
        ((int?)jsonToken["status"]).Should().Be(401);
        jsonToken["substitute"]?.Should().HaveCount(1);
        jsonToken["substitute"]?[0]?["@id"]?.ToString().Should().Be("https://example.org/imageService2");
        jsonToken["substitute"]?[0]?["@type"]?.ToString().Should().Be("ImageService2");
        jsonToken["heading"]?["en"]?.Values<string>().Should().Contain("heading value");
        jsonToken["note"]?["en"]?.Values<string>().Should().Contain("note value");
    }
    
    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteImageService2()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IResource>
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

        // Assert - explicit field validation
        deserialised.Status.Should().Be(401);
        deserialised.Heading.Should().BeEquivalentTo(probe.Heading);
        deserialised.Note.Should().BeEquivalentTo(probe.Note);
        deserialised.Substitute.Should().HaveCount(1);
        deserialised.Substitute![0].Should().BeOfType<ImageService2>().Which
            .Id.Should().Be("https://example.org/imageService2");

        // Verify context was serialized correctly
        var json = Newtonsoft.Json.Linq.JToken.Parse(serialised);
        json["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
    }

    [Fact]
    public void ProbeService_Can_Substitute_ImageService3()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IResource>
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
        var json = probe.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        jsonToken["type"]?.ToString().Should().Be("AuthProbeResult2");
        ((int?)jsonToken["status"]).Should().Be(401);
        jsonToken["substitute"]?.Should().HaveCount(1);
        jsonToken["substitute"]?[0]?["id"]?.ToString().Should().Be("https://example.org/imageService3");
        jsonToken["substitute"]?[0]?["type"]?.ToString().Should().Be("ImageService3");
        jsonToken["heading"]?["en"]?.Values<string>().Should().Contain("heading value");
        jsonToken["note"]?["en"]?.Values<string>().Should().Contain("note value");
    }

    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteImageService3()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IResource>
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

        // Assert - explicit field validation
        deserialised.Status.Should().Be(probe.Status);
        deserialised.Heading.Should().BeEquivalentTo(probe.Heading);
        deserialised.Note.Should().BeEquivalentTo(probe.Note);
        deserialised.Substitute.Should().HaveCount(1);
        deserialised.Substitute![0].Should().BeOfType<ImageService3>().Which
            .Id.Should().Be("https://example.org/imageService3");

        // Verify context was serialized correctly
        var json = Newtonsoft.Json.Linq.JToken.Parse(serialised);
        json["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
    }

    [Fact]
    public void ProbeServiceResult_Can_Deserialise_SubstituteVideo()
    {
        // Arrange
        var probe = new AuthProbeResult2
        {
            Status = 401,
            Substitute = new List<IResource>
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

        // Assert - explicit field validation
        deserialised.Status.Should().Be(401);
        deserialised.Heading.Should().BeEquivalentTo(probe.Heading);
        deserialised.Note.Should().BeEquivalentTo(probe.Note);
        deserialised.Substitute.Should().HaveCount(1);
        deserialised.Substitute![0].Should().BeOfType<Video>().Which
            .Id.Should().Be("https://example.org/video/12345/file.m3u8");

        // Verify context was serialized correctly
        var json = Newtonsoft.Json.Linq.JToken.Parse(serialised);
        json["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
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
        var json = probeResult2.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        jsonToken["type"]?.ToString().Should().Be("AuthProbeResult2");
        ((int?)jsonToken["status"]).Should().Be(200);
        jsonToken["location"]?["id"]?.ToString().Should().Be("https://example.org/video/12345/file.m3u8");
        jsonToken["location"]?["type"]?.ToString().Should().Be("Video");
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

        // Assert - explicit field validation
        deserialised.Status.Should().Be(200);
        deserialised.Location.Should().BeOfType<Video>().Which
            .Id.Should().Be("https://example.org/video/12345/file.m3u8");

        // Verify context was serialized correctly
        var json = Newtonsoft.Json.Linq.JToken.Parse(serialised);
        json["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
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
        var json = probe.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["id"]?.ToString().Should().Be("https://example.org/resource1/probe");
        jsonToken["type"]?.ToString().Should().Be("AuthProbeService2");
        jsonToken["label"]?["en"]?.Values<string>().Should().Contain("A probe service");
        jsonToken["service"]?.Should().HaveCount(1);
        jsonToken["service"]?[0]?["id"]?.ToString().Should().Be("https://example.com/auth/access");
        jsonToken["service"]?[0]?["type"]?.ToString().Should().Be("AuthAccessService2");
        jsonToken["service"]?[0]?["profile"]?.ToString().Should().Be("active");
        jsonToken["service"]?[0]?["label"]?["en"]?.Values<string>().Should().Contain("label value");
        jsonToken["service"]?[0]?["confirmlabel"]?["en"]?.Values<string>().Should().Contain("confirmLabel value");
        jsonToken["service"]?[0]?["heading"]?["en"]?.Values<string>().Should().Contain("heading value");
        jsonToken["service"]?[0]?["note"]?["en"]?.Values<string>().Should().Contain("note value");
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
        deserialised.Should().BeEquivalentTo(probe, options => options.PreferringRuntimeMemberTypes());
    }
}