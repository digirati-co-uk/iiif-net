using IIIF.Presentation.V2;
using Newtonsoft.Json;

namespace IIIF.Search.V1;

public class TermList : ResourceBase, IHasIgnorableParameters
{
    public override string? Type
    {
        get => "search:TermList";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 20, PropertyName = "ignored")]
    public string[]? Ignored { get; set; }

    [JsonProperty(Order = 40, PropertyName = "terms")]
    public Term[]? Terms { get; set; }
}