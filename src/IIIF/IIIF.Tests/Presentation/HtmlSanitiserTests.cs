using System;
using IIIF.Presentation;

namespace IIIF.Tests.Presentation;

public class HtmlSanitiserTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SanitiseHtml_ReturnsGivenString_IfNullOrEmpty(string val)
        => val.SanitiseHtml().Should().Be(val);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SanitiseHtml_ReturnsEmptyString_IfGivenInvalidHtml(bool ignoreNonHtml)
    {
        const string input = "<div>invalid html</div>";
        const string expected = "<span>invalid html</span>";

        var actual = input.SanitiseHtml(ignoreNonHtml);

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SanitiseHtml_Trims_Whitespace_FromBeginningAndEnd_IfIgnoreNonHtmlFalse()
    {
        const string input = " <p>valid html</p>  ";
        const string expected = "<p>valid html</p>";

        var actual = input.SanitiseHtml(false);

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_AutoCloses_ValidTags_IfIgnoreNonHtmlFalse()
    {
        const string input = " <p>valid html</p><span>  ";
        const string expected = "<p>valid html</p><span></span>";

        var actual = input.SanitiseHtml(false);

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_RemovesInvalidTags_RetainingChildElements()
    {
        const string input =
            "<br><span><small><i>hi</i></small></span><div><p>child paragraph</p></div><h1>Test</h1><ul><ol><li>foo</li></ol></ul><p><script>alert('hi');</script><sub>valid</sub> <sup>paragraph</sup></p>";
        const string expected =
            "<br><span><small><i>hi</i></small></span><p>child paragraph</p>Test foo<p>alert('hi');<sub>valid</sub> <sup>paragraph</sup></p>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_DoesNotAddSpaceForInlineElements()
    {
        const string input = "<strong>some</strong>thing<em>here</em><div>another</div><div>thing</div>";
        const string expected = "<span>somethinghere another thing</span>";
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_WillReplaceTagReplacementValue_WithSpace()
    {
        // This test highlights a quirk to be aware of: "~||~" is internal space identifier so will be lost
        const string input = "<p>This is the replacement for a tag: ~||~. It will be lost here</p>";
        const string expected = "<p>This is the replacement for a tag:  . It will be lost here</p>";

        var actual = input.SanitiseHtml();
        actual.Should().Be(expected);
    } 

    [Theory]
    [InlineData("http://localhost")]
    [InlineData("https://localhost")]
    [InlineData("mailto://test@example")]
    public void SanitiseHtml_AllowsSpecifiedSchemesForHref(string href)
    {
        var input = $"<a href=\"{href}\">test</a>";
        var expected = $"<a href=\"{href}\">test</a>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_StripsInvalidSchemesFromHref()
    {
        var input = "<a href=\"other://foo-bar\">test</a>";
        const string expected = "<a>test</a>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_RemovesComments()
    {
        const string input = "<!--This will be removed--><p><!--as will this-->valid html</p>";
        const string expected = "<p>valid html</p>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_RemovesCdata()
    {
        const string input = "<![CDATA[This will be removed]]><p>valid html</p>";
        const string expected = "<p>valid html</p>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Fact]
    public void SanitiseHtml_RemovesProcessingInstructions()
    {
        const string input = "<?xml version=\"1.0\"?><p>valid html</p>";
        const string expected = "<p>valid html</p>";

        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("<p>html")]
    [InlineData("<p>html</p")]
    [InlineData("p>html</p>")]
    [InlineData("html</p>")]
    [InlineData(" <p>valid html</p>  ")]
    public void SanitiseHtml_ReturnsInvalidHtml_IfIgnoreNonHtmlTrue(string input)
    {
        var actual = input.SanitiseHtml();

        actual.Should().Be(input);
    }
    
    [Theory]
    [InlineData("html", "<span>html</span>")]
    [InlineData(" html ", "<span>html</span>")]
    [InlineData("<p>html", "<p>html</p>")]
    [InlineData("<p>html</p", "<p>html</p>")]
    [InlineData("p>html</p>", "<span>p&gt;html<p></p></span>")]
    [InlineData("html</p>", "<span>html<p></p></span>")]
    public void SanitiseHtml_HandlesInvalidHtml_IfIgnoreNonHtmlFalse(string input, string expected)
    {
        var actual = input.SanitiseHtml(false);

        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("html", "<p>html</p>")]
    [InlineData(" html ", "<p>html</p>")]
    [InlineData("<p>html", "<p>html</p>")]
    [InlineData("<p>html</p", "<p>html</p>")]
    [InlineData("p>html</p>", "<p>p&gt;html</p><p></p><p></p>")]
    [InlineData("html</p>", "<p>html</p><p></p><p></p>")]
    public void SanitiseHtml_HandlesInvalidHtml_WithCustomWrapperTag(string input, string expected)
    {
        var actual = input.SanitiseHtml(false, "p");

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SanitiseHtml_Throws_IfCustomWrapperTagNotAllowed()
    {
        Action action = () => "html".SanitiseHtml(false, "div");

        action.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Tag provided is not allowed. Must be one of: a,b,br,i,img,p,small,span,sub,sup (Parameter 'nonHtmlWrappingTag')");
    }

    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("i")]
    [InlineData("p")]
    [InlineData("small")]
    [InlineData("span")]
    [InlineData("sub")]
    [InlineData("sup")]
    public void SanitiseHtml_RemovesSrcAndAltAttributes_FromNonImgTag(string tag)
    {
        var input = $"<{tag} alt=\"alt\" src=\"http://foo\">x</{tag}>";
        var expected = $"<{tag}>x</{tag}>";
        
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData("b")]
    [InlineData("i")]
    [InlineData("p")]
    [InlineData("small")]
    [InlineData("span")]
    [InlineData("sub")]
    [InlineData("sup")]
    public void SanitiseHtml_RemovesHrefAttribute_FromNonAnchorTag(string tag)
    {
        var input = $"<{tag} href=\"http://foo\">x</{tag}>";
        var expected = $"<{tag}>x</{tag}>";
        
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SanitiseHtml_RemovesHrefAttribute_FromImageTag()
    {
        // NOTE: this is excluded from above as it has no closing tag so avoids logic in tests
        const string input = "<img href=\"http://foo\">";
        const string expected = "<img>";
        
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SanitiseHtml_RemovesHref_Src_AndAltAttributes_FromLineBreak()
    {
        // NOTE: this is excluded from above as it has no closing tag so avoids logic in tests
        const string input = "<br alt=\"alt\" src=\"http://foo\" href=\"http://foo\">";
        const string expected = "<br>";
        
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SanitiseHtml_AllowsSrcAndAltAttributes_OnImgTag()
    {
        const string input = "<img alt=\"alt\" src=\"http://img.jpg\">";
        const string expected = "<img alt=\"alt\" src=\"http://img.jpg\">";
        
        var actual = input.SanitiseHtml();

        actual.Should().Be(expected);
    }
}