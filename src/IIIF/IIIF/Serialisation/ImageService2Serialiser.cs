using IIIF.ImageApi.V2;
using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Converter for <see cref="ImageService2"/> object, serialises Profile and ProfileDescription into single
    /// "profile" property.
    /// </summary>
    public class ImageService2Serialiser : WriteOnlyConverter<ImageService2>
    {
        public override void WriteJson(JsonWriter writer, ImageService2? value, JsonSerializer serializer)
        {
            // Create a copy to avoid circular reference issue bombing out
            var customSerializer = serializer.CreateCopy(converter => converter is not ImageService2Serialiser);
            if (value?.ProfileDescription == null) 
            {
                // If ProfileDescription is null then no special handling required
                customSerializer.Serialize(writer, value);
                return;
            }

            var imageService = JObject.FromObject(value, customSerializer);
            imageService["profile"] = GetProfileElement(value, customSerializer);
            imageService.WriteTo(writer);
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
}