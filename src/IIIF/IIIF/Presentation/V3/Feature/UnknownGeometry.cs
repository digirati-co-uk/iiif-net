namespace IIIF.Presentation.V3.Feature;

internal sealed class UnknownGeometry : Geometry
{
    internal UnknownGeometry(string type)
    {
        Type = type;
    }
}