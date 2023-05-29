using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Leveling.Collisions;

namespace Leveling
{
    public class LevelEntitiesContainer
    {
        private Action _onAllKilled;
        private Action _anyKilled;

        private int _allEnemiesCount;
        private int _notifyCount;

        private IEnumerable<Entity> _entities;

        public LevelEntitiesContainer(Action onAllKilled, Action onAnyKilled = null)
        {
            _onAllKilled = onAllKilled;
            _anyKilled = onAnyKilled;
        }
        
        public void Collect(Level level, CollisionReceiverType type)
        {
            var collisionReceivers = level.GetComponentsInChildren<EntityCollisionReceiver>()
                .Where(x => x.CollisionReceiverType == type).ToArray();

            _entities = collisionReceivers.Select(x => x.Entity);

            foreach (var collisionReceiver in collisionReceivers)
            {
                collisionReceiver.SetNotifyContainer(this);
            }

            _allEnemiesCount = collisionReceivers.Length;
        }

        public void ApplySkin(Skin skin)
        {
            foreach (var entity in _entities)
            {
                entity.ApplySkin(skin);
            }
        }

        public void Notify()
        {
            _notifyCount++;
            
            _anyKilled?.Invoke();
        
            if(_allEnemiesCount == _notifyCount)
            {
                _onAllKilled?.Invoke();
                _onAllKilled = null;
            }
        }
    }
}