/// <summary>
/// Represents a user reporting state.
/// </summary>
public enum UserReportingState
{
    /// <summary>
    /// Idle.
    /// </summary>
    Idle = 0,

    /// <summary>
    /// Creating bug report.
    /// </summary>
    CreatingUserReport = 1,

    /// <summary>
    /// Showing form.
    /// </summary>
    ShowingForm = 2,

    /// <summary>
    /// Submitting form.
    /// </summary>
    SubmittingForm = 3
}