using System;
using IIIF.Presentation.V3;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

public class CollectionItemConverter : ReadOnlyConverter<ICollectionItem>
{
    public override ICollectionItem? ReadJson(JsonReader reader, Type objectType, ICollectionItem? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        ICollectionItem collectionItem = jsonObject["type"].Value<string>() switch
        {
            nameof(Collection) => new Collection(),
            nameof(Manifest) => new Manifest(),
            _ => null
        };

        serializer.Populate(jsonObject.CreateReader(), collectionItem);
        return collectionItem;
    }
}