using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace IIIF;

/// <summary>
/// Base class, serves as root for all IIIF models.
/// </summary>
public abstract class JsonLdBase
{
    [JsonProperty(Order = 1, PropertyName = "@context")]
    public object? Context { get; set; }

    /// <summary>
    /// Captures arbitrary extension properties for round-trip serialisation.
    /// "type" and "@type" are filtered out: they are read-only computed properties
    /// used as type discriminators and must not be stored here, otherwise they
    /// would be emitted twice during serialisation.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalProperties { get; set; } = new TypeFilteringDictionary();

    private class TypeFilteringDictionary : IDictionary<string, JToken>
    {
        private static readonly HashSet<string> IgnoredKeys = new() { "type", "@type" };
        private readonly Dictionary<string, JToken> inner = new();

        private bool IsAllowed(string key) => !IgnoredKeys.Contains(key);

        public JToken this[string key]
        {
            get => inner[key];
            set { if (IsAllowed(key)) inner[key] = value; }
        }

        public void Add(string key, JToken value) { if (IsAllowed(key)) inner[key] = value; }
        public void Add(KeyValuePair<string, JToken> item) { if (IsAllowed(item.Key)) inner[item.Key] = item.Value; }

        public ICollection<string> Keys => inner.Keys;
        public ICollection<JToken> Values => inner.Values;
        public int Count => inner.Count;
        public bool IsReadOnly => false;

        public bool ContainsKey(string key) => inner.ContainsKey(key);
        public bool TryGetValue(string key, out JToken value) => inner.TryGetValue(key, out value!);
        public bool Remove(string key) => inner.Remove(key);
        public bool Remove(KeyValuePair<string, JToken> item) => ((IDictionary<string, JToken>)inner).Remove(item);
        public bool Contains(KeyValuePair<string, JToken> item) => ((IDictionary<string, JToken>)inner).Contains(item);
        public void Clear() => inner.Clear();
        public void CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex) => ((IDictionary<string, JToken>)inner).CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator() => inner.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => inner.GetEnumerator();
    }
}