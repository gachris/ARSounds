using UnityEngine.UI;

namespace UnityEngine
{
    public sealed class LoadingController : MonoBehaviour
    {
        public GameObject ProgressSpinner;
        public Text Text;
        public float RotationSpeed;

        private void Update()
        {
            ProgressSpinner.GetComponent<RectTransform>().Rotate(0f, 0f, RotationSpeed * Time.deltaTime);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(string message)
        {
            gameObject.SetActive(true);
            Text.text = message;
        }

        public void Hide()
        {
            Text.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}