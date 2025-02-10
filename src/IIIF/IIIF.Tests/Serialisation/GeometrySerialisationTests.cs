using System.Collections.Generic;
using IIIF.Presentation.V3.Feature;
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
        var geometry = new MultiPoint { Coordinates = new List<Point> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };
        const string expected = "{\"type\":\"MultiPoint\",\"coordinates\":[{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsMultiPoint()
    {
        // Arrange
        const string multiPoint = "{\"type\":\"MultiPoint\",\"coordinates\":[{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";
        var expected = new MultiPoint { Coordinates = new List<Point> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };

        // Act
        var result = JsonConvert.DeserializeObject<MultiPoint>(multiPoint, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Serialize_ConvertsLineString()
    {
        // Arrange
        var geometry = new LineString { Coordinates = new List<double> {100.0, 20.2, 10.1} };
        const string expected = "{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsLineString()
    {
        // Arrange
        const string lineString = "{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}";
        var expected = new LineString { Coordinates = new List<double> {100.0, 20.2, 10.1} };

        // Act
        var result = JsonConvert.DeserializeObject<LineString>(lineString, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Serialize_ConvertsMultiLineString()
    {
        // Arrange
        var geometry = new MultiLineString { Coordinates = new List<LineString> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };
        const string expected = "{\"type\":\"MultiLineString\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsMultiLineString()
    {
        // Arrange
        const string multiLineString = "{\"type\":\"MultiLineString\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}";
        var expected = new MultiLineString { Coordinates = new List<LineString> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };

        // Act
        var result = JsonConvert.DeserializeObject<MultiLineString>(multiLineString, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Serialize_ConvertsPolygon()
    {
        // Arrange
        var geometry = new Polygon { Coordinates = new List<LineString> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };
        const string expected = "{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsPolygon()
    {
        // Arrange
        const string polygon = "{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}";
        var expected = new Polygon { Coordinates = new List<LineString> { new() {Coordinates = new List<double> {100.0, 20.2, 10.1}}  } };

        // Act
        var result = JsonConvert.DeserializeObject<Polygon>(polygon, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Serialize_ConvertsMultiPolygon()
    {
        // Arrange
        var geometry = new MultiPolygon
        {
            Coordinates = new List<Polygon>
            {
                new()
                {
                    Coordinates = new List<LineString>
                    { 
                        new()
                        {
                            Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                        }
                    }
                }
            }
        };
        
        const string expected = "{\"type\":\"MultiPolygon\",\"coordinates\":[{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsMultiPolygon()
    {
        // Arrange
        const string multiPolygon = "{\"type\":\"MultiPolygon\",\"coordinates\":[{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}]}";
        var expected = new MultiPolygon
        {
            Coordinates = new List<Polygon>
            {
                new()
                {
                    Coordinates = new List<LineString>
                    { 
                        new()
                        {
                            Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                        }
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
        // Arrange
        var geometry = new GeometryCollection
        {
            Geometries = new List<Geometry>
            {
                new MultiPolygon
                {
                    Coordinates = new List<Polygon>
                    {
                        new()
                        {
                            Coordinates = new List<LineString>
                            {
                                new()
                                {
                                    Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                                }
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
        
        const string expected = "{\"type\":\"GeometryCollection\",\"geometries\":[{\"type\":\"MultiPolygon\",\"coordinates\":[{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}]},{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";

        // Act
        var result = JsonConvert.SerializeObject(geometry, jsonSerializerSettings);

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Deserialize_ConvertsGeometryCollection()
    {
        // Arrange
        const string multiPolygon = "{\"type\":\"GeometryCollection\",\"geometries\":[{\"type\":\"MultiPolygon\",\"coordinates\":[{\"type\":\"Polygon\",\"coordinates\":[{\"type\":\"LineString\",\"coordinates\":[100.0,20.2,10.1]}]}]},{\"type\":\"Point\",\"coordinates\":[100.0,20.2,10.1]}]}";
        var expected = new GeometryCollection
        {
            Geometries = new List<Geometry>
            {
                new MultiPolygon
                {
                    Coordinates = new List<Polygon>
                    {
                        new()
                        {
                            Coordinates = new List<LineString>
                            {
                                new()
                                {
                                    Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                                }
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

        // Act
        var result = JsonConvert.DeserializeObject<GeometryCollection>(multiPolygon, jsonSerializerSettings);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}