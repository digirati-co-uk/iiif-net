﻿using System.Collections.Generic;
using IIIF.Serialisation;

namespace IIIF.Presentation.V3.Annotation;

/// <summary>
/// Used for any annotation that declares a body
/// </summary>
public sealed class GeneralAnnotation : Annotation
{
    public GeneralAnnotation(string? motivation)
    {
        Motivation = motivation;
    }
    
    [ObjectIfSingle]
    [JsonProperty(Order = 500)] public List<ResourceBase>? Body { get; set; }
}