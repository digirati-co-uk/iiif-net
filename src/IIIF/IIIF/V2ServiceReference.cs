namespace IIIF;

public class V2ServiceReference : IService
{
    [JsonProperty(PropertyName = "@id", Order = 1)]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "@type", Order = 2)]
    public string? Type { get; set; }

    public V2ServiceReference()
    {
    }

    public V2ServiceReference(IService service)
    {
        Id = service.Id;
        Type = service.Type;
    }
}