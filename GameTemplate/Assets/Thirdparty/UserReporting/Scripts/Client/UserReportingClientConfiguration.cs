namespace Unity.Cloud.UserReporting.Client
{
    /// <summary>
    /// Represents configuration for the user reporting client.
    /// </summary>
    public class UserReportingClientConfiguration
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportingClientConfiguration"/> class.
        /// </summary>
        public UserReportingClientConfiguration()
        {
            this.MaximumEventCount = 100;
            this.MaximumMeasureCount = 300;
            this.FramesPerMeasure = 60;
            this.MaximumScreenshotCount = 10;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportingClientConfiguration"/> class.
        /// </summary>
        /// <param name="maximumEventCount">The maximum event count. This is a rolling window.</param>
        /// <param name="maximumMeasureCount">The maximum measure count. This is a rolling window.</param>
        /// <param name="framesPerMeasure">The number of frames per measure. A user report is only created on the boundary between measures. A large number of frames per measure will increase user report creation time by this number of frames in the worst case.</param>
        /// <param name="maximumScreenshotCount">The maximum screenshot count. This is a rolling window.</param>
        public UserReportingClientConfiguration(int maximumEventCount, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
        {
            this.MaximumEventCount = maximumEventCount;
            this.MaximumMeasureCount = maximumMeasureCount;
            this.FramesPerMeasure = framesPerMeasure;
            this.MaximumScreenshotCount = maximumScreenshotCount;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportingClientConfiguration"/> class.
        /// </summary>
        /// <param name="maximumEventCount">The maximum event count. This is a rolling window.</param>
        /// <param name="metricsGatheringMode">The metrics gathering mode.</param>
        /// <param name="maximumMeasureCount">The maximum measure count. This is a rolling window.</param>
        /// <param name="framesPerMeasure">The number of frames per measure. A user report is only created on the boundary between measures. A large number of frames per measure will increase user report creation time by this number of frames in the worst case.</param>
        /// <param name="maximumScreenshotCount">The maximum screenshot count. This is a rolling window.</param>
        public UserReportingClientConfiguration(int maximumEventCount, MetricsGatheringMode metricsGatheringMode, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
        {
            this.MaximumEventCount = maximumEventCount;
            this.MetricsGatheringMode = metricsGatheringMode;
            this.MaximumMeasureCount = maximumMeasureCount;
            this.FramesPerMeasure = framesPerMeasure;
            this.MaximumScreenshotCount = maximumScreenshotCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the frames per measure.
        /// </summary>
        public int FramesPerMeasure { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum event count.
        /// </summary>
        public int MaximumEventCount { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum measure count.
        /// </summary>
        public int MaximumMeasureCount { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum screenshot count.
        /// </summary>
        public int MaximumScreenshotCount { get; internal set; }
        
        /// <summary>
        /// Gets or sets the metrics gathering mode.
        /// </summary>
        public MetricsGatheringMode MetricsGatheringMode { get; internal set; }

        #endregion
    }
}