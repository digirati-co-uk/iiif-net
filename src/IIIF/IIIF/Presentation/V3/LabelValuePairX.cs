using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using IIIF.Presentation.V3.Strings;
using IIIF.Utils;

namespace IIIF.Presentation.V3;

public static class LabelValuePairX
{
    /// <summary>
    /// Try get values associated with specified language and label.
    /// </summary>
    /// <param name="labelValuePairs">Collection of <see cref="labelValuePairs"/> to search</param>
    /// <param name="language">Language to get values for</param>
    /// <param name="label">Label to get values for</param>
    /// <param name="languageMap">Found <see cref="LanguageMap"/></param>
    /// <returns>true if matching value found, else false</returns>
    public static bool TryGetValue(
        this List<LabelValuePair>? labelValuePairs, 
        string language, 
        string label,
        [NotNullWhen(true)] out LanguageMap? languageMap)
    {
        languageMap = null;
        if (labelValuePairs.IsNullOrEmpty()) return false;

        foreach (var langValues in labelValuePairs.Where(lvp => lvp.Label.ContainsKey(language)))
        {
            if (langValues.Label.Any(l => l.Value.Contains(label)))
            {
                languageMap = langValues.Value;
                return true;
            }
        }

        return false;
    }
}