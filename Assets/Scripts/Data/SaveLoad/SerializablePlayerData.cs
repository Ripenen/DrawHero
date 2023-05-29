using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.SaveLoad
{
    [Serializable]
    public class SerializablePlayerData
    {
        public int SelectedSkinId;
        public int SelectedDrawAssetId;
        public float SkinProgress;
        public int LastWinLevel;
        public bool NoAds;
        
        public List<int> OpenedSkinsId;
        public List<int> OpenedDrawAssetsId;
        
        public SerializablePlayerData(PlayerData data)
        {
            SelectedSkinId = data.Skin.ID;
            SelectedDrawAssetId = data.DrawAsset.ID;
            
            SkinProgress = data.SkinProgress;
            LastWinLevel = data.LastWinLevel;

            NoAds = data.NoAdsBuy;

            OpenedSkinsId = data.OpenedSkins.Select(x => x.ID).ToList();
            OpenedDrawAssetsId = data.OpenedDrawAssets.Select(x => x.ID).ToList();
        }
    }
}