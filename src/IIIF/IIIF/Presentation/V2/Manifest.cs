using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    /// <summary>
    /// The manifest response contains sufficient information for the client to initialize itself and begin to display
    /// something quickly to the user.
    /// </summary>
    /// <remarks>See https://iiif.io/api/presentation/2.1/#manifest</remarks>
    public class Manifest : IIIFPresentationBase
    {
        public override string? Type
        {
            get => "sc:Manifest";
            set => throw new System.NotImplementedException();
        }

        [JsonProperty(Order = 31, PropertyName = "viewingDirection")]
        public string? ViewingDirection { get; set; }

        [JsonProperty(Order = 40, PropertyName = "sequences")]
        public List<Sequence> Sequences { get; set; }
        
        [JsonProperty(Order = 50, PropertyName = "structures")]
        public List<Range>? Structures { get; set; }
    }
}