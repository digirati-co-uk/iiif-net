namespace IIIF.Presentation.V3.NavPlace;

internal sealed class UnknownGeometry : Geometry
{
    internal UnknownGeometry(string type)
    {
        Type = type;
    }
}