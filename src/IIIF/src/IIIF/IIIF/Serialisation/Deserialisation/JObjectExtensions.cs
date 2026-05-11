using System.Linq;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

internal static class JObjectExtensions
{
    /// <summary>
    /// Removes properties whose value is an empty array.
    /// Some real-world manifests use <c>[]</c> in place of <c>null</c> for optional
    /// single-object properties (e.g. <c>"placeholderCanvas": []</c>), which causes
    /// converters that call <see cref="JObject.Load"/> to throw. Stripping them before
    /// <c>Populate</c> lets the property default to null without error.
    /// </summary>
    public static void StripEmptyArrays(this JObject jsonObject)
    {
        foreach (var prop in jsonObject.Properties().Where(p => p.Value is JArray { Count: 0 }).ToList())
            prop.Remove();
    }
}
