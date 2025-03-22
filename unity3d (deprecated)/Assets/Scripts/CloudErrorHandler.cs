/*===============================================================================
Copyright (c) 2018 PTC Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class CloudErrorHandler : VuforiaMonoBehaviour
{

    #region PRIVATE_MEMBERS
    private bool mustRestartApp;
    private string errorTitle;
    private string errorMsg;
    private CloudRecoBehaviour m_CloudRecoBehaviour;
    #endregion PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    private void Start()
    {
        // Register this event handler with CloudRecoBehaviour
        m_CloudRecoBehaviour = FindObjectOfType<CloudRecoBehaviour>();

        if (m_CloudRecoBehaviour)
        {
            m_CloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
            m_CloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
            m_CloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
            m_CloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
            m_CloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }

        if (VuforiaConfiguration.Instance.Vuforia.LicenseKey == string.Empty && m_CloudRecoBehaviour)
        {
            errorTitle = "Cloud Reco Init Error";
            errorMsg = "Vuforia License Key not found. Cloud Reco requires a valid license.";

            MessageBox.Show(errorTitle, errorMsg, false, null);
        }
    }

    private void OnDestroy()
    {
        m_CloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        m_CloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        m_CloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        m_CloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        m_CloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    #endregion MONOBEHAVIOUR_METHODS


    #region INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler

    /// <summary>
    /// Called if Cloud Reco initialization fails
    /// </summary>
    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        switch (initError)
        {
            case CloudRecoBehaviour.InitError.NO_NETWORK_CONNECTION:
                mustRestartApp = true;
                errorTitle = "Network Unavailable";
                errorMsg = "Please check your Internet connection and try again.";
                break;
            case CloudRecoBehaviour.InitError.SERVICE_NOT_AVAILABLE:
                errorTitle = "Service Unavailable";
                errorMsg = "Failed to initialize app because the service is not available.";
                break;
        }

        // Prepend the error code in red
        errorMsg = "<color=red>" + initError.ToString().Replace("_", " ") + "</color>\n\n" + errorMsg;

        // Remove rich text tags for console logging
        var errorTextConsole = errorMsg.Replace("<color=red>", "").Replace("</color>", "");

        Debug.LogError("OnInitError() - Initialization Error: " + initError + "\n\n" + errorTextConsole);

        MessageBox.Show(errorTitle, errorMsg, true, CloseDialog);
    }

    /// <summary>
    /// Called if a Cloud Reco update error occurs
    /// </summary>
    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        switch (updateError)
        {
            case CloudRecoBehaviour.QueryError.AUTHORIZATION_FAILED:
                errorTitle = "Authorization Error";
                errorMsg = "The cloud recognition service access keys are incorrect or have expired.";
                break;
            case CloudRecoBehaviour.QueryError.NO_NETWORK_CONNECTION:
                errorTitle = "Network Unavailable";
                errorMsg = "Please check your Internet connection and try again.";
                break;
            case CloudRecoBehaviour.QueryError.PROJECT_SUSPENDED:
                errorTitle = "Authorization Error";
                errorMsg = "The cloud recognition service has been suspended.";
                break;
            case CloudRecoBehaviour.QueryError.REQUEST_TIMEOUT:
                errorTitle = "Request Timeout";
                errorMsg = "The network request has timed out, please check your Internet connection and try again.";
                break;
            case CloudRecoBehaviour.QueryError.SERVICE_NOT_AVAILABLE:
                errorTitle = "Service Unavailable";
                errorMsg = "The service is unavailable, please try again later.";
                break;
            case CloudRecoBehaviour.QueryError.TIMESTAMP_OUT_OF_RANGE:
                errorTitle = "Clock Sync Error";
                errorMsg = "Please update the date and time and try again.";
                break;
            case CloudRecoBehaviour.QueryError.UPDATE_SDK:
                errorTitle = "Unsupported Version";
                errorMsg = "The application is using an unsupported version of Vuforia.";
                break;
            case CloudRecoBehaviour.QueryError.BAD_FRAME_QUALITY:
                errorTitle = "Bad Frame Quality";
                errorMsg = "Low-frame quality has been continuously observed.\n\nError Event Received on Frame: " + Time.frameCount;
                break;
        }

        // Prepend the error code in red
        errorMsg = "<color=red>" + updateError.ToString().Replace("_", " ") + "</color>\n\n" + errorMsg;

        // Remove rich text tags for console logging
        var errorTextConsole = errorMsg.Replace("<color=red>", "").Replace("</color>", "");

        Debug.LogError("OnUpdateError() - Update Error: " + updateError + "\n\n" + errorTextConsole);

        MessageBox.Show(errorTitle, errorMsg, true, CloseDialog);
    }

    // These interface methods implemented in seperate ICloudRecoEventHandler class
    public void OnInitialized() { }
    public void OnInitialized(CloudRecoBehaviour targetFinder) { }
    public void OnStateChanged(bool scanning) { }
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult targetSearchResult) { }

    #endregion INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler


    #region PUBLIC_METHODS

    public void CloseDialog()
    {
        if (mustRestartApp) RestartApplication();
    }

    #endregion PUBLIC_METHODS


    #region PRIVATE_METHODS

    // Callback for network-not-available error message
    private void RestartApplication()
    {
        int startLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2;
        if (startLevel < 0) startLevel = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }

    #endregion PRIVATE_METHODS
}
