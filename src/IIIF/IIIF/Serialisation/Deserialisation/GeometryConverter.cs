using System;
using IIIF.Presentation.V3.NavPlace;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

public class GeometryConverter: ReadOnlyConverter<Geometry>
{
    public override Geometry? ReadJson(JsonReader reader, Type objectType, Geometry? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var type = jsonObject["type"].Value<string>();
        Geometry geometry = type switch
        {
            nameof(Point) => new Point(),
            nameof(MultiPoint) => new MultiPoint(),
            nameof(LineString) => new LineString(),
            nameof(MultiLineString) => new MultiLineString(),
            nameof(Polygon) => new Polygon(),
            nameof(MultiPolygon) => new MultiPolygon(),
            nameof(GeometryCollection) => new GeometryCollection(),
            _ => new UnknownGeometry(type)
        };

        serializer.Populate(jsonObject.CreateReader(), geometry);
        return geometry;
    }
}