using System;
using System.Collections.Generic;
using Unity.Cloud.Authorization;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report preview or the fly weight version of a user report.
    /// </summary>
    public class UserReportPreview
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportPreview"/> class.
        /// </summary>
        public UserReportPreview()
        {
            this.Dimensions = new List<UserReportNamedValue>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the aggregate metrics.
        /// </summary>
        public List<UserReportMetric> AggregateMetrics { get; set; }

        /// <summary>
        /// Gets or sets the appearance hint.
        /// </summary>
        public UserReportAppearanceHint AppearanceHint { get; set; }

        /// <summary>
        /// Gets or sets the content length. This property will be overwritten by the server if provided.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the dimensions.
        /// </summary>
        public List<UserReportNamedValue> Dimensions { get; set; }

        /// <summary>
        /// Gets or sets the time at which the user report expires. This property will be overwritten by the server if provided.
        /// </summary>
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// Gets or sets the geo country.
        /// </summary>
        public string GeoCountry { get; set; }

        /// <summary>
        /// Gets or sets the identifier. This property will be overwritten by the server if provided.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the IP address. This property will be overwritten by the server if provided.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user report is hidden in the UI if a dimension filter is not specified. This is recommended for automated or high volume reports.
        /// </summary>
        public bool IsHiddenWithoutDimension { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user report is silent. Silent user reports do not send events to integrations. This is recommended for automated or high volume reports.
        /// </summary>
        public bool IsSilent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user report is temporary. Temporary user reports are short lived and not queryable.
        /// </summary>
        public bool IsTemporary { get; set; }

        /// <summary>
        /// Gets or sets the license level. This property will be overwritten by the server if provided.
        /// </summary>
        public LicenseLevel LicenseLevel { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        public string ProjectIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the time at which the user report was received. This property will be overwritten by the server if provided.
        /// </summary>
        public DateTime ReceivedOn { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail. This screenshot will be resized by the server if too large. Keep the last screenshot small in order to reduce report size and increase submission speed.
        /// </summary>
        public UserReportScreenshot Thumbnail { get; set; }

        #endregion
    }
}