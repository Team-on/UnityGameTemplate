using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
    /// <summary>
    /// Represents a null user reporting platform.
    /// </summary>
    public class NullUserReportingPlatform : IUserReportingPlatform
    {
        #region Methods

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public T DeserializeJson<T>(string json)
        {
            return default(T);
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public IDictionary<string, string> GetDeviceMetadata()
        {
            return new Dictionary<string, string>();
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void ModifyUserReport(UserReport userReport)
        {
            // Empty
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void OnEndOfFrame(UserReportingClient client)
        {
            // Empty
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback)
        {
            progressCallback(1, 1);
            callback(true, content);
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void RunTask(Func<object> task, Action<object> callback)
        {
            callback(task());
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
        {
            // Empty
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public string SerializeJson(object instance)
        {
            return string.Empty;
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
        {
            callback(frameNumber, new byte[0]);
        }

        /// <inheritdoc cref="IUserReportingPlatform"/>
        public void Update(UserReportingClient client)
        {
            // Empty
        }

        #endregion
    }
}