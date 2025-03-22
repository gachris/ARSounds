using System.Threading.Tasks;
using UnityEngine;
using Vuforia;

namespace Assets
{
    public class CloudContentBehaviour : MonoBehaviour
    {
        #region PRIVATE_MEMBER_VARIABLES

        private UnityAuthClient _authClient;
        private ITargetService _targetService;
        private AugmentationObject _augmentationObject;
        private CloudRecoBehaviour.CloudRecoSearchResult _targetSearchResult;

        #endregion // PRIVATE_MEMBER_VARIABLES

        private void Start()
        {
            _authClient = new UnityAuthClient();
            _augmentationObject = GameObject.FindObjectOfType<AugmentationObject>();
        }

        #region PUBLIC_METHODS

        public async void ShowTargetInfo(bool show)
        {
            try
            {
                if (_targetSearchResult.UniqueTargetId != _augmentationObject.UniqueTargetId)
                {
                    _targetService = await GetService();
                    var result = await _targetService.Get(_targetSearchResult.UniqueTargetId).ConfigureAwait(false);
                    if (result.StatusCode == StatusCode.Success)
                        _augmentationObject.Initialize(result.Response.Result);
                }
                _augmentationObject.Show(show);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "", true, null);
            }
        }

        public void HandleTargetFinderResult(CloudRecoBehaviour.CloudRecoSearchResult targetSearchResult)
        {
            Debug.Log("<color=blue>HandleTargetFinderResult(): " + targetSearchResult.TargetName + "</color>");
            _targetSearchResult = targetSearchResult;
        }

        #endregion // PUBLIC_METHODS

        private async Task<TargetService> GetService()
        {
            var token = await _authClient.GetToken();
            return new TargetService(new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken));
        }
    }
}