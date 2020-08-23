using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting.Client
{
    /// <summary>
    /// Represents a metrics gathering mode.
    /// </summary>
    public enum MetricsGatheringMode
    {
        /// <summary>
        /// Automatic. Some metrics are gathered automatically.
        /// </summary>
        Automatic = 0,

        /// <summary>
        /// Manual. No metrics are gathered automatically.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// Disabled. No metrics are gathered. Sampling a metric is a no-op.
        /// </summary>
        Disabled = 2
    }
}
