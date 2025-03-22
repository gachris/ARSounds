/*==============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/
using UnityEngine;
using Vuforia;

/// <summary>
/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
/// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
/// The current state is visualized and new results are enabled using the TargetFinder API.
/// </summary>
public class CloudRecoEventHandler : VuforiaMonoBehaviour
{
    #region PRIVATE_MEMBERS
    private CloudRecoBehaviour m_CloudRecoBehaviour;
    private CloudRecoBehaviour m_TargetFinder;
    #endregion // PRIVATE_MEMBERS


    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour 
    /// that is used for augmentations of new cloud reco results.
    /// </summary>
    [Tooltip("Here you can set the ImageTargetBehaviour from the scene that will be used to " +
             "augment new cloud reco search results.")]
    public ImageTargetBehaviour m_ImageTargetBehaviour;
    public UnityEngine.UI.Image m_CloudActivityIcon;
    public UnityEngine.UI.Image m_CloudIdleIcon;
    #endregion // PUBLIC_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    /// <summary>
    /// Register for events at the CloudRecoBehaviour
    /// </summary>
    private void Start()
    {
        // Register this event handler at the CloudRecoBehaviour
        m_CloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (m_CloudRecoBehaviour)
        {
            m_CloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
            m_CloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
            m_CloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
            m_CloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
            m_CloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }

        if (m_CloudActivityIcon)
        {
            m_CloudActivityIcon.enabled = false;
        }
    }

    private void Update()
    {
        if (m_CloudRecoBehaviour.CloudRecoInitialized && m_TargetFinder != null)
        {
            SetCloudActivityIconVisible(m_TargetFinder.Requesting);
        }

        if (m_CloudIdleIcon)
        {
            m_CloudIdleIcon.color = m_CloudRecoBehaviour.RecoStarted ? Color.white : Color.gray;
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
    #endregion // MONOBEHAVIOUR_METHODS


    #region INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler

    public void OnInitialized(CloudRecoBehaviour targetFinder)
    {
        Debug.Log("Cloud Reco initialized successfully.");

        m_TargetFinder = targetFinder;
    }

    // Error callback methods implemented in CloudErrorHandler
    public void OnInitError(CloudRecoBehaviour.InitError initError) { }
    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError) { }


    /// <summary>
    /// when we start scanning, unregister Trackable from the ImageTargetBehaviour, 
    /// then delete all trackables
    /// </summary>
    public void OnStateChanged(bool scanning)
    {
        Debug.Log("<color=blue>OnStateChanged(): </color>" + scanning);

        // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call:
        // 1. TargetFinder.Stop()
        // 2. All registered ICloudRecoEventHandler.OnStateChanged() with false.

        // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call:
        // 1. TargetFinder.StartRecognition()
        // 2. All registered ICloudRecoEventHandler.OnStateChanged() with true.
    }

    /// <summary>
    /// Handles new search results
    /// </summary>
    /// <param name="targetSearchResult"></param>
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult targetSearchResult)
    {
        Debug.Log("<color=blue>OnNewSearchResult(): </color>" + targetSearchResult.TargetName);

        CloudRecoBehaviour.CloudRecoSearchResult cloudRecoResult = (CloudRecoBehaviour.CloudRecoSearchResult)targetSearchResult;

        // This code demonstrates how to reuse an ImageTargetBehaviour for new search results
        // and modifying it according to the metadata. Depending on your application, it can
        // make more sense to duplicate the ImageTargetBehaviour using Instantiate() or to
        // create a new ImageTargetBehaviour for each new result. Vuforia will return a new
        // object with the right script automatically if you use:
        // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)

        // Check if the metadata isn't null
        if (cloudRecoResult.MetaData == null)
        {
            Debug.Log("Target metadata not available.");
        }
        else
        {
            Debug.Log("MetaData: " + cloudRecoResult.MetaData);
            Debug.Log("TargetName: " + cloudRecoResult.TargetName);
            Debug.Log("TrackingRating: " + cloudRecoResult.TrackingRating);
            Debug.Log("UniqueTargetId: " + cloudRecoResult.UniqueTargetId);
        }

        // Stop the scanning by disabling the behaviour
        m_CloudRecoBehaviour.enabled = false;

        // Clear any existing trackables
        m_TargetFinder.ClearObservers(false);

        // Enable the new result with the same ImageTargetBehaviour:
        m_TargetFinder.EnableObservers(cloudRecoResult, m_ImageTargetBehaviour.gameObject);

        // Pass the TargetSearchResult to the Trackable Event Handler for processing
        m_ImageTargetBehaviour.gameObject.SendMessage("TargetCreated", cloudRecoResult, SendMessageOptions.DontRequireReceiver);
    }
    #endregion // INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler


    #region PRIVATE_METHODS
    private void SetCloudActivityIconVisible(bool visible)
    {
        if (!m_CloudActivityIcon) return;

        m_CloudActivityIcon.enabled = visible;
    }
    #endregion // PRIVATE_METHODS
}
