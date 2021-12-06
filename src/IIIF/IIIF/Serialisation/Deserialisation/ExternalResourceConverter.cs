using System;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="ExternalResource"/> objects to concrete type.
    /// </summary>
    public class ExternalResourceConverter : ReadOnlyConverter<ExternalResource>
    {
        public override ExternalResource? ReadJson(JsonReader reader, Type objectType, ExternalResource? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var type = jsonObject["type"].Value<string>();
            ExternalResource externalResource = type switch
            {
                nameof(Audio) => new Audio(),
                nameof(Video) => new Video(),
                nameof(Image) => new Image(),
                _ => new ExternalResource(type)
            };

            serializer.Populate(jsonObject.CreateReader(), externalResource);
            return externalResource;
        }
    }
}