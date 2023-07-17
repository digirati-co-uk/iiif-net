namespace IIIF.Presentation.V3.Constants;

public static class Motivation
{
    // IIIF-specific extension motivations

    /// <summary>
    /// The content can be thought of as being *of* the Canvas.
    /// For example, an image of a book page, that is expected to be rendered to a user.
    /// </summary>
    public const string Painting = "painting";

    /// <summary>
    /// The content can be thought of as being *from* the Canvas.
    /// For example, a textual transcription of a line or a page.
    /// </summary>
    public const string Supplementing = "supplementing";

    // W3C Annotation motivations

    /// <summary>
    /// The motivation for when the user intends to assess the target resource in some way, rather than
    /// simply make a comment about it. For example to write a review or assessment of a book, assess the
    /// quality of a dataset, or provide an assessment of a student's work.
    /// </summary>
    public const string Assessing = "assessing";

    /// <summary>
    /// The motivation for when the user intends to create a bookmark to the Target or part thereof.
    /// For example an Annotation that bookmarks the point in a text where the reader finished reading.
    /// </summary>
    public const string Bookmarking = "bookmarking";

    /// <summary>
    /// The motivation for when the user intends to classify the Target as something.
    /// For example to classify an image as a portrait.
    /// </summary>
    public const string Classifying = "classifying";

    /// <summary>
    /// The motivation for when the user intends to comment about the Target.
    /// For example to provide a commentary about a particular PDF document.
    /// </summary>
    public const string Commenting = "commenting";

    /// <summary>
    /// The motivation for when the user intends to describe the Target,
    /// as opposed to (for example) a comment about it.
    /// For example describing the above PDF's contents, rather than commenting on their accuracy.
    /// </summary>
    public const string Describing = "describing";

    /// <summary>
    /// The motivation for when the user intends to request a change or edit to the Target resource.
    /// For example an Annotation that requests a typo to be corrected.
    /// </summary>
    public const string Editing = "editing";

    /// <summary>
    /// The motivation for when the user intends to highlight the Target resource or segment of it.
    /// For example to draw attention to the selected text that the annotator disagrees with.
    /// </summary>
    public const string Highlighting = "highlighting";

    /// <summary>
    /// The motivation for when the user intends to assign an identity to the Target.
    /// For example to associate the IRI that identifies a city with a mention of the city in a web page.
    /// </summary>
    public const string Identifying = "identifying";

    /// <summary>
    /// The motivation for when the user intends to link to a resource related to the Target.
    /// </summary>
    public const string Linking = "linking";

    /// <summary>
    /// The motivation for when the user intends to assign some value or quality to the Target.
    /// For example annotating an Annotation to moderate it up in a trust network or threaded discussion.
    /// </summary>
    public const string Moderating = "moderating";

    /// <summary>
    /// The motivation for when the user intends to ask a question about the Target.
    /// For example to ask for assistance with a particular section of text, or question its veracity.
    /// </summary>
    public const string Questioning = "questioning";

    /// <summary>
    /// The motivation for when the user intends to reply to a previous statement,
    /// either an Annotation or another resource. For example providing the assistance requested in the above.
    /// </summary>
    public const string Replying = "replying";

    /// <summary>
    /// The motivation for when the user intends to associate a tag with the Target.
    /// </summary>
    public const string Tagging = "tagging";
}