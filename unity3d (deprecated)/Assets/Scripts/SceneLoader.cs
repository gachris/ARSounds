using System.Collections;
using UnityEngine.UI;

namespace UnityEngine
{
    public class SceneLoader : MonoBehaviour
    {
        public enum Scenes
        {
            Startup = 0,
            CloudReco = 1
        }

        public static void FadeIn(Scenes scene)
        {
            SceneLoader.FadeIn(scene, Color.black);
        }

        public static void FadeOut()
        {
            SceneLoader.FadeOut(Color.black);
        }

        public static void FadeIn(Scenes scene, Color color, float lerpTime = 0.6f)
        {
            var fadeManager = new GameObject("FadeManager");
            SceneLoader messageBox = fadeManager.AddComponent<SceneLoader>();
            messageBox.Initialize(fadeManager, color);
            messageBox.StartCoroutine(messageBox.Fade(fadeManager, 0, 1, lerpTime, scene.ToString()));
        }

        private static void FadeOut(Color color, float lerpTime = 0.6f)
        {
            var fadeManager = new GameObject("FadeManager");
            SceneLoader messageBox = fadeManager.AddComponent<SceneLoader>();
            messageBox.Initialize(fadeManager, color);
            messageBox.StartCoroutine(messageBox.Fade(fadeManager, 1, 0, lerpTime));
        }

        private void Initialize(GameObject fadeManager, Color color)
        {
            AddCanvas(fadeManager);
            AddCanvasScaler(fadeManager);
            AddImage(fadeManager, color);
            AddCanvasGroup(fadeManager);

            fadeManager.AddComponent<GraphicRaycaster>();
            fadeManager.GetComponent<RectTransform>().SetAnchor(RectTransformExtensions.AnchorPresets.StretchAll, 0, 0);
            fadeManager.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        private IEnumerator Fade(GameObject fadeManager, float start, float end, float leartTime, string scene = null)
        {
            CanvasGroup canvasGroup = fadeManager.GetComponent<CanvasGroup>();

            float timeStartedLerping = Time.time;
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / leartTime;

            while (percentageComplete < 1)
            {
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / leartTime;
                float currentValue = Mathf.Lerp(start, end, percentageComplete);
                canvasGroup.alpha = currentValue;
                yield return new WaitForFixedUpdate();
            }

            if (!string.IsNullOrEmpty(scene)) UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
            else Object.Destroy(fadeManager);
        }

        private static void AddCanvas(GameObject fadeManager)
        {
            Canvas parentGameObject = fadeManager.AddComponent<Canvas>();
            parentGameObject.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private static void AddCanvasScaler(GameObject fadeManager)
        {
            CanvasScaler canvasScaler = fadeManager.AddComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
            canvasScaler.matchWidthOrHeight = 0;
            canvasScaler.referencePixelsPerUnit = 0;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        private static void AddImage(GameObject fadeManager, Color color)
        {
            Image image = fadeManager.AddComponent<Image>();
            image.color = color;
        }

        private static void AddCanvasGroup(GameObject fadeManager)
        {
            CanvasGroup canvasGroup = fadeManager.AddComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
