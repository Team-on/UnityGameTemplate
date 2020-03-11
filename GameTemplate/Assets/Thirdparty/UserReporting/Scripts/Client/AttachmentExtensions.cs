using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Provides extensions for working with attachments.
    /// </summary>
    public static class AttachmentExtensions
    {
        #region Static Methods

        /// <summary>
        /// Adds a JSON attachment.
        /// </summary>
        /// <param name="instance">The extended instance.</param>
        /// <param name="name">The name of the attachment.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contents">The contents.</param>
        public static void AddJson(this List<UserReportAttachment> instance, string name, string fileName, string contents)
        {
            if (instance != null)
            {
                instance.Add(new UserReportAttachment(name, fileName, "application/json", System.Text.Encoding.UTF8.GetBytes(contents)));
            }
        }

        /// <summary>
        /// Adds a text attachment.
        /// </summary>
        /// <param name="instance">The extended instance.</param>
        /// <param name="name">The name of the attachment.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contents">The contents.</param>
        public static void AddText(this List<UserReportAttachment> instance, string name, string fileName, string contents)
        {
            if (instance != null)
            {
                instance.Add(new UserReportAttachment(name, fileName, "text/plain", System.Text.Encoding.UTF8.GetBytes(contents)));
            }
        }

        #endregion
    }
}