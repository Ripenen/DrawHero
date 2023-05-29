using UnityEngine;

namespace Leveling.Collisions
{
    public class CollisionProvider : MonoBehaviour, ICollisionProvider
    {
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();

            if (Rigidbody && Rigidbody.mass <= 1)
                Rigidbody.mass = 31;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.TryGetComponent<CollisionReceiver>(out var receiver))
                receiver.OnCollide(this);
        }
    }

    public interface ICollisionProvider
    {
        public Rigidbody Rigidbody { get; }
    }
}