using System.Collections.Generic;
using IIIF.Presentation.V3.Extensions.NavPlace;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IIIF.Tests.Serialisation;

public class GeometrySerialisationTests
{
    private readonly JsonSerializerSettings jsonSerializerSettings;

    public GeometrySerialisationTests()
    {
        // NOTE: Using JsonSerializerSettings to facilitate testing as it makes it a LOT easier
        jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> { new GeometryConverter() }
        };
    }

    [Fact]
    public void Serialize_ConvertsPoint()
    {
        // Arrange
        var geometry = new Point { Coordinates = new List<double> {100.0, 20.2, 10.1} };
        const string expected = "{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsPoint()
    {
        // Arrange
        const string point = "{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}";
        var expected = new Point { Coordinates = new List<double> {100.0, 20.2, 10.1} };

        // Act
        var result = JsonConvert.DeserializeObject<Point>(point, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsMultipoint()
    {
        // Arrange
        var geometry = new MultiPoint
            { Coordinates = new List<List<double>> { new() { 100.0, 0.0 }, new() { 101.0, 1.0 } } };
        const string expected = "{\"type\":\"MultiPoint\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsMultiPoint()
    {
        // Arrange
        const string multiPoint = "{\"type\":\"MultiPoint\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";
        var expected = new MultiPoint
            { Coordinates = new List<List<double>> { new() { 100.0, 0.0 }, new() { 101.0, 1.0 } } };

        // Act
        var result = JsonConvert.DeserializeObject<MultiPoint>(multiPoint, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsLineString()
    {
        // Arrange - see https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.2
        var geometry = new LineString { Coordinates = new List<List<double>> { new(){100.0, 0.0}, new(){101.0, 1.0}} };
        const string expected = "{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsLineString()
    {
        // Arrange - see https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.2
        const string lineString = "{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";
        var expected = new LineString { Coordinates = new List<List<double>> { new(){100.0, 0.0}, new(){101.0, 1.0}} };

        // Act
        var result = JsonConvert.DeserializeObject<LineString>(lineString, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_ConvertsLineString_WithMultiplePositions()
    {
        // Arrange — the issue #91 failing case: coordinates[0] is a position array, not a double
        const string lineString = "{\"type\":\"LineString\",\"coordinates\":[[100.0,0.0],[101.0,1.0],[102.0,2.0]]}";
        var expected = new LineString
        {
            Coordinates = new List<List<double>>
            {
                new() {100.0, 0.0},
                new() {101.0, 1.0},
                new() {102.0, 2.0}
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<LineString>(lineString, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsMultiLineString()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.5
        var geometry = new MultiLineString
        {
            Coordinates = new List<List<List<double>>>
            {
                new() { new(){100.0, 0.0}, new(){101.0, 1.0} },
                new() { new(){102.0, 2.0}, new(){103.0, 3.0} }
            }
        };
        const string expected = "{\"type\":\"MultiLineString\",\"coordinates\":[[[100.0,0.0],[101.0,1.0]],[[102.0,2.0],[103.0,3.0]]]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsMultiLineString()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.5
        const string multiLineString = "{\"type\":\"MultiLineString\",\"coordinates\":[[[100.0,0.0],[101.0,1.0]],[[102.0,2.0],[103.0,3.0]]]}";
        var expected = new MultiLineString
        {
            Coordinates = new List<List<List<double>>>
            {
                new() { new(){100.0, 0.0}, new(){101.0, 1.0} },
                new() { new(){102.0, 2.0}, new(){103.0, 3.0} }
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<MultiLineString>(multiLineString, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsPolygon()
    {
        // Arrange — exterior ring only (closed: first == last position)
        // see https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.3
        var geometry = new Polygon
        {
            Coordinates = new List<List<List<double>>>
            {
                new()
                {
                    new() { 100.0, 0.0 }, new() { 101.0, 0.0 }, new() { 101.0, 1.0 }, new() { 100.0, 1.0 },
                    new() { 100.0, 0.0 }
                }
            }
        };
        const string expected = "{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsPolygon()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.3
        const string polygon = "{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]}";
        var expected = new Polygon
        {
            Coordinates = new List<List<List<double>>>
            {
                new()
                {
                    new() { 100.0, 0.0 }, new() { 101.0, 0.0 }, new() { 101.0, 1.0 }, new() { 100.0, 1.0 },
                    new() { 100.0, 0.0 }
                }
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<Polygon>(polygon, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_ConvertsPolygon_WithHole()
    {
        // Arrange — the issue #91 failing case: exterior ring + interior ring (hole)
        // see https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.3
        const string polygon = "{\"type\":\"Polygon\",\"coordinates\":[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]],[[100.2,0.2],[100.2,0.8],[100.8,0.8],[100.8,0.2],[100.2,0.2]]]}";
        var expected = new Polygon
        {
            Coordinates = new List<List<List<double>>>
            {
                new()
                {
                    new() { 100.0, 0.0 }, new() { 101.0, 0.0 }, new() { 101.0, 1.0 }, new() { 100.0, 1.0 },
                    new() { 100.0, 0.0 }
                },
                new()
                {
                    new() { 100.2, 0.2 }, new() { 100.2, 0.8 }, new() { 100.8, 0.8 }, new() { 100.8, 0.2 },
                    new() { 100.2, 0.2 }
                }
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<Polygon>(polygon, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsMultiPolygon()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.3
        var geometry = new MultiPolygon
        {
            Coordinates = new List<List<List<List<double>>>>
            {
                new()
                {
                    new()
                    {
                        new() { 102.0, 2.0 }, new() { 103.0, 2.0 }, new() { 103.0, 3.0 }, new() { 102.0, 3.0 },
                        new() { 102.0, 2.0 }
                    }
                },
                new()
                {
                    new()
                    {
                        new() { 100.0, 0.0 }, new() { 101.0, 0.0 }, new() { 101.0, 1.0 }, new() { 100.0, 1.0 },
                        new() { 100.0, 0.0 }
                    }
                }
            }
        };

        const string expected = "{\"type\":\"MultiPolygon\",\"coordinates\":[[[[102.0,2.0],[103.0,2.0],[103.0,3.0],[102.0,3.0],[102.0,2.0]]],[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsMultiPolygon()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.6
        const string multiPolygon = "{\"type\":\"MultiPolygon\",\"coordinates\":[[[[102.0,2.0],[103.0,2.0],[103.0,3.0],[102.0,3.0],[102.0,2.0]]],[[[100.0,0.0],[101.0,0.0],[101.0,1.0],[100.0,1.0],[100.0,0.0]]]]}";
        var expected = new MultiPolygon
        {
            Coordinates = new List<List<List<List<double>>>>
            {
                new()
                {
                    new()
                    {
                        new() { 102.0, 2.0 }, new() { 103.0, 2.0 }, new() { 103.0, 3.0 }, new() { 102.0, 3.0 },
                        new() { 102.0, 2.0 }
                    }
                },
                new()
                {
                    new()
                    {
                        new() { 100.0, 0.0 }, new() { 101.0, 0.0 }, new() { 101.0, 1.0 }, new() { 100.0, 1.0 },
                        new() { 100.0, 0.0 }
                    }
                }
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<MultiPolygon>(multiPolygon, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Serialize_ConvertsGeometryCollection()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.6
        var geometry = new GeometryCollection
        {
            Geometries = new List<Geometry>
            {
                new MultiPolygon
                {
                    Coordinates = new List<List<List<List<double>>>>
                    {
                        new()
                        {
                            new()
                            {
                                new() { 102.0, 2.0 }, new() { 103.0, 2.0 }, new() { 103.0, 3.0 }, new() { 102.0, 2.0 }
                            }
                        }
                    }
                },
                new Point
                {
                    Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                }
            }
        };

        const string expected = "{\"type\":\"GeometryCollection\",\"geometries\":[{\"type\":\"MultiPolygon\",\"coordinates\":[[[[102.0,2.0],[103.0,2.0],[103.0,3.0],[102.0,2.0]]]]},{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_ConvertsGeometryCollection()
    {
        // Arrange - https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.7
        const string geometryCollection = "{\"type\":\"GeometryCollection\",\"geometries\":[{\"type\":\"MultiPolygon\",\"coordinates\":[[[[102.0,2.0],[103.0,2.0],[103.0,3.0],[102.0,2.0]]]]},{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";
        var expected = new GeometryCollection
        {
            Geometries = new List<Geometry>
            {
                new MultiPolygon
                {
                    Coordinates = new List<List<List<List<double>>>>
                    {
                        new()
                        {
                            new() { new(){102.0, 2.0}, new(){103.0, 2.0}, new(){103.0, 3.0}, new(){102.0, 2.0} }
                        }
                    }
                },
                new Point
                {
                    Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                }
            }
        };

        // Act
        var result = JsonConvert.DeserializeObject<GeometryCollection>(geometryCollection, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
