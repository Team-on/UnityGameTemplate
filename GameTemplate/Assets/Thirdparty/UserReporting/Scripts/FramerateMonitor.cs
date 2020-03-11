using UnityEngine;

/// <summary>
///  Represents a behavior that monitors the application for framerate issues and automatically submits a user report.
/// </summary>
public class FramerateMonitor : UserReportingMonitor
{
    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="FramerateMonitor"/> class.
    /// </summary>
    public FramerateMonitor()
    {
        this.MaximumDurationInSeconds = 10;
        this.MinimumFramerate = 15;
    }

    #endregion

    #region Fields

    private float duration;

    /// <summary>
    /// Gets or sets the maximum duration in seconds.
    /// </summary>
    public float MaximumDurationInSeconds;

    /// <summary>
    /// Gets or sets the minimum framerate.
    /// </summary>
    public float MinimumFramerate;

    #endregion

    #region Methods

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float framerate = 1.0f / deltaTime;
        if (framerate < this.MinimumFramerate)
        {
            this.duration += deltaTime;
        }
        else
        {
            this.duration = 0;
        }

        if (this.duration > this.MaximumDurationInSeconds)
        {
            this.duration = 0;
            this.Trigger();
        }
    }

    #endregion
}