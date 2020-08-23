namespace Unity.Cloud.UserReporting
{
    /// <summary>
    /// Represents a user report named value.
    /// </summary>
    public struct UserReportNamedValue
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportNamedValue"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public UserReportNamedValue(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}