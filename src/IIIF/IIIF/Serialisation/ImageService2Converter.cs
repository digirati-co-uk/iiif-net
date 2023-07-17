using System;
using IIIF.ImageApi.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation;

/// <summary>
/// Converter for <see cref="ImageService2"/> object, serialises Profile and ProfileDescription into single
/// "profile" property.
/// </summary>
public class ImageService2Converter : JsonConverter<ImageService2>
{
    private const string ProfileProperty = "profile";

    public override void WriteJson(JsonWriter writer, ImageService2? value, JsonSerializer serializer)
    {
        // Create a copy to avoid circular reference issue bombing out
        var customSerializer = serializer.CreateCopy(converter => converter is not ImageService2Converter);
        if (value?.ProfileDescription == null)
        {
            // If ProfileDescription is null then no special handling required
            customSerializer.Serialize(writer, value);
            return;
        }

        var imageService = JObject.FromObject(value, customSerializer);
        imageService[ProfileProperty] = GetProfileElement(value, customSerializer);
        imageService.WriteTo(writer);
    }

    public override ImageService2 ReadJson(JsonReader reader, Type objectType, ImageService2? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        existingValue ??= new ImageService2();

        DeserialiseProperty(existingValue, jsonObject);

        serializer.Populate(jsonObject.CreateReader(), existingValue);
        return existingValue;
    }

    private static void DeserialiseProperty(ImageService2 imageService, JObject jsonObject)
    {
        var profileToken = jsonObject.GetValue(ProfileProperty);

        // If "profile" property null or a string then default serialisation will cope
        if (profileToken == null || profileToken.Type == JTokenType.String) return;

        void SetProfileDescription(JObject profileDescription)
        {
            imageService.ProfileDescription = profileDescription.ToObject<ProfileDescription>();
        }

        var handled = false;
        if (profileToken is JArray profileArray)
        {
            // if "profile" is an array then populate profile + profileDescription properties
            foreach (var profileValue in profileArray)
                if (profileValue.Type == JTokenType.String)
                    imageService.Profile = profileValue.ToString();
                else if (profileValue is JObject profileDescription) SetProfileDescription(profileDescription);

            handled = true;
        }
        else if (profileToken is JObject profileDescription)
        {
            // else if "profile" is a JObject, populate ProfileDescription property only
            SetProfileDescription(profileDescription);
            handled = true;
        }

        if (handled) jsonObject.Remove(ProfileProperty);
    }

    private static JToken GetProfileElement(ImageService2? value, JsonSerializer serializer)
    {
        var profileDescription = JToken.FromObject(value.ProfileDescription, serializer);

        var newProfile = string.IsNullOrEmpty(value.Profile)
            ? new JProperty("property", profileDescription)
            : new JProperty("property", value.Profile, profileDescription);

        return newProfile.Value;
    }
}