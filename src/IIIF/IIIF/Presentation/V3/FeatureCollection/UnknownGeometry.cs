namespace IIIF.Presentation.V3.FeatureCollection;

internal sealed class UnknownGeometry : Geometry
{
    internal UnknownGeometry(string type)
    {
        Type = type;
    }
}