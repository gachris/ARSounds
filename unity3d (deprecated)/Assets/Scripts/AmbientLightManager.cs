/*===============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

[RequireComponent(typeof(Light))]
public class AmbientLightManager : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private IlluminationData illuminationManager;
    private Light sceneLight;
    private float maxIntensity;

    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    private void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;

        this.sceneLight = GetComponent<Light>();
        this.maxIntensity = this.sceneLight.intensity;
    }

    private void Update()
    {
        if (illuminationManager != null && illuminationManager.AmbientIntensity != null)
        {
            float intensity = (float)illuminationManager.AmbientIntensity / 1000;

            // Set light intensity to range between 0 and it's max intensity value
            sceneLight.intensity = Mathf.Clamp(intensity, 0, maxIntensity);

            // Set to scene's ambient light intensity and clamp between 0 and 1
            RenderSettings.ambientIntensity = Mathf.Clamp01(intensity);
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region VUFORIA_CALLBACK_METHODS

    private void OnVuforiaStarted()
    {
        this.illuminationManager = VuforiaBehaviour.Instance.World.IlluminationData;
    }

    #endregion // VUFORIA_CALLBACK_METHODS
}
