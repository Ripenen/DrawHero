using UnityEngine;
using Screen = UI.Screens.Screen;

namespace UI
{
    public class UiFactory
    {
        private readonly GameScreenPrefabs _screens;
        private readonly Canvas _parent;

        public UiFactory(GameScreenPrefabs screens, Canvas parent)
        {
            _screens = screens;
            _parent = parent;
        }

        public void DestroyIfExist<T>() where T : Screen
        {
            var screen = _parent.GetComponentInChildren<T>();
            
            if(screen)
                screen.Destroy();
        }

        public T Create<T>() where T : Screen
        {
            return Object.Instantiate(_screens.GetByType<T>(), _parent.transform);
        }

        public void BeCameraSpace()
        {
            _parent.renderMode = RenderMode.ScreenSpaceCamera;
            _parent.worldCamera = Camera.main;
        }

        public void BeOverlay()
        {
            _parent.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
}