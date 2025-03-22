using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class MenuOptionsBehaviour : MonoBehaviour
{
    public Toggle DeviceTrackerToggle;
    public Toggle AutofocusToggle;
    public Button FlashOnButton;
    public Button FlashOffButton;
    public Toggle BurgerToggle;
    public GameObject ChildMenu;

    #region PRIVATE_MEMBERS
    private CameraSettings m_CameraSettings;
    private TrackableSettings m_TrackableSettings;
    private OptionsConfig m_OptionsConfig;
    #endregion //PRIVATE_MEMBERS

    public bool IsDisplayed { get; private set; }

    #region MONOBEHAVIOUR_METHODS
    protected virtual void Start()
    {
        m_CameraSettings = FindObjectOfType<CameraSettings>();
        m_TrackableSettings = FindObjectOfType<TrackableSettings>();
        m_OptionsConfig = FindObjectOfType<OptionsConfig>();

        VuforiaApplication.Instance.OnVuforiaPaused += OnPaused;
    }
    #endregion //MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    public void ToggleAutofocus(bool enable)
    {
        if (m_CameraSettings)
            m_CameraSettings.SwitchAutofocus(enable);
    }

    public void ToggleTorch(bool enable)
    {
        if (FlashOnButton && FlashOffButton && m_CameraSettings)
        {
            m_CameraSettings.SwitchFlashTorch(enable);

            // Update UI toggle status (ON/OFF) in case the flash switch failed
            if (m_CameraSettings.IsFlashTorchEnabled())
            {
                FlashOnButton.gameObject.SetActive(true);
                FlashOffButton.gameObject.SetActive(false);
            }
            else
            {
                FlashOnButton.gameObject.SetActive(false);
                FlashOffButton.gameObject.SetActive(true);
            }
        }
    }

    public void ToggleExtendedTracking(bool enable)
    {
        if (m_TrackableSettings)
        {
            m_TrackableSettings.ToggleDeviceTracking(enable);
            if (enable && m_TrackableSettings.IsDeviceTrackingEnabled())
            {
                ToastMessage.Show("Device Track Enabled Successfully!", ToastMessage.Position.bottom, ToastMessage.Time.threeSecond);
            }
            else if (!enable && !m_TrackableSettings.IsDeviceTrackingEnabled())
            {
                ToastMessage.Show("Device Track Disabled Successfully!", ToastMessage.Position.bottom, ToastMessage.Time.threeSecond);
            }
            else
            {
                ToastMessage.Show("Unable to set Device Track Enable!", ToastMessage.Position.bottom, ToastMessage.Time.threeSecond);
            }
        }
    }

    public void ResetDeviceTracker()
    {
        if (VuforiaBehaviour.Instance.DevicePoseBehaviour.Reset())
        {
            Debug.Log("Successfully reset device tracker");
        }
        else
        {
            Debug.LogError("Failed to reset device tracker");
        }
    }

    public void ShowChildMenu(bool enable)
    {
        if (m_OptionsConfig && m_OptionsConfig.AnyOptionsEnabled())
        {
            if (enable)
            {
                UpdateUI();
                IsDisplayed = true;
                StartCoroutine(FadeElement(ChildMenu, 0, 1, 0.6f));
            }
            else
            {
                IsDisplayed = false;
                StartCoroutine(FadeElement(ChildMenu, 1, 0, 0.6f));
            }
        }
    }

    #endregion //PUBLIC_METHODS

    #region PRIVATE_METHODS
    private void OnPaused(bool paused)
    {
        if (paused)
        {
            // Handle any tasks when app is paused here:
        }
        else
        {
            // Handle any tasks when app is resume here:

            // The flash torch is switched off by the OS automatically when app is paused.
            // On resume, update torch UI toggle to match torch status.
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (DeviceTrackerToggle && m_TrackableSettings)
            DeviceTrackerToggle.isOn = m_TrackableSettings.IsDeviceTrackingEnabled();

        if (FlashOnButton && m_CameraSettings)
        {
            if (m_CameraSettings.IsFlashTorchEnabled())
            {
                FlashOnButton.gameObject.SetActive(true);
                FlashOffButton.gameObject.SetActive(false);
            }
            else
            {
                FlashOnButton.gameObject.SetActive(false);
                FlashOffButton.gameObject.SetActive(true);
            }
        }

        if (AutofocusToggle && m_CameraSettings)
            AutofocusToggle.isOn = m_CameraSettings.IsAutofocusEnabled();

        if (BurgerToggle)
            BurgerToggle.isOn = IsDisplayed;
    }

    private IEnumerator FadeElement(GameObject gameObject, float start, float end, float lerpTime)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        while (percentageComplete < 1)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;
            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            canvasGroup.alpha = currentValue;
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion //PRIVATE_METHODS

}
