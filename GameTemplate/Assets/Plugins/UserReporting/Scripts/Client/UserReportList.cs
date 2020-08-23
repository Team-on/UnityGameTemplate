using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report list.
    /// </summary>
    public class UserReportList
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportList"/> class.
        /// </summary>
        public UserReportList()
        {
            this.UserReportPreviews = new List<UserReportPreview>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the continuation token.
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list has more items.
        /// </summary>
        public bool HasMore { get; set; }

        /// <summary>
        /// Gets or sets the user report previews.
        /// </summary>
        public List<UserReportPreview> UserReportPreviews { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Completes the list. This only need to be called by the creator of the list.
        /// </summary>
        /// <param name="originalLimit">The original limit.</param>
        /// <param name="continuationToken">The continuation token.</param>
        public void Complete(int originalLimit, string continuationToken)
        {
            if (this.UserReportPreviews.Count > 0)
            {
                if (this.UserReportPreviews.Count > originalLimit)
                {
                    while (this.UserReportPreviews.Count > originalLimit)
                    {
                        this.UserReportPreviews.RemoveAt(this.UserReportPreviews.Count - 1);
                    }
                    this.ContinuationToken = continuationToken;
                    this.HasMore = true;
                }
            }
        }

        #endregion
    }
}