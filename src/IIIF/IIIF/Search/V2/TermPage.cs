using System.Collections.Generic;

namespace IIIF.Search.V2;

public class TermPage : JsonLdBase
{
    [JsonProperty(Order = 1, PropertyName = "@context")]
    public new string Context = SearchService2.Search2Context;
    
    [JsonProperty(Order = 2, PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(Order = 3, PropertyName = "type")]
    public string Type => nameof(TermPage);
    
    [JsonProperty(Order = 5, PropertyName = "items")]
    public List<Term> Items { get; set; } = new();
}

public class Term
{
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }
}