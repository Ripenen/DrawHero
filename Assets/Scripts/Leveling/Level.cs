using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Drawing;
using Leveling.Collisions;
using UnityEngine;

namespace Leveling
{
    public class Level : MonoBehaviour
    {
        private LevelEntitiesContainer _enemies;
        private Canvas _canvas;
        
        public event Action Win;
        public event Action Lose;
    
        public int Id { get; private set; }

        public IEnumerable<Rect> DrawZoneRects => _canvas.GetComponentsInChildren<DrawZone>().Select(x => x.ScreenRect);

        public void Init(int id)
        {
            Id = id;
            
            _enemies = new LevelEntitiesContainer(() => Win?.Invoke());
            
            _enemies.Collect(this, CollisionReceiverType.Enemy);
            
            new LevelEntitiesContainer(null, () => Lose?.Invoke()).Collect(this, CollisionReceiverType.Hostage);

            _canvas = transform.GetComponentInChildren<Canvas>();
        }

        public void SetSkin(Skin skin)
        {
            _enemies.ApplySkin(skin);
        }

        public void DisableUi()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}