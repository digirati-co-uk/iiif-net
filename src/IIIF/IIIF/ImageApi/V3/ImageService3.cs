using System.Collections.Generic;
using IIIF.Presentation.V3;

namespace IIIF.ImageApi.V3;

public class ImageService3 : ResourceBase, IService
{
    public const string Image3Context = "http://iiif.io/api/image/3/context.json";
    public const string Level0Profile = "level0";
    public const string Level1Profile = "level1";
    public const string Level2Profile = "level2";
    public const string ImageProtocol = "http://iiif.io/api/image";

    public override string Type => nameof(ImageService3);

    [JsonProperty(Order = 10)] public string Protocol { get; set; }

    [JsonProperty(Order = 11)] public int Width { get; set; }

    [JsonProperty(Order = 12)] public int Height { get; set; }
    
    [JsonProperty(Order = 13)] public int? MaxWidth { get; set; }
    [JsonProperty(Order = 14)] public int? MaxHeight { get; set; }
    [JsonProperty(Order = 15)] public int? MaxArea { get; set; }

    [JsonProperty(Order = 16)] public List<Size> Sizes { get; set; }

    [JsonProperty(Order = 17)] public List<Tile> Tiles { get; set; }

    /// <summary>
    /// An array of strings that are the preferred format parameter values, arranged in order of preference. The
    /// format parameter values listed must be among those specified in the referenced profile or listed in the
    /// extraFormats property
    /// </summary>
    [JsonProperty(Order = 26)]
    public List<string> PreferredFormats { get; set; }

    /// <summary>
    /// An array of strings that can be used as the quality parameter, in addition to default.
    /// </summary>
    [JsonProperty(Order = 28)]
    public List<string> ExtraQualities { get; set; }

    /// <summary>
    /// An array of strings that can be used as the format parameter, in addition to the ones specified in the
    /// referenced profile.
    /// </summary>
    [JsonProperty(Order = 29)]
    public List<string> ExtraFormats { get; set; }

    /// <summary>
    /// An array of strings identifying features supported by the service, in addition to the ones specified in the
    /// referenced profile. 
    /// </summary>
    [JsonProperty(Order = 30)]
    public List<string> ExtraFeatures { get; set; }
}