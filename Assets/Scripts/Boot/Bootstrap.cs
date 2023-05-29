using Data;
using Drawing;
using Leveling;
using Processes;
using UI;
using UnityEngine;
using Zenject;

namespace Boot
{
    public class Bootstrap : IInitializable
    {
        private readonly DrawerConfig _config;
        private readonly LevelFactory _levelFactory;
        private readonly GameUi _ui;
        private readonly int _startLevel;
        private readonly PlayerData _playerData;

        public Bootstrap(DrawerConfig config, LevelFactory levelFactory, GameUi ui, int startLevel, PlayerData playerData)
        {
            _config = config;
            _levelFactory = levelFactory;
            _ui = ui;
            _playerData = playerData;
            
#if !UNITY_EDITOR
            _startLevel = playerData.LastWinLevel + 1;
#else
            _startLevel = startLevel;
#endif
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;

            AppMetricaWeb.Event("play");
            
            new GameProcess(_ui, _levelFactory, _config, Camera.main, _playerData).Start(_startLevel);
        }
    }
}