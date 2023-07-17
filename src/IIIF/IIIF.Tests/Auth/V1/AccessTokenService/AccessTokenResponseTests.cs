using FluentAssertions;
using IIIF.Auth.V1.AccessTokenService;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Auth.V1.AccessTokenService;

public class AccessTokenResponseTests
{
    private readonly JsonSerializerSettings jsonSerializerSettings;

    public AccessTokenResponseTests()
    {
        // NOTE: Using JsonSerializerSettings to facilitate testing as it makes it a LOT easier
        jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new PrettyIIIFContractResolver()
        };
    }

    [Theory]
    [InlineData(AccessTokenErrorConditions.InvalidRequest, "invalidRequest")]
    [InlineData(AccessTokenErrorConditions.MissingCredentials, "missingCredentials")]
    [InlineData(AccessTokenErrorConditions.InvalidCredentials, "invalidCredentials")]
    [InlineData(AccessTokenErrorConditions.InvalidOrigin, "invalidOrigin")]
    [InlineData(AccessTokenErrorConditions.Unavailable, "unavailable")]
    public void AccessTokenError_ReturnsExpectedJson(AccessTokenErrorConditions error, string description)
    {
        // Arrange
        var accessTokenError = new AccessTokenError(error, "test description");

        var expected = $"{{\"error\":\"{description}\",\"description\":\"test description\"}}";

        var actual = JsonConvert.SerializeObject(accessTokenError, jsonSerializerSettings);

        actual.Should().Be(expected);
    }
}