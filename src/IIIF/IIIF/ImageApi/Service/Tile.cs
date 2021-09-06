using Newtonsoft.Json;

namespace IIIF.ImageApi.Service
{
    public class Tile
    {
        [JsonProperty(Order = 1)]
        public int Width { get; set; }
        
        [JsonProperty(Order = 2)]
        public int Height { get; set; }
        
        [JsonProperty(Order = 3)]
        public int[] ScaleFactors { get; set; }
    }
}