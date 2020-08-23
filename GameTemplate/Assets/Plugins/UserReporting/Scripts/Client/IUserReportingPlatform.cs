using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
    /// <summary>
    /// Represents a user reporting platform.
    /// </summary>
    public interface IUserReportingPlatform
    {
        #region Methods

        /// <summary>
        /// Deserialized the specified JSON.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="json">The JSON.</param>
        /// <returns>The deserialized object instance.</returns>
        T DeserializeJson<T>(string json);

        /// <summary>
        /// Gets device metadata.
        /// </summary>
        /// <returns>Device metadata.</returns>
        IDictionary<string, string> GetDeviceMetadata();

        /// <summary>
        /// Modifies a user report.
        /// </summary>
        /// <param name="userReport">The user report.</param>
        void ModifyUserReport(UserReport userReport);

        /// <summary>
        /// Called at the end of a frame.
        /// </summary>
        /// <param name="client">The client.</param>
        void OnEndOfFrame(UserReportingClient client);

        /// <summary>
        /// Posts to an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="content">The content.</param>
        /// <param name="progressCallback">The progress callback. Provides the upload and download progress.</param>
        /// <param name="callback">The callback. Provides a value indicating whether the post was successful and provides the resulting byte array.</param>
        void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback);

        /// <summary>
        /// Runs a task asynchronously.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="callback">The callback.</param>
        void RunTask(Func<object> task, Action<object> callback);

        /// <summary>
        /// Sends an analytics event.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="eventData">The event data.</param>
        void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData);

        /// <summary>
        /// Serializes the specified object instance.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <returns>The JSON.</returns>
        string SerializeJson(object instance);

        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        /// <param name="frameNumber">The frame number.</param>
        /// <param name="maximumWidth">The maximum width.</param>
        /// <param name="maximumHeight">The maximum height.</param>
        /// <param name="source">The source. Passing null will capture the screen. Passing a camera will capture the camera's view. Passing a render texture will capture the render texture.</param>
        /// <param name="callback">The callback. Provides the screenshot.</param>
        void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback);

        /// <summary>
        /// Called on update.
        /// </summary>
        /// <param name="client">The client.</param>
        void Update(UserReportingClient client);

        #endregion
    }
}