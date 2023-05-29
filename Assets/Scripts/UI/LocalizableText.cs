using Eiko.YaSDK;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizableText : MonoBehaviour
    {
        [SerializeField] private string _ruText;
        [SerializeField] private string _enText;
    
        private void Start()
        {
            var text = YandexSDK.instance.Lang == "ru" ? _ruText : _enText;

            GetComponent<TextMeshProUGUI>().text = text;
        }

        public void SetText(string ruText, string enText)
        {
            _ruText = ruText;
            _enText = enText;
        }
    }
}