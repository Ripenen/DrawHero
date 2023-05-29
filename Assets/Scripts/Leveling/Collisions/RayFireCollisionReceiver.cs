using RayFire;
using UnityEngine;

namespace Leveling.Collisions
{
    public class RayFireCollisionReceiver : CollisionReceiver
    {
        [SerializeField] private RayfireRigid _rayFire;
        
        public override void OnCollide(ICollisionProvider provider)
        {
            _rayFire.Demolish();
        }
    }
}