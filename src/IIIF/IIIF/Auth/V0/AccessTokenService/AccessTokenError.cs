using System.ComponentModel;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Auth.V0.AccessTokenService
{
    /// <summary>
    /// Response from access token service in the result of an error
    /// </summary>
    /// <remarks>
    /// See https://iiif.io/api/auth/0.9/#access-token-error-conditions
    /// </remarks>
    public class AccessTokenError : JsonLdBase
    {
        [JsonProperty(PropertyName = "error", Order = 1)]
        [CamelCaseEnum]
        public AccessTokenErrorConditions Error { get; set; }
        
        [JsonProperty(PropertyName = "description", Order = 2)]
        public string Description { get; set; }

        public AccessTokenError()
        {
        }

        /// <summary>
        /// Create new AccessTokenError using provided error condition and default description for condition
        /// </summary>
        public AccessTokenError(AccessTokenErrorConditions error) : this(error, error.GetDescription())
        {
        }

        /// <summary>
        /// Create new AccessTokenError using provided error condition and custom description of error
        /// </summary>
        public AccessTokenError(AccessTokenErrorConditions error, string description)
        {
            Error = error;
            Description = description;
        }
    }


    public enum AccessTokenErrorConditions
    {
        /// <summary>
        /// The service could not process the information sent in the body of the request.
        /// </summary>
        [Description("The service could not process the information sent in the body of the request.")]
        InvalidRequest,

        /// <summary>
        /// The request did not have the credentials required.
        /// </summary>
        [Description("The request did not have the credentials required.")]
        MissingCredentials,

        /// <summary>
        /// The request had credentials that are not valid for the service.
        /// </summary>
        [Description("The request had credentials that are not valid for the service.")]
        InvalidCredentials,

        /// <summary>
        /// The request came from a different origin than that specified in the access cookie service request,
        /// or an origin that the server rejects for other reasons.
        /// </summary>
        [Description("The request came from a different origin than that specified in the access cookie service request, or an origin that the server rejects for other reasons.")]
        InvalidOrigin,

        /// <summary>
        /// The request could not be fulfilled for reasons other than those listed above, such as scheduled maintenance.
        /// </summary>
        [Description("The request could not be fulfilled for reasons other than those listed above, such as scheduled maintenance.")] 
        Unavailable
    }
}