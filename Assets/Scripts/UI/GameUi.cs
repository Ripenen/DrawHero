using System;
using Data;
using Leveling;
using Processes;
using UI.Screens;
using UniRx;

namespace UI
{
    public class GameUi
    {
        private readonly UiFactory _factory;

        private GameScreen _gameScreen;
        private IDisposable _subscription;

        public GameUi(UiFactory factory)
        {
            _factory = factory;
        }
    
        public void ShowTapWaitScreen(Action onTap, Action<Skin> onSkinChange, Action<DrawAsset> onDrawAssetChanged, PlayerData playerData)
        {
            var tapWait = _factory.Create<TapWaitScreen>();
        
            tapWait.Construct(() =>
            {
                onTap?.Invoke();
                tapWait.Destroy();
            }, () =>
            {
                var skinChangeScreen = _factory.Create<SkinChangeScreen>();
                skinChangeScreen.Construct(onSkinChange, onDrawAssetChanged, playerData);
            }, playerData);
        }

        public void ShowMiniGameScreen(int levelId, PlayerData data)
        {
            if (_gameScreen)
            {
                _gameScreen.ShowMiniVersion();
                return;
            }
            _gameScreen = _factory.Create<GameScreen>();
        
            _gameScreen.Construct(levelId, null, null, null, data);
            _gameScreen.ShowMiniVersion();
        }
    
        public void ShowMaxGameScreen(int levelId, Action onReload, Action onSkip, Action onBack, PlayerData data)
        {
            if (!_gameScreen)
                throw new InvalidOperationException();
        
            _gameScreen.Construct(levelId, onReload, onSkip, onBack, data);
        
            _gameScreen.ShowMaxVersion();
        }

        public void Clear()
        {
            _factory.DestroyIfExist<WinScreenBackground>();
            _factory.DestroyIfExist<WinScreen>();
            _factory.DestroyIfExist<SkinPreviewScreen>();

            _subscription?.Dispose();
            
            _factory.BeOverlay();
        }

        public void ShowWinScreenBackground(Action onShown)
        {
            _subscription = Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
            {
                _factory.Create<WinScreenBackground>();

                onShown?.Invoke();

                _factory.BeCameraSpace();
            });
        }

        public void ShowWinScreen(Action onContinueClicked, float skinProgress, PlayerData data)
        {
            var winScreen = _factory.Create<WinScreen>();

            winScreen.Construct(() =>
            {
                Clear();
                onContinueClicked?.Invoke();
            }, skinProgress, data);
        }

        public void ShowSkinPreviewScreen(Skin skin, Action onGetClicked, Action refuseClicked)
        {
            var skinPreviewScreen = _factory.Create<SkinPreviewScreen>();
            
            skinPreviewScreen.Construct(skin, onGetClicked, refuseClicked);
        }
        
        public void ShowDrawAssetPreviewScreen(DrawAsset asset, Action onGetClicked, Action refuseClicked)
        {
            var skinPreviewScreen = _factory.Create<SkinPreviewScreen>();
            
            skinPreviewScreen.Construct(asset, onGetClicked, refuseClicked);
        }

        public void ShowLoseScreen(Action onEnd)
        {
            var loseScreen = _factory.Create<LoseScreen>();
        
            loseScreen.Construct(onEnd);
        }

        public void DisableBackButton()
        {
            if (_gameScreen)
                _gameScreen.DisableBackButton();
        }
    }
}