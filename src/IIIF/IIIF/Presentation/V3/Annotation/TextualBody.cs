using System;
using System.Collections.Generic;

namespace IIIF.Presentation.V3.Annotation;

public class TextualBody : ResourceBase
{
    public TextualBody(string value)
    {
        Value = value;
    }

    public TextualBody(string value, string format)
    {
        Value = value;
        Format = format;
    }

    public override string Type => nameof(TextualBody);
    
    [JsonProperty(Order = 299)]
    public string? Language { get; set; }

    [JsonProperty(Order = 300)]
    public string? Value { get; set; }
    
    [JsonProperty(Order = 301)]
    public string? Format { get; set; }
    
    [JsonProperty(Order = 302)]
    public string? Motivation { get; set; }
    
    [JsonProperty(Order = 302)]
    public string? Purpose { get; set; }
    
    [JsonProperty(Order = 303)]
    public string? Creator { get; set; }
    
    [JsonProperty(Order = 304)]
    public DateTime? Created { get; set; }
    
    [JsonProperty(Order = 305)]
    public DateTime? Modified { get; set; }
    
    [JsonProperty(Order = 306)]
    public string? Generator { get; set; }
    
    [JsonProperty(Order = 307)]
    public DateTime? Generated { get; set; }
    
    [JsonProperty(Order = 308)]
    public string? Role { get; set; }
    
    [JsonProperty(Order = 309)]
    public string? Audience { get; set; }
    
    [JsonProperty(Order = 310)]
    public string? Accessibility { get; set; }
    
    [JsonProperty(Order = 311)]
    public string? Canonical { get; set; }
    
    [JsonProperty(Order = 312)]
    public string? Via { get; set; }
}