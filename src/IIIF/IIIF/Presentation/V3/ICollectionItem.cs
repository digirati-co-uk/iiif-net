using System.Collections.Generic;

namespace IIIF.Presentation.V3;

/// <summary>
/// Marker interface for resources that can be in a Collection's Items property.
/// </summary>
public interface ICollectionItem
{
    List<IService>? Services { get; set; }
}