using System;
using Data;
using Drawing;
using Eiko.YaSDK;
using Leveling;
using RayFire;
using UI;
using UnityEngine;
using DrawAsset = Data.DrawAsset;

namespace Processes
{
    public class LevelProcess
    {
        private readonly Drawer _drawer;
    
        private readonly GameUi _ui;
        private readonly PlayerData _playerData;

        private Level _level;
        private readonly Action<Level> _next;
        private readonly Action<Level> _reload;
        private readonly Action<Level> _backToMenu;

        private bool _skin;

        public LevelProcess(Camera camera, PlayerData data, DrawerConfig drawerConfig, GameUi ui, Action<Level> next, Action<Level> reload, Action<Level> backToMenu)
        {
            _ui = ui;

            _next = next;
            _reload = reload;
            _playerData = data;
            _backToMenu = backToMenu;
            
            _drawer = new Drawer(camera, drawerConfig);
        }

        public void Start(Level level)
        {
            level.SetSkin(_playerData.Skin);
            
            AdHelper.AddRewardHandler("Skip", () =>
            {
                AppMetricaWeb.Event("skipRoundAd");
                
                Dispose();

                _playerData.LastWinLevel = level.Id;
                
                _next?.Invoke(_level);
            });

            _ui.ShowMaxGameScreen(level.Id, () =>
            {
                Dispose();
                
                RayfireMan.inst = null;

                _reload?.Invoke(_level);
            }, () =>
            {

#if UNITY_WEBGL
                YandexSDK.Instance.ShowRewarded("Skip");
#elif UNITY_ANDROID
                Dispose();

                _playerData.LastWinLevel = level.Id;
                
                _next?.Invoke(_level);          
#endif
            }, () =>
            {
                Dispose();

                _backToMenu?.Invoke(level);
            }, _playerData);

            _drawer.Start(() =>
            {
                _drawer.CreateLineMesh(level);
                _drawer.Clear();
            }, level.DrawZoneRects, _playerData.DrawAsset);
        
            level.Win += OnLevelWin;
            level.Lose += OnLevelLose;
            _level = level;
        }

        private void Dispose()
        {
            _level.Win -= OnLevelWin;
            _level.Lose -= OnLevelLose;
            _drawer.Clear();
            _drawer.Dispose();
            _ui.Clear();
        }

        private void OnLevelLose()
        {
            Dispose();

            _ui.ShowLoseScreen(() =>
            {
                RayfireMan.inst = null;
                _reload.Invoke(_level);
            });
            
#if UNITY_WEBGL
                YandexSDK.instance.ShowInterstitial();
#endif
            Sound.Sound.PlayLose();
        }

        private void OnLevelWin()
        {
            _level.Win -= OnLevelWin;

            _drawer.Clear();
            _drawer.Dispose();
            
            _ui.DisableBackButton();

            _ui.ShowWinScreenBackground(() =>
            {
                _level.DisableUi();
#if UNITY_WEBGL
                YandexSDK.instance.ShowInterstitial();
#endif
                if(_playerData.LastWinLevel < _level.Id)
                    _playerData.SkinProgress += 0.1f;

                _playerData.LastWinLevel = _level.Id;
                
                Sound.Sound.PlayWin();
                
                if (_playerData.SkinProgress >= 1)
                {
                    _playerData.SkinProgress = 0;

                    if (_skin)
                    {
                        OpenRandomDrawAsset();
                        _skin = false;
                    }
                    else
                    {
                        OpenRandomSkin();
                        _skin = true;
                    }
                }
                else
                {
                    ShowWinScreen();
                }
            });
        }

        private bool OpenRandomSkin(bool react = true)
        {
            Skin skinForOpen = _playerData.GetRandomNotOpenedSkin();

            if (skinForOpen is null)
            {
                if (react && OpenRandomDrawAsset(false))
                    return true;
                
                if(react)
                    ShowWinScreen();

                return false;
            }
            
            _ui.ShowSkinPreviewScreen(skinForOpen, () =>
            {
                _ui.Clear();

                _playerData.OpenedSkins.Add(skinForOpen);
                _playerData.Skin = skinForOpen;

                _level.Lose -= OnLevelLose;

                _next.Invoke(_level);
            }, () =>
            {
                _ui.Clear();

                _level.Lose -= OnLevelLose;

                _next.Invoke(_level);
            });

            return true;
        }
        
        private bool OpenRandomDrawAsset(bool react = true)
        {
            DrawAsset drawAsset = _playerData.GetRandomNotOpenedDrawAsset();

            if (drawAsset is null)
            {
                if (react && OpenRandomSkin(false))
                    return true;
                
                if(react)
                    ShowWinScreen();

                return false;
            }
            
            _ui.ShowDrawAssetPreviewScreen(drawAsset, () =>
            {
                _ui.Clear();

                _playerData.OpenedDrawAssets.Add(drawAsset);
                _playerData.DrawAsset = drawAsset;

                _level.Lose -= OnLevelLose;

                _next.Invoke(_level);
            }, () =>
            {
                _ui.Clear();

                _level.Lose -= OnLevelLose;

                _next.Invoke(_level);
            });

            return true;
        }

        private void ShowWinScreen()
        {
            _ui.ShowWinScreen(() =>
            {
                _level.Lose -= OnLevelLose;

                _next.Invoke(_level);
            }, _playerData.SkinProgress, _playerData);
        }
    }
}