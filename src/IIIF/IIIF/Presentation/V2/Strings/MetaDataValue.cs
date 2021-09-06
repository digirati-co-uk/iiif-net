using System;
using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V2.Serialisation;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Strings
{
    /// <summary>
    /// A collection of <see cref="LanguageValues"/> for metadata.
    /// </summary>
    [JsonConverter(typeof(MetaDataValueSerialiser))]
    public class MetaDataValue
    {
        public List<LanguageValue> LanguageValues { get; set; }

        public MetaDataValue(string value) 
            => LanguageValues = new List<LanguageValue> {new() {Value = value}};

        public MetaDataValue(string value, string language) 
            => LanguageValues = new List<LanguageValue> {new() {Value = value, Language = language}};

        public MetaDataValue(IEnumerable<LanguageValue> languageValues) 
            => LanguageValues = languageValues.ToList();

        /// <summary>
        /// Create a new MetaDataValue object from specified LanguageMap.
        /// </summary>
        /// <param name="languageMap">LanguageMap to convert to MetaDataValue</param>
        /// <param name="ignoreLanguage">If true language not set</param>
        /// <param name="languagePredicate">Optional predicate to filter MetaDataValues.</param>
        /// <returns>Null if LanguageMap null, else new MetaDataValue object </returns>
        public static MetaDataValue? Create(LanguageMap? languageMap, bool ignoreLanguage = false,
            Func<string, bool>? languagePredicate = null)
        {
            // "none" is used in P3 if language unknown
            const string unknownLanguage = "none";
            if (languageMap == null) return null;

            languagePredicate ??= s => true;

            var langVals = new List<LanguageValue>();
            foreach (var kvp in languageMap)
            {
                var language = ignoreLanguage || kvp.Key == unknownLanguage ? null : kvp.Key;

                langVals.AddRange(kvp.Value
                    .Where(languagePredicate)
                    .Select(values => new LanguageValue
                    {
                        Language = language,
                        Value = values
                    }));
            }

            return new MetaDataValue(langVals);
        }
    }
}