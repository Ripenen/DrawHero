using System;
using Data;
using Eiko.YaSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class WinScreen : Screen
    {
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _noAds;
        [SerializeField] private Slider _skinSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        
        public void Construct(Action onNextClicked, float sliderProgress, PlayerData data)
        {
            _nextLevel.onClick.AddListener(() =>
            {
                AppMetricaWeb.Event("nextLvl");
                onNextClicked?.Invoke();
            });

            _skinSlider.value = sliderProgress;
            _progressText.text = Mathf.RoundToInt(sliderProgress * 100) + "%";
            
            if (data.NoAdsBuy)
            {
                _noAds.gameObject.SetActive(false);
                YandexSDK.Instance.AdsOff();
            }
            
            _noAds.onClick.AddListener(() =>
            {
                PurchaseProcess.instance.ProcessPurchase("NoAds", () =>
                {
                    YandexSDK.Instance.AdsOff();
                    _noAds.gameObject.SetActive(false);
                    data.NoAdsBuy = true;
                });
            });
        }
    }
}