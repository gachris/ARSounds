using System.Collections;
using UnityEngine;

namespace Assets
{
    public class AppStartupBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            var loadScene = LoadScene();
            StartCoroutine(loadScene);
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(2);

            SceneLoader.FadeIn(SceneLoader.Scenes.CloudReco);
        }
    }
}