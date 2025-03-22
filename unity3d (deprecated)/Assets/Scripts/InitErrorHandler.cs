/*===============================================================================
Copyright (c) 2016-2018 PTC Inc. All Rights Reserved.
 
Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class InitErrorHandler : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES
    private readonly string key;
    private string errorMsg;
    private const string errorTitle = "Vuforia Initialization Error";
    #endregion PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    private void Awake()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitError;
    }

    private void OnDestroy()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized -= OnVuforiaInitError;
    }
    #endregion MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS
    private void OnVuforiaInitError(VuforiaInitError error)
    {
        if (error != VuforiaInitError.NONE)
        {
            ShowErrorMessage(error);
        }
    }

    private void ShowErrorMessage(VuforiaInitError errorCode)
    {
        switch (errorCode)
        {
            case VuforiaInitError.DEVICE_NOT_SUPPORTED:
                errorMsg =
                    "Failed to initialize Vuforia because this device is not supported.";
                break;
            case VuforiaInitError.LICENSE_CONFIG_MISSING_KEY:
                errorMsg =
                    "Vuforia App Key is missing. \n" +
                    "Please get a valid key, by logging into your account at " +
                    "developer.vuforia.com and creating a new project.";
                break;
            case VuforiaInitError.LICENSE_CONFIG_INVALID_KEY:
                errorMsg =
                    "Vuforia App key is invalid. \n" +
                    "Please get a valid key, by logging into your account at " +
                    "developer.vuforia.com and creating a new project. \n\n" +
                    GetKeyInfo();
                break;
            case VuforiaInitError.LICENSE_CONFIG_NO_NETWORK_TRANSIENT:
                errorMsg = "Unable to contact server. Please try again later.";
                break;
            case VuforiaInitError.LICENSE_CONFIG_NO_NETWORK_PERMANENT:
                errorMsg = "No network available. Please make sure you are connected to the Internet.";
                break;
            case VuforiaInitError.LICENSE_CONFIG_KEY_CANCELED:
                errorMsg =
                    "This App license key has been cancelled and may no longer be used. " +
                    "Please get a new license key. \n\n" +
                    GetKeyInfo();
                break;
            case VuforiaInitError.LICENSE_CONFIG_PRODUCT_TYPE_MISMATCH:
                errorMsg =
                    "Vuforia App key is not valid for this product. Please get a valid key, " +
                    "by logging into your account at developer.vuforia.com and choosing the " +
                    "right product type during project creation. \n\n" +
                    GetKeyInfo() + "\n\n" +
                    "Note that Universal Windows Platform (UWP) apps require " +
                    "a license key created on or after August 9th, 2016.";
                break;
            case VuforiaInitError.PERMISSION_ERROR:
                errorMsg =
                    "User denied Camera access to this app.\n" +
                    "To restore, enable Camera access in Settings:\n" +
                    "Settings > Privacy > Camera > " + Application.productName + "\n" +
                    "Also verify that the Camera is enabled in:\n" +
                    "Settings > General > Restrictions.";
                break;
            case VuforiaInitError.INITIALIZATION:
                errorMsg = "Failed to initialize Vuforia.";
                break;
        }

        // Prepend the error code in red
        errorMsg = "<color=red>" + errorCode.ToString().Replace("_", " ") + "</color>\n\n" + errorMsg;

        // Remove rich text tags for console logging
        var errorTextConsole = errorMsg.Replace("<color=red>", "").Replace("</color>", "");

        Debug.LogError("Vuforia initialization failed: " + errorCode + "\n\n" + errorTextConsole);

        MessageBox.Show(errorTitle, errorMsg, true, OnErrorDialogClose);
    }

    private string GetKeyInfo()
    {
        string key = VuforiaConfiguration.Instance.Vuforia.LicenseKey;
        string keyInfo = key.Length > 10
            ? "Your current key is <color=red>" + key.Length + "</color> characters in length. " +
                "It begins with <color=red>" + key.Substring(0, 5) + "</color> " +
                "and ends with <color=red>" + key.Substring(key.Length - 5, 5) + "</color>."
            : "Your current key is <color=red>" + key.Length + "</color> characters in length. \n" +
                "The key is: <color=red>" + key + "</color>.";
        return keyInfo;
    }
    #endregion //PRIVATE_METHODS


    #region PUBLIC_METHODS
    public void OnErrorDialogClose()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion //PUBLIC_METHODS
}
