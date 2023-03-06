using System.Collections.Generic;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;

namespace IIIF.Discovery.V1
{
    /// <summary>
    /// The top-most resource for managing the lists of Activities
    /// </summary>
    /// <remarks>See https://iiif.io/api/discovery/1.0/#orderedcollection</remarks>
    public class OrderedCollection : JsonLdBase, IService
    {
        [JsonProperty(Order = 2)]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 3)]
        public string Type => nameof(OrderedCollection);

        /// <summary>
        /// The total number of Activities in the entire Ordered Collection.
        /// </summary>
        [JsonProperty(Order = 5)]
        public int? TotalItems { get; set; }
        
        /// <summary>
        /// A string that identifies a license or rights statement that applies to the usage of the Ordered Collection.
        /// </summary>
        [JsonProperty(Order = 6)]
        public string? Rights { get; set; }
     
        /// <summary>
        /// Refers to one or more documents that semantically describe the set of resources that are being acted upon in
        /// the Activities within the Ordered Collection, rather than any particular resource referenced from within the
        /// collection
        /// </summary>
        [JsonProperty(Order = 10)]
        public List<ExternalResource>? SeeAlso { get; set; }
        
        /// <summary>
        /// This property is used to refer to a larger Ordered Collection, of which this Ordered Collection is part. 
        /// </summary>
        [JsonProperty(Order = 11)]
        public List<OrderedCollection>? PartOf { get; set; }
        
        /// <summary>
        /// A link to the first Ordered Collection Page for this Collection.
        /// </summary>
        [JsonProperty(Order = 20)]
        public OrderedCollectionPage? First { get; set; }
        
        /// <summary>
        /// A link to the last Ordered Collection Page for this Collection.
        /// </summary>
        [JsonProperty(Order = 21)]
        public OrderedCollectionPage Last { get; set; }
    }
}