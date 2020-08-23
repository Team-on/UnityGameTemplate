using System;

namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report event.
    /// </summary>
    public struct UserReportEvent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public SerializableException Exception { get; set; }

        /// <summary>
        /// Gets or sets the frame number.
        /// </summary>
        public int FrameNumber { get; set; }

        /// <summary>
        /// Gets or sets the full message.
        /// </summary>
        public string FullMessage
        {
            get { return string.Format("{0}{1}{2}", this.Message, Environment.NewLine, this.StackTrace); }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public UserReportEventLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        #endregion
    }
}