using Assets;
using Vuforia;

namespace UnityEngine
{
    public class CloudTrackableEventHandler : DefaultObserverEventHandler
    {
        #region PRIVATE_MEMBERS
        private CloudRecoBehaviour m_CloudRecoBehaviour;
        private CloudContentBehaviour m_CloudContentManager;
        #endregion // PRIVATE_MEMBERS


        #region MONOBEHAVIOUR_METHODS
        protected override void Start()
        {
            base.Start();

            m_CloudRecoBehaviour = FindObjectOfType<CloudRecoBehaviour>();
            m_CloudContentManager = FindObjectOfType<CloudContentBehaviour>();
        }
        #endregion // MONOBEHAVIOUR_METHODS


        #region BUTTON_METHODS
        public void OnReset()
        {
            Debug.Log("<color=blue>OnReset()</color>");

            OnTrackingLost();
            m_CloudRecoBehaviour.ClearObservers(false);
        }
        #endregion BUTTON_METHODS


        #region PUBLIC_METHODS
        /// <summary>
        /// Method called from the CloudRecoEventHandler
        /// when a new target is created
        /// </summary>
        public void TargetCreated(CloudRecoBehaviour.CloudRecoSearchResult targetSearchResult)
        {
            m_CloudContentManager.HandleTargetFinderResult(targetSearchResult);
        }
        #endregion // PUBLIC_METHODS


        #region PROTECTED_METHODS

        protected override void OnTrackingFound()
        {
            Debug.Log("<color=blue>OnTrackingFound()</color>");

            base.OnTrackingFound();


            if (m_CloudRecoBehaviour)
            {
                // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call TargetFinder.Stop()
                // and also call all registered ICloudRecoEventHandler.OnStateChanged() with false.
                m_CloudRecoBehaviour.enabled = false;
            }

            if (m_CloudContentManager)
            {
                transform.localScale = new Vector3(32, 32, 32);
                m_CloudContentManager.ShowTargetInfo(true);
            }
        }

        protected override void OnTrackingLost()
        {
            Debug.Log("<color=blue>OnTrackingLost()</color>");

            base.OnTrackingLost();

            if (m_CloudRecoBehaviour)
            {
                // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call TargetFinder.StartRecognition()
                // and also call all registered ICloudRecoEventHandler.OnStateChanged() with true.
                m_CloudRecoBehaviour.enabled = true;
            }

            if (m_CloudContentManager)
            {
                m_CloudContentManager.ShowTargetInfo(false);
            }
        }

        #endregion // PROTECTED_METHODS
    }

}