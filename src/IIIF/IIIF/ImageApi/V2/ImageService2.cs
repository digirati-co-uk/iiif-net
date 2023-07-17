using System.Collections.Generic;
using IIIF.Presentation.V2;
using IIIF.Serialisation;

namespace IIIF.ImageApi.V2;

public class ImageService2 : ResourceBase, IService
{
    public const string Image2Context = "http://iiif.io/api/image/2/context.json";
    public const string Level0Profile = "http://iiif.io/api/image/2/level0.json";
    public const string Level1Profile = "http://iiif.io/api/image/2/level1.json";
    public const string Level2Profile = "http://iiif.io/api/image/2/level2.json";
    public const string Image2Protocol = "http://iiif.io/api/image";

    [JsonProperty(PropertyName = "@type", Order = 3)]
    public override string? Type { get; set; } = nameof(ImageService2);

    [JsonProperty(PropertyName = "protocol", Order = 10)]
    public string? Protocol { get; set; }

    [JsonIgnore] public ProfileDescription ProfileDescription { get; set; }

    [JsonProperty(Order = 11)] public int Width { get; set; }

    [JsonProperty(Order = 12)] public int Height { get; set; }

    [JsonProperty(Order = 13)] public List<Size> Sizes { get; set; }

    [JsonProperty(Order = 14)] public List<Tile> Tiles { get; set; }

    // TODO - Attribution, logo; not needed right now
    [JsonProperty(Order = 20)] public string[] License { get; set; }

    [JsonProperty(Order = 28)]
    [ObjectIfSingle]
    public List<IService>? Service { get; set; }
}