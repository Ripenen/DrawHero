using System;
using Data;
using Data.SaveLoad;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Screens
{
    public class TapWaitScreen : Screen, IPointerClickHandler
    {
        [SerializeField] private Button _skins;
        [SerializeField] private Button _noAds;
        
        private Action _onTap;

        public void Construct(Action tap, Action skinsClicked, PlayerData data)
        {
            _onTap = tap;
            _skins.onClick.RemoveAllListeners();
            _noAds.onClick.RemoveAllListeners();
            
            _skins.onClick.AddListener(() => skinsClicked?.Invoke());

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
                    
                    SaveLoadPlayerData.Save(data);
                });
            });
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            _onTap?.Invoke();
        }
    }
}