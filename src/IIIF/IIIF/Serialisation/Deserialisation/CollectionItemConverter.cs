﻿using System;
using System.Collections.Generic;
using IIIF.Presentation.V3;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

public class CollectionItemConverter : ReadOnlyConverter<ICollectionItem>
{
    public override ICollectionItem? ReadJson(JsonReader reader, Type objectType, ICollectionItem? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var type = jsonObject["type"].Value<string>();
        ICollectionItem collectionItem = null;
            
        // Look for consumer-provided mapping
        if (serializer.Context.Context is IDictionary<string, Func<JObject, ICollectionItem>> customMappings
            && customMappings.TryGetValue(type, out var customMapping))
        {
            collectionItem = customMapping(jsonObject);
        }

        if (collectionItem == null)
        {
            collectionItem = type switch
            {
                nameof(Collection) => new Collection(),
                nameof(Manifest) => new Manifest(),
                _ => null
            };
        }

        serializer.Populate(jsonObject.CreateReader(), collectionItem);
        return collectionItem;
    }
}