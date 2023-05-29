using System;
using UI.Screens;
using UnityEngine;
using Screen = UI.Screens.Screen;

namespace UI
{
    [CreateAssetMenu(menuName = "Create GameScreenPrefabs", fileName = "GameScreenPrefabs", order = 0)]
    public class GameScreenPrefabs : ScriptableObject
    {
        [SerializeField] private TapWaitScreen _tapWaitScreen;
        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private WinScreenBackground _winScreenBackground;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private SkinChangeScreen _skinChangeScreen;
        [SerializeField] private SkinPreviewScreen _skinPreviewScreen;
    
        public T GetByType<T>() where T : Screen
        {
            return typeof(T).Name switch
            {
                nameof(TapWaitScreen) => _tapWaitScreen as T,
                nameof(GameScreen) => _gameScreen as T,
                nameof(WinScreenBackground) => _winScreenBackground as T,
                nameof(LoseScreen) => _loseScreen as T,
                nameof(SkinChangeScreen) => _skinChangeScreen as T,
                nameof(SkinPreviewScreen) => _skinPreviewScreen as T,
                nameof(WinScreen) => _winScreen as T,
                _ => throw new ArgumentException()
            };
        }
    }
}