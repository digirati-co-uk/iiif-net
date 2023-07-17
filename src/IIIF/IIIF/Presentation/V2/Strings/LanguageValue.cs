using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Strings;

/// <summary>
/// Represents a value object with optional language.
/// </summary>
public class LanguageValue : ValueObject
{
    [JsonProperty(Order = 4, PropertyName = "@language")]
    public string? Language { get; set; }
}