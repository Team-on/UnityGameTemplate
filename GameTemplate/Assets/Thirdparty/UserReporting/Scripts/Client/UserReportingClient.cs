using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting.Client
{
    /// <summary>
    /// Represents a user reporting client.
    /// </summary>
    public class UserReportingClient
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UserReportingClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="projectIdentifier">The project identifier.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="configuration">The configuration.</param>
        public UserReportingClient(string endpoint, string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
        {
            // Arguments
            this.Endpoint = endpoint;
            this.ProjectIdentifier = projectIdentifier;
            this.Platform = platform;
            this.Configuration = configuration;

            // Configuration Clean Up
            this.Configuration.FramesPerMeasure = this.Configuration.FramesPerMeasure > 0 ? this.Configuration.FramesPerMeasure : 1;
            this.Configuration.MaximumEventCount = this.Configuration.MaximumEventCount > 0 ? this.Configuration.MaximumEventCount : 1;
            this.Configuration.MaximumMeasureCount = this.Configuration.MaximumMeasureCount > 0 ? this.Configuration.MaximumMeasureCount : 1;
            this.Configuration.MaximumScreenshotCount = this.Configuration.MaximumScreenshotCount > 0 ? this.Configuration.MaximumScreenshotCount : 1;

            // Lists
            this.clientMetrics = new Dictionary<string, UserReportMetric>();
            this.currentMeasureMetadata = new Dictionary<string, string>();
            this.currentMetrics = new Dictionary<string, UserReportMetric>();
            this.events = new CyclicalList<UserReportEvent>(configuration.MaximumEventCount);
            this.measures = new CyclicalList<UserReportMeasure>(configuration.MaximumMeasureCount);
            this.screenshots = new CyclicalList<UserReportScreenshot>(configuration.MaximumScreenshotCount);

            // Device Metadata
            this.deviceMetadata = new List<UserReportNamedValue>();
            foreach (KeyValuePair<string, string> kvp in this.Platform.GetDeviceMetadata())
            {
                this.AddDeviceMetadata(kvp.Key, kvp.Value);
            }

            // Client Version
            this.AddDeviceMetadata("UserReportingClientVersion", "2.0");

            // Synchronized Action
            this.synchronizedActions = new List<Action>();
            this.currentSynchronizedActions = new List<Action>();

            // Update Stopwatch
            this.updateStopwatch = new Stopwatch();

            // Is Connected to Logger
            this.IsConnectedToLogger = true;
        }

        #endregion

        #region Fields

        private Dictionary<string, UserReportMetric> clientMetrics;

        private Dictionary<string, string> currentMeasureMetadata;

        private Dictionary<string, UserReportMetric> currentMetrics;

        private List<Action> currentSynchronizedActions;

        private List<UserReportNamedValue> deviceMetadata;

        private CyclicalList<UserReportEvent> events;

        private int frameNumber;

        private bool isMeasureBoundary;

        private int measureFrames;

        private CyclicalList<UserReportMeasure> measures;

        private CyclicalList<UserReportScreenshot> screenshots;

        private int screenshotsSaved;

        private int screenshotsTaken;

        private List<Action> synchronizedActions;

        private Stopwatch updateStopwatch;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public UserReportingClientConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        public string Endpoint { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client is connected to the logger. If true, log messages will be included in user reports.
        /// </summary>
        public bool IsConnectedToLogger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client is self reporting. If true, event and metrics about the client will be included in user reports.
        /// </summary>
        public bool IsSelfReporting { get; set; }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        public IUserReportingPlatform Platform { get; private set; }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        public string ProjectIdentifier { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether user reporting events should be sent to analytics.
        /// </summary>
        public bool SendEventsToAnalytics { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds device metadata.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddDeviceMetadata(string name, string value)
        {
            lock (this.deviceMetadata)
            {
                UserReportNamedValue userReportNamedValue = new UserReportNamedValue();
                userReportNamedValue.Name = name;
                userReportNamedValue.Value = value;
                this.deviceMetadata.Add(userReportNamedValue);
            }
        }

        /// <summary>
        /// Adds measure metadata. Measure metadata is associated with a period of time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddMeasureMetadata(string name, string value)
        {
            if (this.currentMeasureMetadata.ContainsKey(name))
            {
                this.currentMeasureMetadata[name] = value;
            }
            else
            {
                this.currentMeasureMetadata.Add(name, value);
            }
        }

        /// <summary>
        /// Clears the screenshots.
        /// </summary>
        public void ClearScreenshots()
        {
            lock (this.screenshots)
            {
                this.screenshots.Clear();
            }
        }

        /// <summary>
        /// Creates a user report.
        /// </summary>
        /// <param name="callback">The callback. Provides the user report that was created.</param>
        public void CreateUserReport(Action<UserReport> callback)
        {
            this.LogEvent(UserReportEventLevel.Info, "Creating user report.");
            this.WaitForPerforation(this.screenshotsTaken, () =>
            {
                this.Platform.RunTask(() =>
                {
                    // Start Stopwatch
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    // Copy Data
                    UserReport userReport = new UserReport();
                    userReport.ProjectIdentifier = this.ProjectIdentifier;

                    // Device Metadata
                    lock (this.deviceMetadata)
                    {
                        userReport.DeviceMetadata = this.deviceMetadata.ToList();
                    }

                    // Events
                    lock (this.events)
                    {
                        userReport.Events = this.events.ToList();
                    }

                    // Measures
                    lock (this.measures)
                    {
                        userReport.Measures = this.measures.ToList();
                    }

                    // Screenshots
                    lock (this.screenshots)
                    {
                        userReport.Screenshots = this.screenshots.ToList();
                    }

                    // Complete
                    userReport.Complete();

                    // Modify
                    this.Platform.ModifyUserReport(userReport);

                    // Stop Stopwatch
                    stopwatch.Stop();

                    // Sample Client Metric
                    this.SampleClientMetric("UserReportingClient.CreateUserReport.Task", stopwatch.ElapsedMilliseconds);

                    // Copy Client Metrics
                    foreach (KeyValuePair<string, UserReportMetric> kvp in this.clientMetrics)
                    {
                        userReport.ClientMetrics.Add(kvp.Value);
                    }

                    // Return
                    return userReport;
                }, (result) =>
                {
                    callback(result as UserReport);
                });
            });
        }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        /// <returns>The endpoint.</returns>
        public string GetEndpoint()
        {
            if (this.Endpoint == null)
            {
                return "https://localhost";
            }

            return this.Endpoint.Trim();
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public void LogEvent(UserReportEventLevel level, string message)
        {
            this.LogEvent(level, message, null, null);
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        public void LogEvent(UserReportEventLevel level, string message, string stackTrace)
        {
            this.LogEvent(level, message, stackTrace, null);
        }

        /// <summary>
        /// Logs an event with a stack trace and exception.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="exception">The exception.</param>
        private void LogEvent(UserReportEventLevel level, string message, string stackTrace, Exception exception)
        {
            lock (this.events)
            {
                UserReportEvent userReportEvent = new UserReportEvent();
                userReportEvent.Level = level;
                userReportEvent.Message = message;
                userReportEvent.FrameNumber = this.frameNumber;
                userReportEvent.StackTrace = stackTrace;
                userReportEvent.Timestamp = DateTime.UtcNow;
                if (exception != null)
                {
                    userReportEvent.Exception = new SerializableException(exception);
                }
                this.events.Add(userReportEvent);
            }
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogException(Exception exception)
        {
            this.LogEvent(UserReportEventLevel.Error, null, null, exception);
        }

        /// <summary>
        /// Samples a client metric. These metrics are only sample when self reporting is enabled.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SampleClientMetric(string name, double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return;
            }

            if (!this.clientMetrics.ContainsKey(name))
            {
                UserReportMetric newUserReportMetric = new UserReportMetric();
                newUserReportMetric.Name = name;
                this.clientMetrics.Add(name, newUserReportMetric);
            }

            UserReportMetric userReportMetric = this.clientMetrics[name];
            userReportMetric.Sample(value);
            this.clientMetrics[name] = userReportMetric;

            // Self Reporting
            if (this.IsSelfReporting)
            {
                this.SampleMetric(name, value);
            }
        }

        /// <summary>
        /// Samples a metric. Metrics can be sampled frequently and have low overhead.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SampleMetric(string name, double value)
        {
            if (this.Configuration.MetricsGatheringMode == MetricsGatheringMode.Disabled)
            {
                return;
            }
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return;
            }

            if (!this.currentMetrics.ContainsKey(name))
            {
                UserReportMetric newUserReportMetric = new UserReportMetric();
                newUserReportMetric.Name = name;
                this.currentMetrics.Add(name, newUserReportMetric);
            }

            UserReportMetric userReportMetric = this.currentMetrics[name];
            userReportMetric.Sample(value);
            this.currentMetrics[name] = userReportMetric;
        }

        /// <summary>
        /// Saves a user report to disk.
        /// </summary>
        /// <param name="userReport">The user report.</param>
        public void SaveUserReportToDisk(UserReport userReport)
        {
            this.LogEvent(UserReportEventLevel.Info, "Saving user report to disk.");
            string json = this.Platform.SerializeJson(userReport);
            File.WriteAllText("UserReport.json", json);
        }

        /// <summary>
        /// Sends a user report to the server.
        /// </summary>
        /// <param name="userReport">The user report.</param>
        /// <param name="callback">The callback. Provides a value indicating whether sending the user report was successful and provides the user report after it is modified by the server.</param>
        public void SendUserReport(UserReport userReport, Action<bool, UserReport> callback)
        {
            this.SendUserReport(userReport, null, callback);
        }

        /// <summary>
        /// Sends a user report to the server.
        /// </summary>
        /// <param name="userReport">The user report.</param>
        /// <param name="progressCallback">The progress callback. Provides the upload and download progress.</param>
        /// <param name="callback">The callback. Provides a value indicating whether sending the user report was successful and provides the user report after it is modified by the server.</param>
        public void SendUserReport(UserReport userReport, Action<float, float> progressCallback, Action<bool, UserReport> callback)
        {
            try
            {
                if (userReport == null)
                {
                    return;
                }
                if (userReport.Identifier != null)
                {
                    this.LogEvent(UserReportEventLevel.Warning, "Identifier cannot be set on the client side. The value provided was discarded.");
                    return;
                }
                if (userReport.ContentLength != 0)
                {
                    this.LogEvent(UserReportEventLevel.Warning, "ContentLength cannot be set on the client side. The value provided was discarded.");
                    return;
                }
                if (userReport.ReceivedOn != default(DateTime))
                {
                    this.LogEvent(UserReportEventLevel.Warning, "ReceivedOn cannot be set on the client side. The value provided was discarded.");
                    return;
                }
                if (userReport.ExpiresOn != default(DateTime))
                {
                    this.LogEvent(UserReportEventLevel.Warning, "ExpiresOn cannot be set on the client side. The value provided was discarded.");
                    return;
                }
                this.LogEvent(UserReportEventLevel.Info, "Sending user report.");
                string json = this.Platform.SerializeJson(userReport);
                byte[] jsonData = Encoding.UTF8.GetBytes(json);
                string endpoint = this.GetEndpoint();
                string url = string.Format(string.Format("{0}/api/userreporting", endpoint));
                this.Platform.Post(url, "application/json", jsonData, (uploadProgress, downloadProgress) =>
                {
                    if (progressCallback != null)
                    {
                        progressCallback(uploadProgress, downloadProgress);
                    }
                },
                (success, result) =>
                {
                    this.synchronizedActions.Add(() =>
                    {
                        if (success)
                        {
                            try
                            {
                                string jsonResult = Encoding.UTF8.GetString(result);
                                UserReport userReportResult = this.Platform.DeserializeJson<UserReport>(jsonResult);
                                if (userReportResult != null)
                                {
                                    if (this.SendEventsToAnalytics)
                                    {
                                        Dictionary<string, object> eventData = new Dictionary<string, object>();
                                        eventData.Add("UserReportIdentifier", userReport.Identifier);
                                        this.Platform.SendAnalyticsEvent("UserReportingClient.SendUserReport", eventData);
                                    }

                                    callback(success, userReportResult);
                                }
                                else
                                {
                                    callback(false, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.LogEvent(UserReportEventLevel.Error, string.Format("Sending user report failed: {0}", ex.ToString()));
                                callback(false, null);
                            }
                        }
                        else
                        {
                            this.LogEvent(UserReportEventLevel.Error, "Sending user report failed.");
                            callback(false, null);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                this.LogEvent(UserReportEventLevel.Error, string.Format("Sending user report failed: {0}", ex.ToString()));
                callback(false, null);
            }
        }

        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        /// <param name="maximumWidth">The maximum width.</param>
        /// <param name="maximumHeight">The maximum height.</param>
        /// <param name="callback">The callback. Provides the screenshot.</param>
        public void TakeScreenshot(int maximumWidth, int maximumHeight, Action<UserReportScreenshot> callback)
        {
            this.TakeScreenshotFromSource(maximumWidth, maximumHeight, null, callback);
        }

        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        /// <param name="maximumWidth">The maximum width.</param>
        /// <param name="maximumHeight">The maximum height.</param>
        /// <param name="source">The source. Passing null will capture the screen. Passing a camera will capture the camera's view. Passing a render texture will capture the render texture.</param>
        /// <param name="callback">The callback. Provides the screenshot.</param>
        public void TakeScreenshotFromSource(int maximumWidth, int maximumHeight, object source, Action<UserReportScreenshot> callback)
        {
            this.LogEvent(UserReportEventLevel.Info, "Taking screenshot.");
            this.screenshotsTaken++;
            this.Platform.TakeScreenshot(this.frameNumber, maximumWidth, maximumHeight, source, (passedFrameNumber, data) =>
            {
                this.synchronizedActions.Add(() =>
                {
                    lock (this.screenshots)
                    {
                        UserReportScreenshot userReportScreenshot = new UserReportScreenshot();
                        userReportScreenshot.FrameNumber = passedFrameNumber;
                        userReportScreenshot.DataBase64 = Convert.ToBase64String(data);
                        this.screenshots.Add(userReportScreenshot);
                        this.screenshotsSaved++;
                        callback(userReportScreenshot);
                    }
                });
            });
        }

        /// <summary>
        /// Updates the user reporting client, which updates networking communication, screenshotting, and metrics gathering.
        /// </summary>
        public void Update()
        {
            // Stopwatch
            this.updateStopwatch.Reset();
            this.updateStopwatch.Start();

            // Update Platform
            this.Platform.Update(this);

            // Measures
            if (this.Configuration.MetricsGatheringMode != MetricsGatheringMode.Disabled)
            {
                this.isMeasureBoundary = false;
                int framesPerMeasure = this.Configuration.FramesPerMeasure;
                if (this.measureFrames >= framesPerMeasure)
                {
                    lock (this.measures)
                    {
                        UserReportMeasure userReportMeasure = new UserReportMeasure();
                        userReportMeasure.StartFrameNumber = this.frameNumber - framesPerMeasure;
                        userReportMeasure.EndFrameNumber = this.frameNumber - 1;
                        UserReportMeasure evictedUserReportMeasure = this.measures.GetNextEviction();
                        if (evictedUserReportMeasure.Metrics != null)
                        {
                            userReportMeasure.Metadata = evictedUserReportMeasure.Metadata;
                            userReportMeasure.Metrics = evictedUserReportMeasure.Metrics;
                        }
                        else
                        {
                            userReportMeasure.Metadata = new List<UserReportNamedValue>();
                            userReportMeasure.Metrics = new List<UserReportMetric>();
                        }

                        userReportMeasure.Metadata.Clear();
                        userReportMeasure.Metrics.Clear();
                        foreach (KeyValuePair<string, string> kvp in this.currentMeasureMetadata)
                        {
                            UserReportNamedValue userReportNamedValue = new UserReportNamedValue();
                            userReportNamedValue.Name = kvp.Key;
                            userReportNamedValue.Value = kvp.Value;
                            userReportMeasure.Metadata.Add(userReportNamedValue);
                        }

                        foreach (KeyValuePair<string, UserReportMetric> kvp in this.currentMetrics)
                        {
                            userReportMeasure.Metrics.Add(kvp.Value);
                        }

                        this.currentMetrics.Clear();
                        this.measures.Add(userReportMeasure);
                        this.measureFrames = 0;
                        this.isMeasureBoundary = true;
                    }
                }

                this.measureFrames++;
            }
            else
            {
                this.isMeasureBoundary = true;
            }

            // Synchronization
            foreach (Action synchronizedAction in this.synchronizedActions)
            {
                this.currentSynchronizedActions.Add(synchronizedAction);
            }

            this.synchronizedActions.Clear();
            foreach (Action synchronizedAction in this.currentSynchronizedActions)
            {
                synchronizedAction();
            }

            this.currentSynchronizedActions.Clear();

            // Frame Number
            this.frameNumber++;

            // Stopwatch
            this.updateStopwatch.Stop();
            this.SampleClientMetric("UserReportingClient.Update", this.updateStopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Updates the user reporting client at the end of the frame, which updates networking communication, screenshotting, and metrics gathering.
        /// </summary>
        public void UpdateOnEndOfFrame()
        {
            // Stopwatch
            this.updateStopwatch.Reset();
            this.updateStopwatch.Start();

            // Update Platform
            this.Platform.OnEndOfFrame(this);

            // Stopwatch
            this.updateStopwatch.Stop();
            this.SampleClientMetric("UserReportingClient.UpdateOnEndOfFrame", this.updateStopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Waits for perforation, a boundary between measures when no screenshots are in progress.
        /// </summary>
        /// <param name="currentScreenshotsTaken">The current screenshots taken.</param>
        /// <param name="callback">The callback.</param>
        private void WaitForPerforation(int currentScreenshotsTaken, Action callback)
        {
            if (this.screenshotsSaved >= currentScreenshotsTaken && this.isMeasureBoundary)
            {
                callback();
            }
            else
            {
                this.synchronizedActions.Add(() => { this.WaitForPerforation(currentScreenshotsTaken, callback); });
            }
        }

        #endregion
    }
}