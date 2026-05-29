using IIIF.Presentation.V3.Annotation;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// Helper class to convert json to concrete annotation types, used by different converters.
/// </summary>
internal static class MotivationConverter
{
    public static Annotation TryGetAnnotation(JObject jsonObject, string motivation)
    {
        var annotation = motivation switch
        {
            Presentation.V3.Constants.Motivation.Painting => GetPaintingAnnotation(jsonObject),
            Presentation.V3.Constants.Motivation.Supplementing => new SupplementingDocumentAnnotation(),
            Presentation.V3.Constants.Motivation.Classifying => new TypeClassifyingAnnotation(),
            _ => jsonObject["body"] is not { HasValues: true } ? new Annotation() : new GeneralAnnotation(motivation) 
        };

        return annotation;
    }
    
    /// <summary>
    /// PaintingAnnotation only supports single "body", multiple bodies are rare but possible
    /// </summary>
    private static Annotation GetPaintingAnnotation(JObject jsonObject) =>
        jsonObject.SelectToken("body") is JArray
            ? new GeneralAnnotation(Presentation.V3.Constants.Motivation.Painting)
            : new PaintingAnnotation();
}