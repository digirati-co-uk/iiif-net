using System;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

/// <summary>
/// Serialises enum as camelCase representation of enum value
/// </summary>
public class EnumStringValueConverter : WriteOnlyConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Enum enumValue)
            throw new ArgumentException(
                $"EnumCamelCaseValueConverter expected enum but got {value.GetType().Name}", nameof(value));

        writer.WriteValue(enumValue.ToString());
    }
}