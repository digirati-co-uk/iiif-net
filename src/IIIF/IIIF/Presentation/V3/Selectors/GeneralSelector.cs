using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace IIIF.Presentation.V3.Selectors;

/// <summary>
/// Generic fall-through <see cref="ISelector"/> implementation for storing non-standard selector types.
/// </summary>
public class GeneralSelector : ISelector
{
    public string? Type { get; set; }
    
    [JsonExtensionData]
    public IDictionary<string, JToken>? AdditionalProperties { get; set; }
}