using System;
using System.Collections.Generic;
using FluentAssertions;
using IIIF.Presentation.V2;
using IIIF.Presentation.V2.Strings;
using Xunit;

namespace IIIF.Tests.Presentation.V2
{
    public class MetadataXTests
    {
        [Fact]
        public void GetValueByLabel_Null_IfMetadataNull()
        {
            // Arrange
            List<Metadata> metadata = null;
            
            // Act
            var actual = metadata.GetValueByLabel("foo");
            
            // Assert
            actual.Should().BeNull();
        }
        
        [Fact]
        public void GetValueByLabel_Null_IfMetadataEmpty()
        {
            // Arrange
            List<Metadata> metadata = new();
            
            // Act
            var actual = metadata.GetValueByLabel("foo");
            
            // Assert
            actual.Should().BeNull();
        }
        
        [Fact]
        public void GetValueByLabel_Null_IfMetadataLabelNotFound()
        {
            // Arrange
            var metadata = new List<Metadata>
            {
                new() {Label = new MetaDataValue("TestLabel"), Value = new MetaDataValue("TestValue")}
            };

            // Act
            var actual = metadata.GetValueByLabel("foo");
            
            // Assert
            actual.Should().BeNull();
        }
        
        [Fact]
        public void GetValueByLabel_ReturnsValue_IfFound()
        {
            // Arrange
            var metadata = new List<Metadata>
            {
                new() {Label = new MetaDataValue("TestLabel"), Value = new MetaDataValue("TestValue")}
            };

            // Act
            var actual = metadata.GetValueByLabel("TestLabel");
            
            // Assert
            actual.Should().Be("TestValue");
        }
        
        [Fact]
        public void GetValueByLabel_Throws_IfMultipleItemsWithLabel()
        {
            // Arrange
            var metadata = new List<Metadata>
            {
                new() {Label = new MetaDataValue("TestLabel"), Value = new MetaDataValue("TestValue")},
                new() {Label = new MetaDataValue("TestLabel"), Value = new MetaDataValue("Another Value")}
            };

            // Act
            Action action = () => metadata.GetValueByLabel("TestLabel");
            
            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Sequence contains more than one matching element");
        }
    }
}