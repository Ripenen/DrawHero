using System;
using System.Collections.Generic;
using Data;
using Data.SaveLoad;
using Eiko.YaSDK;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = UI.Screens.Screen;

namespace UI
{
    public class SkinChangeScreen : Screen
    {
        [SerializeField] private Button _enemiesSkins;
        [SerializeField] private Button _drawerSkins;
        [SerializeField] private Button _openAll;
        [SerializeField] private Button _back;

        [SerializeField] private TextMeshProUGUI _categoryText;

        [SerializeField] private SkinChangeButton[] _skins;
        [SerializeField] private DrawAssetChangeButton[] _drawAssetChangeButtons;

        private PlayerData _playerData;

        public void Construct(Action<Skin> skinSelected, Action<DrawAsset> drawAssetSelected, PlayerData playerData)
        {
            _playerData = playerData;
            
            ConstructChangeButtons(_skins, playerData.Skin, playerData.OpenedSkins, skinSelected, "Skin");
            ConstructChangeButtons(_drawAssetChangeButtons, playerData.DrawAsset, playerData.OpenedDrawAssets, drawAssetSelected, "DrawAsset");

            _back.onClick.AddListener(() => Destroy(gameObject));
            
            _enemiesSkins.onClick.AddListener(() => ChangeView(true, false));
            _drawerSkins.onClick.AddListener(() => ChangeView(false, true));
            
            if(_skins.Length == playerData.OpenedSkins.Count && _drawAssetChangeButtons.Length == playerData.OpenedDrawAssets.Count)
                _openAll.gameObject.SetActive(false);

            _openAll.onClick.AddListener(() =>
            {
                PurchaseProcess.instance.ProcessPurchase("OpenAllInShop", () =>
                {
                    foreach (var skin in _skins)
                    {
                        if(!playerData.OpenedSkins.Contains(skin.Element))
                            playerData.OpenedSkins.Add(skin.Element);
                        
                        skin.SetInteractable(true);
                        
                        if (skin.Buy)
                            skin.Buy.gameObject.SetActive(false);
                    }
                    
                    foreach (var skin in _drawAssetChangeButtons)
                    {
                        if(!playerData.OpenedDrawAssets.Contains(skin.Element))
                            playerData.OpenedDrawAssets.Add(skin.Element);
                        
                        skin.SetInteractable(true);
                        
                        if (skin.Buy)
                            skin.Buy.gameObject.SetActive(false);
                    }
                    
                    _openAll.gameObject.SetActive(false);
                    SaveLoadPlayerData.Save(playerData);
                });
            });
        }
        
        
        private void ConstructChangeButtons<TButton, TElement>(TButton[] buttons, TElement selected, ICollection<TElement> opened, Action<TElement> onSelected, string typeName) where TButton : ActionButton<TElement>
        {
            foreach (var button in buttons)
            {
                AdHelper.AddRewardHandler(button.Element.GetType().Name + button.gameObject.name, () =>
                {
                    AppMetricaWeb.Event(typeName == "Skin" ? "buyEnemySkinAd" : "buyPencilAd");

                    SelectElement(buttons, opened, onSelected, button);
                    SaveLoadPlayerData.Save(_playerData);
                });
                
                button.SetOnClick(element =>
                {
                    if (button.Open)
                    {
                        foreach (var button in buttons)
                        {
                            button.SetScale(1);
                        }

                        button.SetScale(1.15f);
                        
                        onSelected?.Invoke(element);
                    }
                    else
                    {
#if UNITY_WEBGL
                        YandexSDK.instance.ShowRewarded(button.Element.GetType().Name + button.gameObject.name);

#elif UNITY_ANDROID
                        SelectElement(buttons, opened, onSelected, button);      
#endif
                    }
                });
                
                if(button.Element.Equals(selected))
                    button.SetScale(1.15f);

                if (opened.Contains(button.Element))
                {
                    button.SetInteractable(true);
                    
                    if(!button.Buy)
                        continue;
                    
                    button.Buy.gameObject.SetActive(false);
                }
                else
                {
                    button.SetInteractable(false);
                }
                
                if(!button.Buy)
                    continue;
                
                button.Buy.onClick.AddListener(() =>
                {
                    PurchaseProcess.instance.ProcessPurchase("Buy" + typeName + (buttons.IndexOf(button) - 1),
                        () =>
                        {
                            SelectElement(buttons, opened, onSelected, button);
                            SaveLoadPlayerData.Save(_playerData);
                        });
                });
            }
        }

        private void SelectElement<TButton, TElement>(TButton[] buttons, ICollection<TElement> opened, Action<TElement> selected,
            TButton button) where TButton : ActionButton<TElement>
        {
            button.SetInteractable(true);
            selected?.Invoke(button.Element);
            opened.Add(button.Element);

            foreach (var button1 in buttons)
            {
                button1.SetScale(1);
            }

            if(_skins.Length == _playerData.OpenedSkins.Count && _drawAssetChangeButtons.Length == _playerData.OpenedDrawAssets.Count)
                _openAll.gameObject.SetActive(false);
            
            button.SetScale(1.15f);

            if (!button.Buy)
                return;

            button.Buy.gameObject.SetActive(false);
        }

        private void ChangeView(bool enemiesSkinsActive, bool drawAssetsActive)
        {
            _categoryText.text = enemiesSkinsActive ? YandexSDK.instance.Lang == "ru" ? "Враг" : "Enemy" 
                : YandexSDK.instance.Lang == "ru" ? "Карандаш" : "Painter";

            foreach (var skinChangeButton in _skins) 
                skinChangeButton.gameObject.SetActive(enemiesSkinsActive);

            foreach (var drawAssetChangeButton in _drawAssetChangeButtons)
                drawAssetChangeButton.gameObject.SetActive(drawAssetsActive);
        }
    }
}