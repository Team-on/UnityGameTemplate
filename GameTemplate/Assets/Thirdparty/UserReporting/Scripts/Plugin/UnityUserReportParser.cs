namespace Unity.Cloud.UserReporting.Plugin
{
    /// <summary>
    /// Provides static methods for parsing user reports.
    /// </summary>
    public static class UnityUserReportParser
    {
        #region Static Methods

        /// <summary>
        /// Parses a user report.
        /// </summary>
        /// <param name="json">The JSON.</param>
        /// <returns>The user report.</returns>
        public static UserReport ParseUserReport(string json)
        {
            return SimpleJson.SimpleJson.DeserializeObject<UserReport>(json);
        }

        /// <summary>
        /// Parses a user report list.
        /// </summary>
        /// <param name="json">The JSON.</param>
        /// <returns>The user report list.</returns>
        public static UserReportList ParseUserReportList(string json)
        {
            return SimpleJson.SimpleJson.DeserializeObject<UserReportList>(json);
        }

        #endregion
    }
}