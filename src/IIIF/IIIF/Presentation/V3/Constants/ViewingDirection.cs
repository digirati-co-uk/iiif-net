namespace IIIF.Presentation.V3.Constants
{
    /// <summary>
    /// Constants representing the direction in which a set of Canvases should be displayed to the user.
    /// </summary>
    public static class ViewingDirection
    {
        /// <summary>
        /// The object is displayed from left to right. The default if not specified.
        /// </summary>
        public const string LeftToRight = "left-to-right";
        
        /// <summary>
        /// The object is displayed from right to left.
        /// </summary>
        public const string RightToLeft = "right-to-left";
        
        /// <summary>
        /// The object is displayed from the top to the bottom.
        /// </summary>
        public const string TopToBottom = "top-to-bottom";
        
        /// <summary>
        /// The object is displayed from the bottom to the top.
        /// </summary>
        public const string BottomToTop = "bottom-to-top";
    }
}
