using System.Collections.Generic;
using IIIF.Presentation.V3.Annotation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3.Content
{
    public class ExternalResource : ResourceBase
    {
        public ExternalResource(string type)
        {
            Type = type;
        }

        public override string Type { get; }

        /// <summary>
        /// The specific media type (MIME type) for a content resource.
        /// See <a href="https://iiif.io/api/presentation/3.0/#format">format</a>
        /// </summary>
        /// <remarks>Only content resources may have the Format property</remarks>
        [JsonProperty(Order = 20)]
        public string? Format { get; set; }

        /// <summary>
        /// The language or languages used in the content of this external resource.
        /// See <a href="https://iiif.io/api/presentation/3.0/#language">language</a>
        /// </summary>
        [JsonProperty(Order = 21)]
        public List<string>? Language { get; set; }

        [JsonProperty(Order = 900)]
        public List<AnnotationPage>? Annotations { get; set; }
    }
}
