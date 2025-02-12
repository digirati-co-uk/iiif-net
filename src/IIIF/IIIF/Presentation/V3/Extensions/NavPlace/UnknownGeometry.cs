namespace IIIF.Presentation.V3.Extensions.NavPlace;

internal sealed class UnknownGeometry : Geometry
{
    internal UnknownGeometry(string type)
    {
        Type = type;
    }
}