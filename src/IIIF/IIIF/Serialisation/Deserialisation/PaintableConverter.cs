using System;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="IPaintable"/> objects to concrete type.
    /// </summary>
    public class PaintableConverter : ReadOnlyConverter<IPaintable>
    {
        public override IPaintable? ReadJson(JsonReader reader, Type objectType, IPaintable? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            IPaintable? paintable = jsonObject["type"].Value<string>() switch
            {
                nameof(Audio) => new Audio(),
                nameof(Video) => new Video(),
                nameof(Image) => new Image(),
                nameof(Canvas) => new Canvas(),
                "Choice" => new PaintingChoice(),
                _ => null
            };

            serializer.Populate(jsonObject.CreateReader(), paintable);
            return paintable;
        }
    }
}