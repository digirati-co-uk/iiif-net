using System;
using IIIF.Presentation.V3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Range = IIIF.Presentation.V3.Range;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="IStructuralLocation"/> objects to concrete type.
    /// </summary>
    public class StructuralLocationConverter : ReadOnlyConverter<IStructuralLocation>
    {
        public override IStructuralLocation? ReadJson(JsonReader reader, Type objectType,
            IStructuralLocation? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            
            IStructuralLocation structuralLocation = jsonObject["type"].Value<string>() switch
            {
                nameof(Canvas) => new Canvas(),
                nameof(Range) => new Range(),
                nameof(SpecificResource) => new SpecificResource(),
            };
            
            serializer.Populate(jsonObject.CreateReader(), structuralLocation);
            return structuralLocation;
        }
    }
}