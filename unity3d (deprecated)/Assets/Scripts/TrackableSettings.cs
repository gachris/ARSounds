/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using System.Timers;
using UnityEngine;
using Vuforia;

public class TrackableSettings : MonoBehaviour
{
    #region PRIVATE_MEMBERS
    [SerializeField] private bool deviceTrackerEnabled;
    private DevicePoseBehaviour positionalDeviceTracker;
    private Timer relocalizationStatusDelayTimer;
    private Timer resetDeviceTrackerTimer;
    #endregion // PRIVATE_MEMBERS


    #region UNITY_MONOBEHAVIOUR_METHODS

    private void Awake()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;
    }

    private void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnDevicePoseStatusChanged;

        // Setup a timer to have short delay before processing RELOCALIZING status
        relocalizationStatusDelayTimer = new Timer(1000);
        relocalizationStatusDelayTimer.Elapsed += RelocalizingStatusDelay;
        relocalizationStatusDelayTimer.AutoReset = false;

        // Setup a timer to restart the DeviceTracker if tracking does not receive
        // status change from StatusInfo.RELOCALIZATION after 10 seconds.
        resetDeviceTrackerTimer = new Timer(10000);
        resetDeviceTrackerTimer.Elapsed += ResetDeviceTracker;
        resetDeviceTrackerTimer.AutoReset = false;
    }

    private void OnDestroy()
    {
        VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
        VuforiaApplication.Instance.OnVuforiaInitialized -= OnVuforiaInitialized;
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS


    #region VUFORIA_CALLBACKS

    private void OnVuforiaInitialized(VuforiaInitError obj)
    {
        positionalDeviceTracker = VuforiaBehaviour.Instance.DevicePoseBehaviour;
        if (positionalDeviceTracker != null)
        {
            Debug.Log("Successfully initialized the positional device tracker");
        }
        else
        {
            Debug.LogError("Failed to initialize the positional device tracker");
        }
    }

    private void OnVuforiaStarted()
    {
        // Device Tracking is off by default for mobile samples.
        // The deviceTrackerEnabled public Inspector option allows you to specify
        // per-sample if the option is to be on by default. (i.e. Model Targets)
        ToggleDeviceTracking(deviceTrackerEnabled);
    }

    private void OnDevicePoseStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
    {
        Debug.Log("OnDevicePoseStatusChanged(" + targetStatus.Status + ", " + targetStatus.StatusInfo + ")");

        if (targetStatus.StatusInfo == StatusInfo.RELOCALIZING)
        {
            // If the status is Relocalizing, then start the timer if it isn't active
            if (!relocalizationStatusDelayTimer.Enabled)
            {
                relocalizationStatusDelayTimer.Start();
            }
        }
        else
        {
            // If the status is not Relocalizing, then stop the timers if they are active
            if (relocalizationStatusDelayTimer.Enabled)
            {
                relocalizationStatusDelayTimer.Stop();
            }

            if (resetDeviceTrackerTimer.Enabled)
            {
                resetDeviceTrackerTimer.Stop();
            }

            // Clear the status message
            StatusMessage.Instance.Display(string.Empty);
        }
    }

    #endregion // VUFORIA_CALLBACKS


    #region PRIVATE_METHODS

    // This is a C# delegate method for the Timer:
    // ElapsedEventHandler(object sender, ElapsedEventArgs e)
    private void RelocalizingStatusDelay(System.Object source, ElapsedEventArgs e)
    {
        StatusMessage.Instance.Display("Point camera to previous position to restore tracking");

        if (!resetDeviceTrackerTimer.Enabled)
        {
            resetDeviceTrackerTimer.Start();
        }
    }

    // This is a C# delegate method for the Timer:
    // ElapsedEventHandler(object sender, ElapsedEventArgs e)
    private void ResetDeviceTracker(System.Object source, ElapsedEventArgs e)
    {
        ToggleDeviceTracking(false);
        ToggleDeviceTracking(true);
    }

    #endregion // PRIVATE_METHODS


    #region PUBLIC_METHODS

    public bool IsDeviceTrackingEnabled()
    {
        return deviceTrackerEnabled;
    }

    // TODO: fix ToggleDeviceTracking
    public virtual void ToggleDeviceTracking(bool enableDeviceTracking)
    {
        //if (this.positionalDeviceTracker != null)
        //{
        //    if (enableDeviceTracking)
        //    {
        //        // if the positional device tracker is not yet started, start it
        //        if (!this.positionalDeviceTracker.IsActive)
        //        {
        //            if (this.positionalDeviceTracker.Start())
        //            {
        //                Debug.Log("Successfully started device tracker");
        //            }
        //            else
        //            {
        //                Debug.LogError("Failed to start device tracker");
        //            }
        //        }
        //    }
        //    else if (this.positionalDeviceTracker.IsActive)
        //    {
        //        this.positionalDeviceTracker.Stop();

        //        Debug.Log("Successfully stopped device tracker");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Failed to toggle device tracker state, make sure device tracker is initialized");
        //}

        //this.deviceTrackerEnabled = this.positionalDeviceTracker.IsActive;
    }

    #endregion //PUBLIC_METHODS
}
