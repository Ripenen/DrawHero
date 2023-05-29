using System;
using System.Collections.Generic;
using Eiko.YaSDK;

namespace UI
{
    public static class AdHelper
    {
        private static readonly Dictionary<string, Action> Handlers = new();

        public static void Init()
        {
            YandexSDK.Instance.onRewardedAdReward += HandleReward;
        }

        private static void HandleReward(string obj)
        {
            Handlers[obj]?.Invoke();
        }
    
        public static void AddRewardHandler(string name, Action handler)
        {
            if(Handlers.ContainsKey(name))
                Handlers.Remove(name);
        
            Handlers.Add(name, handler);
        }
    }
}