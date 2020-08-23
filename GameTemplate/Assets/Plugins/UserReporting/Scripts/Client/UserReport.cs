using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report.
    /// </summary>
    public class UserReport : UserReportPreview
    {
        #region Nested Types

        /// <summary>
        /// Provides sorting for metrics.
        /// </summary>
        private class UserReportMetricSorter : IComparer<UserReportMetric>
        {
            #region Methods

            /// <inheritdoc />
            public int Compare(UserReportMetric x, UserReportMetric y)
            {
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReport"/> class.
        /// </summary>
        public UserReport()
        {
            this.AggregateMetrics = new List<UserReportMetric>();
            this.Attachments = new List<UserReportAttachment>();
            this.ClientMetrics = new List<UserReportMetric>();
            this.DeviceMetadata = new List<UserReportNamedValue>();
            this.Events = new List<UserReportEvent>();
            this.Fields = new List<UserReportNamedValue>();
            this.Measures = new List<UserReportMeasure>();
            this.Screenshots = new List<UserReportScreenshot>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public List<UserReportAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the client metrics.
        /// </summary>
        public List<UserReportMetric> ClientMetrics { get; set; }

        /// <summary>
        /// Gets or sets the device metadata.
        /// </summary>
        public List<UserReportNamedValue> DeviceMetadata { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        public List<UserReportEvent> Events { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        public List<UserReportNamedValue> Fields { get; set; }

        /// <summary>
        /// Gets or sets the measures.
        /// </summary>
        public List<UserReportMeasure> Measures { get; set; }

        /// <summary>
        /// Gets or sets the screenshots.
        /// </summary>
        public List<UserReportScreenshot> Screenshots { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clones the user report.
        /// </summary>
        /// <returns>The cloned user report.</returns>
        public UserReport Clone()
        {
            UserReport userReport = new UserReport();
            userReport.AggregateMetrics = this.AggregateMetrics != null ? this.AggregateMetrics.ToList() : null;
            userReport.Attachments = this.Attachments != null ? this.Attachments.ToList() : null;
            userReport.ClientMetrics = this.ClientMetrics != null ? this.ClientMetrics.ToList() : null;
            userReport.ContentLength = this.ContentLength;
            userReport.DeviceMetadata = this.DeviceMetadata != null ? this.DeviceMetadata.ToList() : null;
            userReport.Dimensions = this.Dimensions.ToList();
            userReport.Events = this.Events != null ? this.Events.ToList() : null;
            userReport.ExpiresOn = this.ExpiresOn;
            userReport.Fields = this.Fields != null ? this.Fields.ToList() : null;
            userReport.Identifier = this.Identifier;
            userReport.IPAddress = this.IPAddress;
            userReport.Measures = this.Measures != null ? this.Measures.ToList() : null;
            userReport.ProjectIdentifier = this.ProjectIdentifier;
            userReport.ReceivedOn = this.ReceivedOn;
            userReport.Screenshots = this.Screenshots != null ? this.Screenshots.ToList() : null;
            userReport.Summary = this.Summary;
            userReport.Thumbnail = this.Thumbnail;
            return userReport;
        }

        /// <summary>
        /// Completes the user report. This is called by the client and only needs to be called when constructing a user report manually.
        /// </summary>
        public void Complete()
        {
            // Thumbnail
            if (this.Screenshots.Count > 0)
            {
                this.Thumbnail = this.Screenshots[this.Screenshots.Count - 1];
            }

            // Aggregate Metrics
            Dictionary<string, UserReportMetric> aggregateMetrics = new Dictionary<string, UserReportMetric>();
            foreach (UserReportMeasure measure in this.Measures)
            {
                foreach (UserReportMetric metric in measure.Metrics)
                {
                    if (!aggregateMetrics.ContainsKey(metric.Name))
                    {
                        UserReportMetric userReportMetric = new UserReportMetric();
                        userReportMetric.Name = metric.Name;
                        aggregateMetrics.Add(metric.Name, userReportMetric);
                    }
                    UserReportMetric aggregateMetric = aggregateMetrics[metric.Name];
                    aggregateMetric.Sample(metric.Average);
                    aggregateMetrics[metric.Name] = aggregateMetric;
                }
            }
            if (this.AggregateMetrics == null)
            {
                this.AggregateMetrics = new List<UserReportMetric>();
            }
            foreach (KeyValuePair<string, UserReportMetric> kvp in aggregateMetrics)
            {
                this.AggregateMetrics.Add(kvp.Value);
            }
            this.AggregateMetrics.Sort(new UserReportMetricSorter());
        }

        /// <summary>
        /// Fixes the user report by replace null lists with empty lists.
        /// </summary>
        public void Fix()
        {
            this.AggregateMetrics = this.AggregateMetrics ?? new List<UserReportMetric>();
            this.Attachments = this.Attachments ?? new List<UserReportAttachment>();
            this.ClientMetrics = this.ClientMetrics ?? new List<UserReportMetric>();
            this.DeviceMetadata = this.DeviceMetadata ?? new List<UserReportNamedValue>();
            this.Dimensions = this.Dimensions ?? new List<UserReportNamedValue>();
            this.Events = this.Events ?? new List<UserReportEvent>();
            this.Fields = this.Fields ?? new List<UserReportNamedValue>();
            this.Measures = this.Measures ?? new List<UserReportMeasure>();
            this.Screenshots = this.Screenshots ?? new List<UserReportScreenshot>();
        }

        /// <summary>
        /// Gets the dimension string for the dimensions associated with this user report.
        /// </summary>
        /// <returns></returns>
        public string GetDimensionsString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < this.Dimensions.Count; i++)
            {
                UserReportNamedValue dimension = this.Dimensions[i];
                stringBuilder.Append(dimension.Name);
                stringBuilder.Append(": ");
                stringBuilder.Append(dimension.Value);
                if (i != this.Dimensions.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Removes screenshots above a certain size from the user report.
        /// </summary>
        /// <param name="maximumWidth">The maximum width.</param>
        /// <param name="maximumHeight">The maximum height.</param>
        /// <param name="totalBytes">The total bytes allowed by screenshots.</param>
        /// <param name="ignoreCount">The number of screenshots to ignoreCount.</param>
        public void RemoveScreenshots(int maximumWidth, int maximumHeight, int totalBytes, int ignoreCount)
        {
            int byteCount = 0;
            for (int i = this.Screenshots.Count; i > 0; i--)
            {
                if (i < ignoreCount)
                {
                    continue;
                }
                UserReportScreenshot screenshot = this.Screenshots[i];
                byteCount += screenshot.DataBase64.Length;
                if (byteCount > totalBytes)
                {
                    break;
                }
                if (screenshot.Width > maximumWidth || screenshot.Height > maximumHeight)
                {
                    this.Screenshots.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Casts the user report to a user report preview.
        /// </summary>
        /// <returns>The user report preview.</returns>
        public UserReportPreview ToPreview()
        {
            UserReportPreview userReportPreview = new UserReportPreview();
            userReportPreview.AggregateMetrics = this.AggregateMetrics != null ? this.AggregateMetrics.ToList() : null;
            userReportPreview.ContentLength = this.ContentLength;
            userReportPreview.Dimensions = this.Dimensions != null ? this.Dimensions.ToList() : null;
            userReportPreview.ExpiresOn = this.ExpiresOn;
            userReportPreview.Identifier = this.Identifier;
            userReportPreview.IPAddress = this.IPAddress;
            userReportPreview.ProjectIdentifier = this.ProjectIdentifier;
            userReportPreview.ReceivedOn = this.ReceivedOn;
            userReportPreview.Summary = this.Summary;
            userReportPreview.Thumbnail = this.Thumbnail;
            return userReportPreview;
        }

        #endregion
    }
}