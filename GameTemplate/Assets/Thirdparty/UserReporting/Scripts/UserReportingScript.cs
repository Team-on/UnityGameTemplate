using System;
using System.Collections;
using System.Reflection;
using System.Text;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a behavior for working with the user reporting client.
/// </summary>
/// <remarks>
/// This script is provided as a sample and isn't necessarily the most optimal solution for your project.
/// You may want to consider replacing with this script with your own script in the future.
/// </remarks>
public class UserReportingScript : MonoBehaviour
{
    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="UserReportingScript"/> class.
    /// </summary>
    public UserReportingScript()
    {
        this.UserReportSubmitting = new UnityEvent();
        this.unityUserReportingUpdater = new UnityUserReportingUpdater();
    }

    #endregion

    #region Fields

    /// <summary>
    /// Gets or sets the category dropdown.
    /// </summary>
    [Tooltip("The category dropdown.")] public Dropdown CategoryDropdown;

    /// <summary>
    /// Gets or sets the description input on the user report form.
    /// </summary>
    [Tooltip("The description input on the user report form.")]
    public InputField DescriptionInput;

    /// <summary>
    /// Gets or sets the UI shown when there's an error.
    /// </summary>
    [Tooltip("The UI shown when there's an error.")]
    public Canvas ErrorPopup;

    private bool isCreatingUserReport;

    /// <summary>
    /// Gets or sets a value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).
    /// </summary>
    [Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
    public bool IsHotkeyEnabled;

    /// <summary>
    /// Gets or sets a value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.
    /// </summary>
    [Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
    public bool IsInSilentMode;

    /// <summary>
    /// Gets or sets a value indicating whether the user report client reports metrics about itself.
    /// </summary>
    [Tooltip("A value indicating whether the user report client reports metrics about itself.")]
    public bool IsSelfReporting;

    private bool isShowingError;

    private bool isSubmitting;

    /// <summary>
    /// Gets or sets the display text for the progress text.
    /// </summary>
    [Tooltip("The display text for the progress text.")]
    public Text ProgressText;

    /// <summary>
    /// Gets or sets a value indicating whether the user report client send events to analytics.
    /// </summary>
    [Tooltip("A value indicating whether the user report client send events to analytics.")]
    public bool SendEventsToAnalytics;

    /// <summary>
    /// Gets or sets the UI shown while submitting.
    /// </summary>
    [Tooltip("The UI shown while submitting.")]
    public Canvas SubmittingPopup;

    /// <summary>
    /// Gets or sets the summary input on the user report form.
    /// </summary>
    [Tooltip("The summary input on the user report form.")]
    public InputField SummaryInput;

    /// <summary>
    /// Gets or sets the thumbnail viewer on the user report form.
    /// </summary>
    [Tooltip("The thumbnail viewer on the user report form.")]
    public Image ThumbnailViewer;

    private UnityUserReportingUpdater unityUserReportingUpdater;

    /// <summary>
    /// Gets or sets the user report button used to create a user report.
    /// </summary>
    [Tooltip("The user report button used to create a user report.")]
    public Button UserReportButton;

    /// <summary>
    /// Gets or sets the UI for the user report form. Shown after a user report is created.
    /// </summary>
    [Tooltip("The UI for the user report form. Shown after a user report is created.")]
    public Canvas UserReportForm;

    /// <summary>
    /// Gets or sets the User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.
    /// </summary>
    [Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
    public UserReportingPlatformType UserReportingPlatform;

    /// <summary>
    /// Gets or sets the UI for the event raised when a user report is submitting.
    /// </summary>
    [Tooltip("The event raised when a user report is submitting.")]
    public UnityEvent UserReportSubmitting;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current user report.
    /// </summary>
    public UserReport CurrentUserReport { get; private set; }

    /// <summary>
    /// Gets the current state.
    /// </summary>
    public UserReportingState State
    {
        get
        {
            if (this.CurrentUserReport != null)
            {
                if (this.IsInSilentMode)
                {
                    return UserReportingState.Idle;
                }
                else if (this.isSubmitting)
                {
                    return UserReportingState.SubmittingForm;
                }
                else
                {
                    return UserReportingState.ShowingForm;
                }
            }
            else
            {
                if (this.isCreatingUserReport)
                {
                    return UserReportingState.CreatingUserReport;
                }
                else
                {
                    return UserReportingState.Idle;
                }
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Cancels the user report.
    /// </summary>
    public void CancelUserReport()
    {
        this.CurrentUserReport = null;
        this.ClearForm();
    }

    private IEnumerator ClearError()
    {
        yield return new WaitForSeconds(10);
        this.isShowingError = false;
    }

    private void ClearForm()
    {
        this.SummaryInput.text = null;
        this.DescriptionInput.text = null;
    }

    /// <summary>
    /// Creates a user report.
    /// </summary>
    public void CreateUserReport()
    {
        // Check Creating Flag
        if (this.isCreatingUserReport)
        {
            return;
        }

        // Set Creating Flag
        this.isCreatingUserReport = true;

        // Take Main Screenshot
        UnityUserReporting.CurrentClient.TakeScreenshot(2048, 2048, s => { });

        // Take Thumbnail Screenshot
        UnityUserReporting.CurrentClient.TakeScreenshot(512, 512, s => { });

        // Create Report
        UnityUserReporting.CurrentClient.CreateUserReport((br) =>
        {
            // Ensure Project Identifier
            if (string.IsNullOrEmpty(br.ProjectIdentifier))
            {
                Debug.LogWarning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
            }

            // Attachments
            br.Attachments.Add(new UserReportAttachment("Sample Attachment.txt", "SampleAttachment.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("This is a sample attachment.")));

            // Dimensions
            string platform = "Unknown";
            string version = "0.0";
            foreach (UserReportNamedValue deviceMetadata in br.DeviceMetadata)
            {
                if (deviceMetadata.Name == "Platform")
                {
                    platform = deviceMetadata.Value;
                }

                if (deviceMetadata.Name == "Version")
                {
                    version = deviceMetadata.Value;
                }
            }

            br.Dimensions.Add(new UserReportNamedValue("Platform.Version", string.Format("{0}.{1}", platform, version)));

            // Set Current Report
            this.CurrentUserReport = br;

            // Set Creating Flag
            this.isCreatingUserReport = false;

            // Set Thumbnail
            this.SetThumbnail(br);

            // Submit Immediately in Silent Mode
            if (this.IsInSilentMode)
            {
                this.SubmitUserReport();
            }
        });
    }

    private UserReportingClientConfiguration GetConfiguration()
    {
        return new UserReportingClientConfiguration();
    }

    /// <summary>
    /// Gets a value indicating whether the user report is submitting.
    /// </summary>
    /// <returns>A value indicating whether the user report is submitting.</returns>
    public bool IsSubmitting()
    {
        return this.isSubmitting;
    }

    private void SetThumbnail(UserReport userReport)
    {
        if (userReport != null && this.ThumbnailViewer != null)
        {
            byte[] data = Convert.FromBase64String(userReport.Thumbnail.DataBase64);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(data);
            this.ThumbnailViewer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5F, 0.5F));
            this.ThumbnailViewer.preserveAspect = true;
        }
    }

    private void Start()
    {
        // Set Up Event System
        if (Application.isPlaying)
        {
            EventSystem sceneEventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
            if (sceneEventSystem == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        // Configure Client
        bool configured = false;
        if (this.UserReportingPlatform == UserReportingPlatformType.Async)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type asyncUnityUserReportingPlatformType = assembly.GetType("Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform");
            if (asyncUnityUserReportingPlatformType != null)
            {
                object activatedObject = Activator.CreateInstance(asyncUnityUserReportingPlatformType);
                IUserReportingPlatform asyncUnityUserReportingPlatform = activatedObject as IUserReportingPlatform;
                if (asyncUnityUserReportingPlatform != null)
                {
                    UnityUserReporting.Configure(asyncUnityUserReportingPlatform, this.GetConfiguration());
                    configured = true;
                }
            }
        }

        if (!configured)
        {
            UnityUserReporting.Configure(this.GetConfiguration());
        }

        // Ping
        string url = string.Format("https://userreporting.cloud.unity3d.com/api/userreporting/projects/{0}/ping", UnityUserReporting.CurrentClient.ProjectIdentifier);
        UnityUserReporting.CurrentClient.Platform.Post(url, "application/json", Encoding.UTF8.GetBytes("\"Ping\""), (upload, download) => { }, (result, bytes) => { });
    }

    /// <summary>
    /// Submits the user report.
    /// </summary>
    public void SubmitUserReport()
    {
        // Preconditions
        if (this.isSubmitting || this.CurrentUserReport == null)
        {
            return;
        }

        // Set Submitting Flag
        this.isSubmitting = true;

        // Set Summary
        if (this.SummaryInput != null)
        {
            this.CurrentUserReport.Summary = this.SummaryInput.text;
        }

        // Set Category
        if (this.CategoryDropdown != null)
        {
            Dropdown.OptionData optionData = this.CategoryDropdown.options[this.CategoryDropdown.value];
            string category = optionData.text;
            this.CurrentUserReport.Dimensions.Add(new UserReportNamedValue("Category", category));
            this.CurrentUserReport.Fields.Add(new UserReportNamedValue("Category", category));
        }

        // Set Description
        // This is how you would add additional fields.
        if (this.DescriptionInput != null)
        {
            UserReportNamedValue userReportField = new UserReportNamedValue();
            userReportField.Name = "Description";
            userReportField.Value = this.DescriptionInput.text;
            this.CurrentUserReport.Fields.Add(userReportField);
        }

        // Clear Form
        this.ClearForm();

        // Raise Event
        this.RaiseUserReportSubmitting();

        // Send Report
        UnityUserReporting.CurrentClient.SendUserReport(this.CurrentUserReport, (uploadProgress, downloadProgress) =>
        {
            if (this.ProgressText != null)
            {
                string progressText = string.Format("{0:P}", uploadProgress);
                this.ProgressText.text = progressText;
            }
        }, (success, br2) =>
        {
            if (!success)
            {
                this.isShowingError = true;
                this.StartCoroutine(this.ClearError());
            }

            this.CurrentUserReport = null;
            this.isSubmitting = false;
        });
    }

    private void Update()
    {
        // Hotkey Support
        if (this.IsHotkeyEnabled)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    this.CreateUserReport();
                }
            }
        }

        // Update Client
        UnityUserReporting.CurrentClient.IsSelfReporting = this.IsSelfReporting;
        UnityUserReporting.CurrentClient.SendEventsToAnalytics = this.SendEventsToAnalytics;

        // Update UI
        if (this.UserReportButton != null)
        {
            this.UserReportButton.interactable = this.State == UserReportingState.Idle;
        }

        if (this.UserReportForm != null)
        {
            this.UserReportForm.enabled = this.State == UserReportingState.ShowingForm;
        }

        if (this.SubmittingPopup != null)
        {
            this.SubmittingPopup.enabled = this.State == UserReportingState.SubmittingForm;
        }

        if (this.ErrorPopup != null)
        {
            this.ErrorPopup.enabled = this.isShowingError;
        }

        // Update Client
        // The UnityUserReportingUpdater updates the client at multiple points during the current frame.
        this.unityUserReportingUpdater.Reset();
        this.StartCoroutine(this.unityUserReportingUpdater);
    }

    #endregion

    #region Virtual Methods

    /// <summary>
    /// Occurs when a user report is submitting.
    /// </summary>
    protected virtual void RaiseUserReportSubmitting()
    {
        if (this.UserReportSubmitting != null)
        {
            this.UserReportSubmitting.Invoke();
        }
    }

    #endregion
}