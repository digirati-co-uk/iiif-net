using System.Collections.Generic;

namespace IIIF.Presentation.V3.Annotation;

/// <summary>
/// Marker interface for things that can be painted ONTO a Canvas.
/// This includes other canvases.
/// </summary>
public interface IPaintable
{
    List<IService>? Service { get; set; }
}