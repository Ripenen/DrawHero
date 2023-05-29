using Data;
using Data.SaveLoad;
using Drawing;
using Leveling;
using UI;
using UnityEngine;

namespace Processes
{
    public class GameProcess
    {
        private readonly GameUi _ui;
        private readonly LevelFactory _levelFactory;
        private readonly LevelProcess _levelProcess;
        private readonly PlayerData _playerData;

        public GameProcess(GameUi ui, LevelFactory levelFactory, DrawerConfig config, Camera camera, PlayerData playerData)
        {
            _ui = ui;
            _levelFactory = levelFactory;
            _playerData = playerData;

            _levelProcess = new LevelProcess(camera, playerData, config, _ui, level => LoadNextLevel(level.Id, level),
                level => LoadNextLevel(level.Id - 1, level), BackToMenu);
        }

        public void Start(int startLevelId)
        {
            CreateLevel(startLevelId);
            
            Sound.Sound.PlayBackground();
        }

        private void CreateLevel(int startLevelId)
        {
            Level level = _levelFactory.Create(startLevelId);

            level.SetSkin(_playerData.Skin);

            _ui.ShowMiniGameScreen(level.Id, _playerData);

            _ui.ShowTapWaitScreen(() => _levelProcess.Start(level), skin =>
            {
                _playerData.Skin = skin;
                level.SetSkin(skin);
            }, draw => { _playerData.DrawAsset = draw; }, _playerData);
        }

        private void LoadNextLevel(int currentId, Level previousLevel)
        {
            Object.Destroy(previousLevel.gameObject);
        
            Level level = _levelFactory.Create(currentId + 1);
            
            _levelProcess.Start(level);

            SaveLoadPlayerData.Save(_playerData);
        }

        private void BackToMenu(Level previousLevel)
        {
            Object.Destroy(previousLevel.gameObject);
            
            CreateLevel(previousLevel.Id);
        }
    }
}