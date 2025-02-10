namespace IIIF.Presentation.V3.Feature;

public class Feature
{
    [JsonProperty(Order = 1)]
    public string Id { get; set; }
    [JsonProperty(Order = 2)]
    public string Type { get; set; }
    [JsonProperty(Order = 3)]
    public Properties Properties { get; set; }
    [JsonProperty(Order = 4)]
    public Geometry Geometry { get; set; }
}