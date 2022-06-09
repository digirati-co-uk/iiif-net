using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentAssertions;
using IIIF.Presentation;
using Xunit;

namespace IIIF.Tests.Presentation
{
    public class VersionTests
    {
        [Theory]
        [InlineData(Version.Unknown, "Unknown")]
        [InlineData(Version.V2, "http://iiif.io/api/presentation/2/context.json")]
        [InlineData(Version.V3, "http://iiif.io/api/presentation/3/context.json")]
        public void VersionsHaveCorrectDescriptions(Version version, string context)
        {
            var fieldInfo = typeof(Version).GetField(version.ToString());
            var actual = fieldInfo!
                .GetCustomAttribute<DisplayAttribute>()!
                .Description;

            actual.Should().Be(context);
        }
    }
}