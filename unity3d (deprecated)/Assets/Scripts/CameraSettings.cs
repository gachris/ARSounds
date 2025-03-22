/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using System.Collections;
using UnityEngine;
using Vuforia;

public class CameraSettings : MonoBehaviour
{
    #region PRIVATE_MEMBERS
    private bool mVuforiaStarted = false;
    private bool mAutofocusEnabled = true;
    private bool mFlashTorchEnabled = false;
    #endregion //PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    private void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        VuforiaApplication.Instance.OnVuforiaPaused += OnPaused;
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public bool IsFlashTorchEnabled()
    {
        return mFlashTorchEnabled;
    }

    public void SwitchFlashTorch(bool ON)
    {
        if (VuforiaBehaviour.Instance.CameraDevice.SetFlash(ON))
        {
            Debug.Log("Successfully turned flash " + ON);
            mFlashTorchEnabled = ON;
        }
        else
        {
            Debug.Log("Failed to set the flash torch " + ON);
            mFlashTorchEnabled = false;
        }
    }

    public bool IsAutofocusEnabled()
    {
        return mAutofocusEnabled;
    }

    public void SwitchAutofocus(bool ON)
    {
        if (ON)
        {
            if (VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
            {
                Debug.Log("Successfully enabled continuous autofocus.");
                mAutofocusEnabled = true;
            }
            else
            {
                // Fallback to normal focus mode
                Debug.Log("Failed to enable continuous autofocus, switching to normal focus mode");
                mAutofocusEnabled = false;
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
            }
        }
        else
        {
            Debug.Log("Disabling continuous autofocus (enabling normal focus mode).");
            mAutofocusEnabled = false;
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
        }
    }

    public void TriggerAutofocusEvent()
    {
        // Trigger an autofocus event
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_TRIGGERAUTO);

        // Then restore original focus mode
        StartCoroutine(RestoreOriginalFocusMode());
    }

    // TODO: fix RestartCamera
    public bool RestartCamera()
    {
        //ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        //if (objectTracker != null)
        //{
        //    objectTracker.Stop();
        //}

        //// Camera must be deinitialized before attempting to deinitialize trackers
        //CameraDevice.Instance.Stop();
        //CameraDevice.Instance.Deinit();

        //if (!CameraDevice.Instance.Init())
        //{
        //    Debug.Log("Failed to initialize the camera.");
        //    return false;
        //}
        //if (!CameraDevice.Instance.Start())
        //{
        //    Debug.Log("Failed to start the camera.");
        //    return false;
        //}

        //if (objectTracker != null)
        //{
        //    if (!objectTracker.Start())
        //    {
        //        Debug.Log("Failed to restart the Object Tracker.");
        //        return false;
        //    }
        //}

        return true;
    }
    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS
    private void OnVuforiaStarted()
    {
        mVuforiaStarted = true;
        // Try enabling continuous autofocus
        SwitchAutofocus(true);
    }

    private void OnPaused(bool paused)
    {
        bool appResumed = !paused;
        if (appResumed && mVuforiaStarted)
        {
            // Restore original focus mode when app is resumed
            if (mAutofocusEnabled)
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            else
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
        }
        else
        {
            // Set the torch flag to false on pause (because the flash torch is switched off by the OS automatically)
            mFlashTorchEnabled = false;
        }
    }

    private IEnumerator RestoreOriginalFocusMode()
    {
        // Wait 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        // Restore original focus mode
        if (mAutofocusEnabled)
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        else
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
    }

    #endregion // PRIVATE_METHODS
}
