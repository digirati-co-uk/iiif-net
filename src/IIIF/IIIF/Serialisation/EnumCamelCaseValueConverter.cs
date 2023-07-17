using System;
using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

/// <summary>
/// Serialises enum as camelCase representation of enum value
/// </summary>
public class EnumCamelCaseValueConverter : WriteOnlyConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Enum enumValue)
            throw new ArgumentException(
                $"EnumCamelCaseValueConverter expected enum but got {value.GetType().Name}", nameof(value));

        writer.WriteValue(ConvertToCamelCase(enumValue.ToString()));
    }

    private string ConvertToCamelCase(string name)
    {
        // Based on System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName implementation
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0])) return name;

        var chars = name.ToCharArray();
        FixCasing(chars);
        return new string(chars);
    }

    private static void FixCasing(Span<char> chars)
    {
        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i])) break;

            var hasNext = i + 1 < chars.Length;

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                // If the next char is a space, lowercase current char before exiting.
                if (chars[i + 1] == ' ') chars[i] = char.ToLowerInvariant(chars[i]);

                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}