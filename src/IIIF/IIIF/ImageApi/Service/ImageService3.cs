using IIIF.Presentation.V3;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IIIF.ImageApi.Service
{
    public class ImageService3 : ResourceBase, IService
    {
        public const string Image3Context = "http://iiif.io/api/image/3/context.json";
        public const string ImageProtocol = "http://iiif.io/api/image";

        public ImageService3()
        {
            Context = Image3Context;
        }

        public override string Type => nameof(ImageService3);

        [JsonProperty(Order = 3)]
        public string Protocol => ImageProtocol;

        [JsonProperty(Order = 11)]
        public int Width { get; set; }

        [JsonProperty(Order = 12)]
        public int Height { get; set; }

        [JsonProperty(Order = 13)]
        public List<Size> Sizes { get; set; }

        [JsonProperty(Order = 14)]
        public List<Tile> Tiles { get; set; }

        [JsonProperty(Order = 30)]
        public List<string> ExtraFeatures { get; set; }
    }
}
