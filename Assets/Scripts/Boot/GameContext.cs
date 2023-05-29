using Data;
using Data.SaveLoad;
using Drawing;
using Leveling;
using Sound;
using UI;
using UnityEngine;
using Zenject;

namespace Boot
{
    [CreateAssetMenu(menuName = "Create GameContext", fileName = "GameContext", order = 0)]
    public class GameContext : ScriptableObjectInstaller
    {
        [SerializeField] private DrawerConfig _drawerConfig;
        [SerializeField] private GameScreenPrefabs _gameScreen;
        [SerializeField] private uint _startLevelId;
        [SerializeField] private GameSounds _gameSounds;
        [SerializeField] private Level[] _levels;
        [SerializeField] private Skin[] _skins;
        [SerializeField] private DrawAsset[] _drawAssets;
    
        public override void InstallBindings()
        {
            Container.Bind<DrawerConfig>().FromInstance(_drawerConfig).AsSingle();
            Container.Bind<GameScreenPrefabs>().FromInstance(_gameScreen).AsSingle();
            Container.Bind<Level[]>().FromInstance(_levels).AsSingle();
            Container.Bind<Skin[]>().FromInstance(_skins).AsSingle();
            Container.Bind<DrawAsset[]>().FromInstance(_drawAssets).AsSingle();
            Container.Bind<Transform>().FromInstance(new GameObject("LevelParent").transform).AsSingle();
            Container.Bind<int>().FromInstance((int)_startLevelId).AsSingle();

            new Sound.Sound(_gameSounds);
            AdHelper.Init();
        }
    }
}