using Unity.Cloud.UserReporting.Client;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
    /// <summary>
    /// Provides a starting point for Unity User Reporting.
    /// </summary>
    public static class UnityUserReporting
    {
        #region Static Fields

        private static UserReportingClient currentClient;

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the current client.
        /// </summary>
        public static UserReportingClient CurrentClient
        {
            get
            {
                if (UnityUserReporting.currentClient == null)
                {
                    UnityUserReporting.Configure();
                }
                return UnityUserReporting.currentClient;
            }
            private set { UnityUserReporting.currentClient = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="projectIdentifier">The project identifier.</param>
        /// <param name="platform">The plaform.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(string endpoint, string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, platform, configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="projectIdentifier"></param>
        /// <param name="configuration"></param>
        public static void Configure(string endpoint, string projectIdentifier, UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="projectIdentifier"></param>
        /// <param name="configuration"></param>
        public static void Configure(string projectIdentifier, UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="projectIdentifier"></param>
        public static void Configure(string projectIdentifier)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        public static void Configure()
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="configuration"></param>
        public static void Configure(UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="projectIdentifier"></param>
        /// <param name="platform"></param>
        /// <param name="configuration"></param>
        public static void Configure(string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, platform, configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="configuration"></param>
        public static void Configure(IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, configuration);
        }

        /// <summary>
        /// Configures Unity User Reporting.
        /// </summary>
        /// <param name="platform"></param>
        public static void Configure(IUserReportingPlatform platform)
        {
            UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, new UserReportingClientConfiguration());
        }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        /// <returns>The platform.</returns>
        private static IUserReportingPlatform GetPlatform()
        {
            return new UnityUserReportingPlatform();
        }

        /// <summary>
        /// Uses an existing client.
        /// </summary>
        /// <param name="client">The client.</param>
        public static void Use(UserReportingClient client)
        {
            if (client != null)
            {
                UnityUserReporting.CurrentClient = client;
            }
        }

        #endregion
    }
}