using Assets;
using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

namespace UnityEngine
{
    public class AugmentationObject : MonoBehaviour
    {
        private Color _color;
        private Material _material = null;
        private TargetModel _target = null;
        private AudioSource _audioSource = null;
        private bool _initialize = false;
        private bool _show = false;
        private bool _showing = false;

        public string UniqueTargetId
        {
            get { return _target?.VuforiaId; }
        }

        private void Start()
        {
            _material = gameObject.GetComponent<Renderer>().material;
            _audioSource = gameObject.AddComponent<AudioSource>();

            ResetMaterial(_material);
        }

        private void Update()
        {
            if (_initialize)
            {
                ColorUtility.TryParseHtmlString(_target.HexColor, out _color);
                Texture mainTexture = GetMainTex(_target.PngBase64, _color);
                _material.SetTexture("_MainTex", mainTexture);
                _initialize = false;
            }
            if (_show & !_showing)
            {
                _showing = true;
                StopAllCoroutines();
                var start = StartAnimation(_material, _audioSource, _target.AudioBase64, _color);
                StartCoroutine(start);
            }
            else if (!_show && _showing)
            {
                _showing = false;
                StopAllCoroutines();
                StopAnimation();
            }
        }

        public void Initialize(TargetModel targetModel)
        {
            _target = targetModel;
            _initialize = true;
        }

        public void Show(bool show)
        {
            _show = show;
        }

        private IEnumerator StartAnimation(Material material, AudioSource audioSource, string contentAudio, Color color)
        {
            yield return StartCoroutine(SetAudioClip(audioSource, contentAudio));

            var offset = 0f;
            var barWidth = 15;
            var offsetLimit = -1 + (0.00805f * barWidth);
            var offsetSpeed = 1f / audioSource.clip.length;
            var waitForSeconds = audioSource.clip.length / 100f / 2f;

            audioSource.Play();

            for (int i = 0; i <= barWidth; i++)
            {
                material.SetTexture("_MainTexA", GetMainTextA(i, color));
                yield return new WaitForSeconds(waitForSeconds);
            }

            while (offset > offsetLimit)
            {
                offset -= Time.deltaTime * offsetSpeed;
                material.SetTextureOffset("_MainTexA", new Vector2(offset, 0));
                yield return new WaitForEndOfFrame();
            }

            for (int i = barWidth; i >= 0; i--)
            {
                material.SetTexture("_MainTexA", GetMainTextA(barWidth, color, barWidth - i));
                yield return new WaitForSeconds(waitForSeconds);
            }
        }

        private void StopAnimation()
        {
            _audioSource.Stop();
            ResetMaterial(_material);
        }

        private IEnumerator SetAudioClip(AudioSource audioSource, string audio)
        {
            AudioType audioType = AudioType.UNKNOWN;
            if (audio.Contains("data:audio/wav;base64,"))
            {
                audio = audio.Replace("data:audio/wav;base64,", string.Empty);
                audioType = AudioType.WAV;
            }
            else if (audio.Contains("data:audio/mp3;base64,"))
            {
                audioType = AudioType.MPEG;
                audio = audio.Replace("data:audio/mp3;base64,", string.Empty);
            }
            else if (audio.Contains("data:audio/mpeg;base64,"))
            {
                audioType = AudioType.MPEG;
                audio = audio.Replace("data:audio/mpeg;base64,", string.Empty);
            }

            string tempFile = Application.persistentDataPath + "/audioclip_bytes";
            byte[] rawData = Convert.FromBase64String(audio);
            File.WriteAllBytes(tempFile, rawData);

            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + tempFile, audioType);
            var openation = www.SendWebRequest();

            yield return new WaitUntil(() => openation.isDone);

            if (www.result == UnityWebRequest.Result.Success)
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }

        private void ResetMaterial(Material material)
        {
            material.SetTextureOffset("_MainTexA", new Vector2(0, 0));
            Texture mainTexture = GetMainTextA(0, Color.clear);
            material.SetTexture("_MainTexA", mainTexture);
        }

        private Texture GetMainTex(string image, Color color)
        {
            byte[] imageBytes = Convert.FromBase64String(image.Replace("data:image/png;base64,", ""));

            Texture2D texture = new Texture2D(128, 128);
            texture.LoadImage(imageBytes);

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    if (texture.GetPixel(x, y) != Color.clear) texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private Texture GetMainTextA(int width, Color color, int prefixTransparent = 0)
        {
            Texture2D texture = new Texture2D(128, 128);

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, x < width && x >= prefixTransparent ? color : Color.clear);
                }
            }

            texture.Apply();
            return texture;
        }
    }
}