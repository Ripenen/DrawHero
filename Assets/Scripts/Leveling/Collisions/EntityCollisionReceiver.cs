using JetBrains.Annotations;
using UnityEngine;

namespace Leveling.Collisions
{
    [RequireComponent(typeof(Entity))]
    public class EntityCollisionReceiver : CollisionReceiver
    {
        [SerializeField] private CollisionReceiverType _collisionReceiverType;
        [SerializeField] private float _massForKill;
    
        private Entity _entity;
        private LevelEntitiesContainer _notifyContainer;

        public Entity Entity => _entity;
        public CollisionReceiverType CollisionReceiverType => _collisionReceiverType;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }

        public override void OnCollide([CanBeNull] ICollisionProvider provider)
        {
            if (provider is not null && provider.Rigidbody && provider.Rigidbody.mass < _massForKill)
                return;

            if (_notifyContainer is not null && !_entity.Killed)
            {
                _notifyContainer.Notify();

                if (_collisionReceiverType == CollisionReceiverType.Enemy)
                    Sound.Sound.PlayEnemyDie();
                else
                    Sound.Sound.PlayHostageDie();
            }

            _entity.Kill();
        }

        public void SetNotifyContainer(LevelEntitiesContainer levelEntitiesContainer)
        {
            _notifyContainer = levelEntitiesContainer;
        }
    }

    public enum CollisionReceiverType
    {
        Enemy,
        Hostage
    }
}