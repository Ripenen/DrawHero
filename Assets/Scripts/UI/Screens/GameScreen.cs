using System;
using Data;
using Eiko.YaSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameScreen : Screen
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _reloadButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _noAds;

        private PlayerData _data;

        public void Construct(int levelId, Action reloadClicked, Action skipClicked, Action backClicked, PlayerData data)
        {
            _levelText.text = (YandexSDK.instance.Lang == "ru" ? "Уровень " : "Level ") + levelId;

            _data = data;
            
            _reloadButton.onClick.RemoveAllListeners();
            _skipButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _noAds.onClick.RemoveAllListeners();
            
            _reloadButton.onClick.AddListener(() =>
            {
                AppMetricaWeb.Event("resetLvl");
                reloadClicked?.Invoke();
            });
            
            _skipButton.onClick.AddListener(() => skipClicked?.Invoke());
            _backButton.onClick.AddListener(() => backClicked?.Invoke());
            
            _reloadButton.onClick.AddListener(Sound.Sound.PlayClick);
            _skipButton.onClick.AddListener(Sound.Sound.PlayClick);
            _backButton.onClick.AddListener(Sound.Sound.PlayClick);
            
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

        public void ShowMiniVersion()
        {
            _reloadButton.gameObject.SetActive(false);
            _skipButton.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _noAds.gameObject.SetActive(false);
        }
    
        public void ShowMaxVersion()
        {
            _reloadButton.gameObject.SetActive(true);
            _skipButton.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
            
            if(!_data.NoAdsBuy)
                _noAds.gameObject.SetActive(true);
        }

        public void DisableBackButton()
        {
            _backButton.gameObject.SetActive(false);
            _skipButton.gameObject.SetActive(false);
            _noAds.gameObject.SetActive(false);
        }
    }
}