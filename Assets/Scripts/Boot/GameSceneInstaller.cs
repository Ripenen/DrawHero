using System.Collections;
using Data;
using Data.SaveLoad;
using Drawing;
using Eiko.YaSDK.Data;
using Leveling;
using UI;
using UnityEngine;
using Zenject;

namespace Boot
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
    
        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            Container.Bind<UiFactory>().AsSingle();
            Container.Bind<LevelFactory>().AsSingle();
            Container.Bind<GameUi>().AsSingle();

            StartCoroutine(StartD());
        }
        
        private IEnumerator StartD()
        {
            var process = new PurchaseProcess(); 
            
            yield return process.InitPurchases();
            yield return YandexPrefs.Init();
            
            //SaveLoadPlayerData.Delete();
            
            var d= SaveLoadPlayerData.Load(Container.Resolve<Skin[]>(), Container.Resolve<DrawAsset[]>());
            
            Container.Bind<PlayerData>().FromInstance(d).AsSingle();
            
            new Bootstrap(Container.Resolve<DrawerConfig>(), Container.Resolve<LevelFactory>(), Container.Resolve<GameUi>(),
                Container.Resolve<int>(), d).Initialize();
        }
    }
}