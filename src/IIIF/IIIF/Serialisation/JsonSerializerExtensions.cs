using System;
using System.Linq;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
    internal static class JsonSerializerExtensions
    {
        /// <summary>
        /// Create a copy of <see cref="JsonSerializer"/>, filtering Converters array
        /// </summary>
        /// <param name="serializer">JsonSerializer to copy.</param>
        /// <param name="converterFilter">Predicate to filter JsonConverters - if true then converter copied</param>
        /// <returns>New JsonSerializer instance</returns>
        /// <remarks>Based on https://stackoverflow.com/a/38230327/83096</remarks>
        public static JsonSerializer CreateCopy(this JsonSerializer serializer, Func<JsonConverter, bool> converterFilter)
        {
            var copiedSerializer = new JsonSerializer
            {
                Context = serializer.Context,
                Culture = serializer.Culture,
                ContractResolver = serializer.ContractResolver,
                ConstructorHandling = serializer.ConstructorHandling,
                CheckAdditionalContent = serializer.CheckAdditionalContent,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateParseHandling = serializer.DateParseHandling,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DefaultValueHandling = serializer.DefaultValueHandling,
                EqualityComparer = serializer.EqualityComparer,
                FloatFormatHandling = serializer.FloatFormatHandling,
                Formatting = serializer.Formatting,
                FloatParseHandling = serializer.FloatParseHandling,
                MaxDepth = serializer.MaxDepth,
                MetadataPropertyHandling = serializer.MetadataPropertyHandling,
                MissingMemberHandling = serializer.MissingMemberHandling,
                NullValueHandling = serializer.NullValueHandling,
                ObjectCreationHandling = serializer.ObjectCreationHandling,
                PreserveReferencesHandling = serializer.PreserveReferencesHandling,
                ReferenceResolver = serializer.ReferenceResolver,
                ReferenceLoopHandling = serializer.ReferenceLoopHandling,
                StringEscapeHandling = serializer.StringEscapeHandling,
                TraceWriter = serializer.TraceWriter,
                TypeNameHandling = serializer.TypeNameHandling,
                SerializationBinder = serializer.SerializationBinder,
                TypeNameAssemblyFormatHandling = serializer.TypeNameAssemblyFormatHandling
            }; 
            
            foreach (var converter in serializer.Converters.Where(c => converterFilter(c)))
            {
                copiedSerializer.Converters.Add(converter);
            }
            return copiedSerializer;
        }
    }
}