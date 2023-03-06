using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Discovery.V1
{
    /// <summary>
    /// A page of Activities
    /// </summary>
    /// <remarks>See https://iiif.io/api/discovery/1.0/#ordered-collection-page</remarks>
    public class OrderedCollectionPage : JsonLdBase, IService
    {
        [JsonProperty(Order = 2)]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 3)]
        public string Type => nameof(OrderedCollectionPage);
        
        /// <summary>
        /// The position of the first item in this page’s orderedItems list, relative to the overall ordering across all
        /// pages within the Collection.
        /// </summary>
        [JsonProperty(Order = 10)]
        public int? StartIndex { get; set; }
        
        /// <summary>
        /// The Ordered Collection of which this Page is a part.
        /// </summary>
        [JsonProperty(Order = 11)]
        public OrderedCollection? PartOf { get; set; }
        
        /// <summary>
        /// A reference to the previous page in the list of pages.
        /// </summary>
        [JsonProperty(Order = 20)]
        public OrderedCollectionPage Prev { get; set; }
        
        /// <summary>
        /// A reference to the next page in the list of pages.
        /// </summary>
        [JsonProperty(Order = 21)]
        public OrderedCollectionPage? Next { get; set; }

        /// <summary>
        /// The Activities that are listed as part of this page.
        /// </summary>
        [JsonProperty(Order = 22)]
        public List<Activity> OrderedItems { get; set; }
    }
}