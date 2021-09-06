using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3
{
    public class Manifest : StructureBase, ICollectionItem
    {
        public override string Type => nameof(Manifest);
        
        [JsonProperty(Order = 300)]
        public List<Canvas>? Items { get; set; }
        
        [JsonProperty(Order = 400)]
        public List<Range>? Structures { get; set; }
        
        /// <summary>
        /// The direction in which a set of Canvases should be displayed to the user
        /// See <a href="https://iiif.io/api/presentation/3.0/#viewingdirection">viewingdirection</a>
        /// </summary>
        /// 
        [JsonProperty(Order = 32)]
        public string? ViewingDirection { get; set; }
        
        [JsonProperty(Order = 35)]
        // TODO - Interface may cause issues for deserialization
        public IStructuralLocation? Start { get; set; }
        
        /// <summary>
        /// Note that this is not the same as ResourceBase::Service
        /// </summary>
        [JsonProperty(Order = 39)]
        public List<IService>? Services { get; set; }
    }
}
