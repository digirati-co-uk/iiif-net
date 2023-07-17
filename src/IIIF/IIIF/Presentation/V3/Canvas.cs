using System.Collections.Generic;
using IIIF.Presentation.V3.Annotation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3;

/// <summary>
/// A Canvas represents an individual page or view and acts as a central point for assembling the different content
/// resources that make up the display.
/// See <a href="https://iiif.io/api/presentation/3.0/#53-canvas">Canvas docs</a>.
/// </summary>
public class
    Canvas : StructureBase, IStructuralLocation, IPaintable // but not ISpatial or ITemporal - that's for content
{
    public override string Type => nameof(Canvas);

    /// <summary>
    /// The Width of the Canvas. This value does not have a unit.
    /// </summary>
    [JsonProperty(Order = 11)]
    public int? Width { get; set; }

    /// <summary>
    /// The Height of the Canvas. This value does not have a unit.
    /// </summary>
    [JsonProperty(Order = 12)]
    public int? Height { get; set; }

    /// <summary>
    /// The Duration of the Canvas, in seconds.
    /// </summary>
    [JsonProperty(Order = 13)]
    public double? Duration { get; set; }

    [JsonProperty(Order = 300)] public List<AnnotationPage>? Items { get; set; }

    /// <summary>
    /// Used to control serialisation logic for Canvas items that are <see cref="Annotation"/> Targets.
    /// If true then Target is serialised as simple Id string only.
    /// </summary>
    [JsonIgnore]
    public bool SerialiseTargetAsId { get; set; }
}