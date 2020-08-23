using System;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report attachment.
    /// </summary>
    public struct UserReportAttachment
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportAttachment"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="data">The data.</param>
        public UserReportAttachment(string name, string fileName, string contentType, byte[] data)
        {
            this.Name = name;
            this.FileName = fileName;
            this.ContentType = contentType;
            this.DataBase64 = Convert.ToBase64String(data);
            this.DataIdentifier = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data (base 64 encoded).
        /// </summary>
        public string DataBase64 { get; set; }

        /// <summary>
        /// Gets or sets the data identifier. This property will be overwritten by the server if provided.
        /// </summary>
        public string DataIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}