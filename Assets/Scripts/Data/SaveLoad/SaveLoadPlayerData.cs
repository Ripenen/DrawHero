using System.Linq;
using Eiko.YaSDK.Data;
using UnityEngine;

namespace Data.SaveLoad
{
    public static class SaveLoadPlayerData
    {
        private const string SaveKey = "save";

        public static void Save(PlayerData playerData)
        { 
            YandexPrefs.SetString(SaveKey, JsonUtility.ToJson(new SerializablePlayerData(playerData)));
        }

        public static void Delete()
        {
            YandexPrefs.SetString(SaveKey, string.Empty);
        }

        public static PlayerData Load(Skin[] skins, DrawAsset[] assets)
        {
            var save = YandexPrefs.GetString(SaveKey);
            
            if(save == string.Empty)
                return new PlayerData(skins[0], assets[0], skins, assets);

            var data = JsonUtility.FromJson<SerializablePlayerData>(save);

            var skin = skins.First(x => x.ID == data.SelectedSkinId);
            var drawAsset = assets.First(x => x.ID == data.SelectedDrawAssetId);

            var openedSkins = skins.Where(x => data.OpenedSkinsId.Contains(x.ID));
            var openedAssets = assets.Where(x =>  data.OpenedDrawAssetsId.Contains(x.ID));
            
            var playerData = new PlayerData(skin, drawAsset, openedSkins, openedAssets, skins, assets)
            {
                SkinProgress = data.SkinProgress,
                LastWinLevel = data.LastWinLevel,
                NoAdsBuy = data.NoAds,
            };

            return playerData;
        }
    }
}