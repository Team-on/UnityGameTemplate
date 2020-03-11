using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report measure.
    /// </summary>
    public struct UserReportMeasure
    {
        #region Properties

        /// <summary>
        /// Gets or sets the end frame number.
        /// </summary>
        public int EndFrameNumber { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public List<UserReportNamedValue> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the metrics.
        /// </summary>
        public List<UserReportMetric> Metrics { get; set; }

        /// <summary>
        /// Gets or sets the start frame number.
        /// </summary>
        public int StartFrameNumber { get; set; }

        #endregion
    }
}