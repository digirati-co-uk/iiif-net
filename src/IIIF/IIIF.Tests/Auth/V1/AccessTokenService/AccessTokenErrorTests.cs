using FluentAssertions;
using IIIF.Auth.V1.AccessTokenService;
using Xunit;

namespace IIIF.Tests.Auth.V1.AccessTokenService
{
    public class AccessTokenErrorTests
    {
        [Theory]
        [InlineData(AccessTokenErrorConditions.InvalidRequest,
            "The service could not process the information sent in the body of the request.")]
        [InlineData(AccessTokenErrorConditions.MissingCredentials,
            "The request did not have the credentials required.")]
        [InlineData(AccessTokenErrorConditions.InvalidCredentials,
            "The request had credentials that are not valid for the service.")]
        [InlineData(AccessTokenErrorConditions.InvalidOrigin,
            "The request came from a different origin than that specified in the access cookie service request, or an origin that the server rejects for other reasons.")]
        [InlineData(AccessTokenErrorConditions.Unavailable,
            "The request could not be fulfilled for reasons other than those listed above, such as scheduled maintenance.")]
        public void Ctor_ErrorOnly_UsesDefaultDescription(AccessTokenErrorConditions error, string description)
        {
            // Arrange
            var accessTokenError = new AccessTokenError(error);

            // Assert
            accessTokenError.Error.Should().Be(error);
            accessTokenError.Description.Should().Be(description);
        }
    }
}