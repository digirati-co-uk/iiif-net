using System;
using System.Collections.Generic;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class ImageRequestXTests
{
    [Fact]
    public void GetResultingSize_CorrectMax()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Max = true
        };
        var originalSize = new Size(400, 400);
        var expected = new Size(400, 400);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectMaxScaled()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Max = true, Upscaled = true
        };
        var originalSize = new Size(400, 400);
        var expected = new Size(400, 400);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthOnly()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 100
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(100, 200);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthOnlyScaled()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 100, Upscaled = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(100, 200);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectHeightOnly()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Height = 40
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(20, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectHeightOnlyScaled()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Height = 40, Upscaled = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(20, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthHeight()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 90, Height = 40
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(90, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthHeightScaled()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 90, Height = 40, Upscaled = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(90, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthHeightConfined()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 90, Height = 40, Confined = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(20, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectWidthHeightScaledConfined()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            Width = 90, Height = 40, Confined = true, Upscaled = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(20, 40);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectPercentage()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            PercentScale = 30f
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(120, 240);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetResultingSize_CorrectPercentageScaled()
    {
        // Arrange
        var sizeParameter = new SizeParameter
        {
            PercentScale = 30f, Upscaled = true
        };
        var originalSize = new Size(400, 800);
        var expected = new Size(120, 240);

        // Act
        var result = sizeParameter.GetResultingSize(originalSize);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Theory]
    [InlineData("my-asset")]
    [InlineData("my-asset/")]
    public void Parse_IsBase(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var result = ImageRequest.Parse($"{prefix}{path}", prefix);
        
        // Assert
        result.IsBase.Should().BeTrue();
        result.ToString().Should().Be("iiif-img/27/1/my-asset");
    }
    
    [Fact]
    public void Parse_InfoJson()
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var result = ImageRequest.Parse($"{prefix}my-asset/info.json", prefix);
        
        // Assert
        result.IsInformationRequest.Should().BeTrue();
        result.ToString().Should().Be("iiif-img/27/1/my-asset/info.json");
    }
    
    [Fact]
    public void Parse_Fails_WhenInfoHasInvalidExtension()
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}my-asset/info.jsonll", prefix);
        
        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1")]
    [InlineData("iiif-img/27/1")]
    public void Parse_Validate_HandlesPrefixFormats(string prefix)
    {
        // Arrange and Act
        const string request = $"iiif-img/27/1/my-asset/full/800,/0/default.jpg";
        var action = () => ImageRequest.Parse(request, prefix, true);
        
        // Assert
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Theory]
    [InlineData("my-asset//full/800,/0/default.jpg")]
    [InlineData("my-asset/full//800,/0/default.jpg")]
    [InlineData("my-asset/full/800,//0/default.jpg")]
    [InlineData("my-asset/full/800,/0//default.jpg")]
    public void Parse_Validate_Fails_WhenGivenExtraSegments(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}{path}", prefix, true);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Path contains empty or an invalid number of segments");
    }
    
    [Theory]
    [InlineData("my-asset//800,/0/default.jpg")]
    [InlineData("my-asset/full//0/default.jpg")]
    [InlineData("my-asset/full/800,//default.jpg")]
    [InlineData("my-asset/full/800,/0/")]
    [InlineData("my-asset////default.jpg")]
    [InlineData("my-asset////")]
    public void Parse_Validate_Fails_WhenGivenEmptyParameters(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}{path}", prefix, true);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Path contains empty or an invalid number of segments");
    }
    
    public static IEnumerable<object[]> ImageRequests =>
        new List<object[]>
        {
            new object[] { "iiif-img/27/1/my-asset", new ImageRequest { IsBase = true } },
            new object[] { "iiif-img/27/1/my-asset/info.json", new ImageRequest { IsInformationRequest = true } },
            new object[]
            {
                "iiif-img/27/1/my-asset/full/max/0/default.jpg",
                new ImageRequest
                {
                    Region = new RegionParameter { Full = true },
                    Format = "jpg",
                    Size = new SizeParameter { Max = true },
                    Quality = "default",
                    Rotation = new RotationParameter { Angle = 0f },
                    OriginalPath = "my-asset/full/max/0/default.jpg",
                }
            },
            new object[]
            {
                "iiif-img/27/1/my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
                new ImageRequest
                {
                    Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
                    Format = "png",
                    Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
                    Quality = "bitonal",
                    Rotation = new RotationParameter { Angle = 180f, Mirror = true },
                    OriginalPath = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
                }
            },
        };

    [Theory]
    [MemberData(nameof(ImageRequests))]
    public void Parse_ValidRequest_CanRoundTripToString(string input, ImageRequest expected)
    {
        const string prefix = "iiif-img/27/1/";
        expected.Prefix = prefix;
        expected.Identifier = "my-asset";
        
        var result = ImageRequest.Parse(input, prefix);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
        result.ToString().Should().Be(input);
    }
    
    [Fact]
    public void Parse_ValidRequest_IncludingHostname()
    {
        const string input = "http://iiif.io/images/my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var expected = new ImageRequest
        {
            Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
            Format = "png",
            Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
            Quality = "bitonal",
            Rotation = new RotationParameter { Angle = 180f, Mirror = true },
            OriginalPath = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
            Prefix = "images/",
            Identifier = "my-asset",
            Scheme = "http",
            Server = "iiif.io",
        };
        
        var success = ImageRequest.TryParse(input, out var result);
        
        // Assert
        success.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        result.ToString().Should().Be(input);
    }
    
    [Fact]
    public void Parse_ValidRequest_IncludingHostname_NoPrefix()
    {
        const string input = "http://iiif.io/my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var expected = new ImageRequest
        {
            Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
            Format = "png",
            Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
            Quality = "bitonal",
            Rotation = new RotationParameter { Angle = 180f, Mirror = true },
            OriginalPath = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
            Identifier = "my-asset",
            Scheme = "http",
            Server = "iiif.io",
        };
        
        var success = ImageRequest.TryParse(input, out var result);
        
        // Assert
        success.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("default.jpg")]
    [InlineData("/no-int/default.jpg")]
    [InlineData("/circle/0/default.jpg")]
    [InlineData("/portrait/max/0/default.jpg")]
    [InlineData("http://hello.io/full/max/0/default.jpg")]
    public void TryParse_InvalidRequest_ReturnsFalse(string input)
    {
        var success = ImageRequest.TryParse(input, out var result);

        success.Should().BeFalse();
        result.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(ImageRequests))]
    public void TryParse_ValidRequest_CanRoundTripToString(string input, ImageRequest expected)
    {
        expected.Prefix = "iiif-img/27/1/";
        expected.Identifier = "my-asset";
        
        var success = ImageRequest.TryParse(input, out var result);
        
        // Assert
        success.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        result.ToString().Should().Be(input);
    }

    [Fact]
    public void ToString_ReflectsUpdates()
    {
        var expected = "iiif-img/27/1/my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var imageRequest = new ImageRequest
        {
            Prefix = "iiif-img/27/1/",
            Identifier = "my-asset", 
            Format = "jpg",
            Quality = "default",
            Size = new SizeParameter { PercentScale = 75 },
            Region = new RegionParameter { Square = true },
            Rotation = new RotationParameter { Angle = 0f },
        };

        imageRequest.Format = "png";
        imageRequest.Quality = "bitonal";
        imageRequest.Size = new SizeParameter { Confined = true, Width = 500, Height = 250 };
        imageRequest.Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 };
        imageRequest.Rotation = new RotationParameter { Angle = 180f, Mirror = true };
        
        imageRequest.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void ToString_Correct_NoPrefixReflectsUpdates()
    {
        var expected = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var imageRequest = new ImageRequest
        {
            Format = "png",
            Identifier = "my-asset",
            Quality = "bitonal",
            Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
            Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
            Rotation = new RotationParameter { Angle = 180f, Mirror = true },
        };
        
        imageRequest.ToString().Should().Be(expected);
    }

    // 5000 x 4000 service, 512px square tiles at scaleFactors [1,2,4,8,16]
    private static readonly List<Tile> StandardTiles = new()
    {
        new Tile { Width = 512, Height = 512, ScaleFactors = new[] { 1, 2, 4, 8, 16 } }
    };

    // 5000 x 4000 service, 256 wide x 512 high (non-square) tiles at scaleFactors [1,2,4]
    private static readonly List<Tile> NonSquareTiles = new()
    {
        new Tile { Width = 256, Height = 512, ScaleFactors = new[] { 1, 2, 4 } }
    };

    // 5000 x 4000 service offering two distinct tile sets at different sizes
    private static readonly List<Tile> MixedTiles = new()
    {
        new Tile { Width = 256, Height = 256, ScaleFactors = new[] { 1, 2 } },
        new Tile { Width = 1024, Height = 1024, ScaleFactors = new[] { 1, 2, 4 } }
    };

    [Theory]
    [InlineData("id/1024,512,512,512/512,512/0/default.jpg")]   // interior tile, scaleFactor 1
    [InlineData("id/1024,512,512,512/512,/0/default.jpg")]      // interior tile, width-only size
    [InlineData("id/4608,512,392,512/392,512/0/default.jpg")]   // right-edge column clamped, scaleFactor 1
    [InlineData("id/1024,1024,1024,1024/512,512/0/default.jpg")]// interior tile, scaleFactor 2
    [InlineData("id/0,0,5000,4000/313,250/0/default.jpg")]      // whole image as one tile, scaleFactor 16
    [InlineData("id/full/313,250/0/default.jpg")]               // whole image expressed as full region
    public void IsTileRequest_True_ForComputedTiles(string path)
    {
        var imageRequest = ImageRequest.Parse(path, string.Empty);

        imageRequest.IsTileRequest(5000, 4000, StandardTiles).Should().BeTrue();
    }

    [Theory]
    [InlineData("id/full/200,/0/default.jpg")]                  // thumbnail, not on the tile grid
    [InlineData("id/1000,512,512,512/512,512/0/default.jpg")]   // region origin not aligned to grid
    [InlineData("id/1024,512,512,512/256,256/0/default.jpg")]   // size implies scaleFactor 2 but grid misaligned
    [InlineData("id/pct:10,10,20,20/512,512/0/default.jpg")]    // percentage region
    [InlineData("id/square/512,512/0/default.jpg")]             // square region
    [InlineData("my-asset/info.json")]                          // information request
    public void IsTileRequest_False_ForNonTiles(string path)
    {
        var imageRequest = ImageRequest.Parse(path, string.Empty);

        imageRequest.IsTileRequest(5000, 4000, StandardTiles).Should().BeFalse();
    }

    [Theory]
    [InlineData("id/512,512,256,512/256,512/0/default.jpg")]    // interior tile, scaleFactor 1
    [InlineData("id/512,1024,512,1024/256,512/0/default.jpg")]  // interior tile, scaleFactor 2
    [InlineData("id/4864,512,136,512/136,512/0/default.jpg")]   // right-edge column clamped
    [InlineData("id/0,3584,256,416/256,416/0/default.jpg")]     // bottom-edge row clamped
    public void IsTileRequest_True_ForNonSquareTiles(string path)
    {
        var imageRequest = ImageRequest.Parse(path, string.Empty);

        imageRequest.IsTileRequest(5000, 4000, NonSquareTiles).Should().BeTrue();
    }

    [Fact]
    public void IsTileRequest_False_WhenRegionMatchesWrongTileShape()
    {
        // 256x256 square region would be a tile for square tiles, but the service's tiles are 256x512
        var imageRequest = ImageRequest.Parse("id/512,512,256,256/256,256/0/default.jpg", string.Empty);

        imageRequest.IsTileRequest(5000, 4000, NonSquareTiles).Should().BeFalse();
    }

    [Theory]
    [InlineData("id/256,256,256,256/256,256/0/default.jpg")]      // matches the 256px tile set, scaleFactor 1
    [InlineData("id/512,512,512,512/256,256/0/default.jpg")]      // matches the 256px tile set, scaleFactor 2
    [InlineData("id/1024,1024,1024,1024/1024,1024/0/default.jpg")]// matches the 1024px tile set, scaleFactor 1
    [InlineData("id/2048,0,2048,2048/1024,1024/0/default.jpg")]   // matches the 1024px tile set, scaleFactor 2
    public void IsTileRequest_True_WhenMatchingAnyTileSet(string path)
    {
        var imageRequest = ImageRequest.Parse(path, string.Empty);

        imageRequest.IsTileRequest(5000, 4000, MixedTiles).Should().BeTrue();
    }

    [Fact]
    public void IsTileRequest_False_WhenMatchingNoTileSet()
    {
        // 512px region scaled to 512 is scaleFactor 1, but neither tile set has a 512px tile
        var imageRequest = ImageRequest.Parse("id/512,512,512,512/512,512/0/default.jpg", string.Empty);

        imageRequest.IsTileRequest(5000, 4000, MixedTiles).Should().BeFalse();
    }

    [Fact]
    public void IsTileRequest_Null_WhenNoTiles()
    {
        var imageRequest = ImageRequest.Parse("id/1024,512,512,512/512,512/0/default.jpg", string.Empty);

        imageRequest.IsTileRequest(5000, 4000, null).Should().BeNull();
        imageRequest.IsTileRequest(5000, 4000, new List<Tile>()).Should().BeNull();
    }
}