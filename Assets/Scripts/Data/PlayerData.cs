using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Data
{
    public class PlayerData
    {
        public Skin Skin;
        public DrawAsset DrawAsset;
        public float SkinProgress;
        public bool NoAdsBuy;
        public int LastWinLevel;
        
        public readonly List<Skin> OpenedSkins = new();
        public readonly List<DrawAsset> OpenedDrawAssets = new();
        
        private readonly List<Skin> _allSkins = new ();
        private readonly List<DrawAsset> _drawAssets = new ();

        public PlayerData(Skin defaultSkin, DrawAsset defaultAsset, IEnumerable<Skin> allSkins, IEnumerable<DrawAsset> allDrawAssets)
        {
            Skin = defaultSkin;
            DrawAsset = defaultAsset;
            
            OpenedSkins.Add(defaultSkin);
            OpenedDrawAssets.Add(defaultAsset);
            
            _allSkins.AddRange(allSkins);
            _drawAssets.AddRange(allDrawAssets);
        }
        
        public PlayerData(Skin defaultSkin, DrawAsset defaultAsset, IEnumerable<Skin> openedSkins, IEnumerable<DrawAsset> openedAssets, IEnumerable<Skin> allSkins, IEnumerable<DrawAsset> allDrawAssets)
        {
            Skin = defaultSkin;
            DrawAsset = defaultAsset;
            
            OpenedSkins.AddRange(openedSkins);
            OpenedDrawAssets.AddRange(openedAssets);

            _allSkins.AddRange(allSkins);
            _drawAssets.AddRange(allDrawAssets);
        }

        public Skin GetRandomNotOpenedSkin()
        {
            var id = Random.Range(0, _allSkins.Count);
            
            for (int i = 0; OpenedSkins.Contains(_allSkins[id]); i++)
            {
                id = Random.Range(0, _allSkins.Count);

                if (i >= 10)
                    return null;
            }

            return _allSkins[id];
        }
        
        public DrawAsset GetRandomNotOpenedDrawAsset()
        {
            var id = Random.Range(0, _drawAssets.Count);
            
            for (int i = 0; OpenedDrawAssets.Contains(_drawAssets[id]); i++)
            {
                id = Random.Range(0, _drawAssets.Count);

                if (i >= 10)
                    return null;
            }

            return _drawAssets[id];
        }
    }
}