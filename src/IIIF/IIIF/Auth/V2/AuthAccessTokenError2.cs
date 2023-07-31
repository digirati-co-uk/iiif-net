using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Auth.V2;

public class AuthAccessTokenError2 : ResourceBase, IService
{
    /// <summary>
    /// The service could not process the access token request.
    /// </summary>
    public const string InvalidRequest = "invalidRequest";
    
    /// <summary>
    /// The request came from a different origin than that specified in the access service request, or an origin that
    /// the server rejects for other reasons.
    /// </summary>
    public const string InvalidOrigin = "invalidOrigin";
    
    /// <summary>
    /// The access token request did not have the required authorizing aspect.
    /// </summary>
    public const string MissingAspect = "missingAspect";
    
    /// <summary>
    /// The access token request had the aspect used for authorization but it was not valid.
    /// </summary>
    public const string InvalidAspect = "invalidAspect";
    
    /// <summary>
    /// The request had credentials that are no longer valid for the service.
    /// </summary>
    public const string ExpiredAspect = "expiredAspect";
    
    /// <summary>
    /// The request could not be fulfilled for reasons other than those listed above, such as scheduled maintenance.
    /// </summary>
    public const string Unavailable = "unavailable";

    public override string Type => nameof(AuthAccessTokenError2);

    [JsonProperty(Order = 101, PropertyName = "heading")]
    public LanguageMap? Heading { get; set; }

    [JsonProperty(Order = 102, PropertyName = "note")]
    public LanguageMap? Note { get; set; }
    
    /// <summary>
    /// The message identifier supplied by the client.
    /// </summary>
    [JsonProperty(Order = 103, PropertyName = "messageId")]
    public string MessageId { get; set; }

    public AuthAccessTokenError2(string profile, LanguageMap? note = null)
    {
        Context = Constants.IIIFAuth2Context;
        Profile = profile;
        Note = note;
    }

    public AuthAccessTokenError2()
    {
    }
}