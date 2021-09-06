using System;
using System.Linq;
using FluentAssertions;
using IIIF.Presentation.V2;
using IIIF.Presentation.V2.Serialisation;
using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Presentation.V2.Serialisation
{
    public class MetaDataValueSerialiserTests
    {
        private readonly MetaDataValueSerialiser sut;
        public MetaDataValueSerialiserTests()
        {
            sut = new MetaDataValueSerialiser();
        }
        
        [Fact]
        public void WriteJson_Throws_IfNoLanguageValues()
        {
            // Arrange
            var metadata = new MetaDataValue(Enumerable.Empty<LanguageValue>());
            
            // Act
            Action action = () => JsonConvert.SerializeObject(metadata, sut);
            
            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void WriteJson_WritesSingleValue_IfNoLanguage_SingleValue()
        {
            // Arrange
            var metadata = new MetaDataValue("Foo bar");
            var expected = "\"Foo bar\"";
            
            // Act
            var result = JsonConvert.SerializeObject(metadata, sut);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_WritesSingleValue_IfNoLanguage_MultipleValues()
        {
            // Arrange
            var metadata = new MetaDataValue("foo");
            metadata.LanguageValues.Add(new LanguageValue{Value = "bar"});
            var expected = "[\"foo\",\"bar\"]";
            
            // Act
            var result = JsonConvert.SerializeObject(metadata, sut);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_WritesLanguageAndValue_IfLanguage_SingleValue()
        {
            // Arrange
            var metadata = new MetaDataValue("Foo bar", "en");
            var expected = "{\"@value\":\"Foo bar\",\"@language\":\"en\"}";
            
            // Act
            var result = JsonConvert.SerializeObject(metadata, sut);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_WritesLanguageAndValue_IfLanguage_MultipleValues()
        {
            // Arrange
            var metadata = new MetaDataValue("foo", "en");
            metadata.LanguageValues.Add(new LanguageValue{Value = "bar", Language = "fr"});
            var expected = "[{\"@value\":\"foo\",\"@language\":\"en\"},{\"@value\":\"bar\",\"@language\":\"fr\"}]";
            
            // Act
            var result = JsonConvert.SerializeObject(metadata, sut);
            
            // Assert
            result.Should().Be(expected);
        }
    }
}