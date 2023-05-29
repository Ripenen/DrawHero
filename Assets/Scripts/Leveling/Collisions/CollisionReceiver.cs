using UnityEngine;

namespace Leveling.Collisions
{
    public abstract class CollisionReceiver : MonoBehaviour
    {
        public abstract void OnCollide(ICollisionProvider provider);
    }
}