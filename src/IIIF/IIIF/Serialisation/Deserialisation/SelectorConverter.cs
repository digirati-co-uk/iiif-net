using System;
using IIIF.Presentation.V3.Selectors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="ISelector"/> objects to concrete type.
    /// </summary>
    public class SelectorConverter : ReadOnlyConverter<ISelector>
    {
        public override ISelector? ReadJson(JsonReader reader, Type objectType, ISelector? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            ISelector selector = jsonObject["type"].Value<string>() switch
            {
                nameof(AudioContentSelector) => new AudioContentSelector(),
                nameof(ImageApiSelector) => new ImageApiSelector(),
                nameof(PointSelector) => new PointSelector(),
                nameof(VideoContentSelector) => new VideoContentSelector(),
            };

            serializer.Populate(jsonObject.CreateReader(), selector);
            return selector;
        }
    }
}