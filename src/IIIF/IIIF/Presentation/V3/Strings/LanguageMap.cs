using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3.Strings
{
    [JsonConverter(typeof(LanguageMapSerialiser))]
    public class LanguageMap : Dictionary<string, List<string>>
    {
        public LanguageMap() { }

        public LanguageMap(string language, string singleValue)
        {
            this[language] = new List<string> { singleValue };
        }
        
        public LanguageMap(string language, IEnumerable<string> values)
        {
            this[language] = values.ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (List<string> value in Values)
            {
                foreach (string s in value)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.Append(s);
                }
            }
            return sb.ToString();
        }

        public string Join(string separator)
        {
            return String.Join(separator, Values.SelectMany(s => s.ToList()));
        }
    }
}
