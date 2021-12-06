using System.Linq;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3.Strings
{
    public class LabelValuePair
    {
        private LanguageMap valueMap;
        private LanguageMap labelMap;
        
        /// <summary>
        /// This helper method can take any number of values, but will only add them if not empty.
        /// It _will_ add whitespace strings, though - you might want that.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="label"></param>
        /// <param name="values"></param>
        public LabelValuePair(string language, string label, params string?[] values)
        {
            var actualValues = values
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
            labelMap = new LanguageMap(language, label);
            valueMap = new LanguageMap(language, actualValues.FirstOrDefault());
            foreach (var value in actualValues.Skip(1))
            {
                valueMap[language].Add(value);
            }
        }
        
        [JsonConstructor]
        public LabelValuePair(LanguageMap label, LanguageMap value)
        {
            labelMap = label;
            valueMap = value;
        }

        public LanguageMap Label { get => labelMap; set => labelMap = value; }
        public LanguageMap Value { get => valueMap; set => valueMap = value; }
    }

}
