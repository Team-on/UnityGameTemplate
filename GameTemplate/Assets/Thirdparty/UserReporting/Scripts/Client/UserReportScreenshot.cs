namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report screenshot.
    /// </summary>
    public struct UserReportScreenshot
    {
        #region Properties

        /// <summary>
        /// Gets or sets the data (base 64 encoded). Screenshots must be in PNG format.
        /// </summary>
        public string DataBase64 { get; set; }

        /// <summary>
        /// Gets or sets the data identifier. This property will be overwritten by the server if provided.
        /// </summary>
        public string DataIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the frame number.
        /// </summary>
        public int FrameNumber { get; set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height
        {
            get { return PngHelper.GetPngHeightFromBase64Data(this.DataBase64); }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width
        {
            get { return PngHelper.GetPngWidthFromBase64Data(this.DataBase64); }
        }

        #endregion
    }
}