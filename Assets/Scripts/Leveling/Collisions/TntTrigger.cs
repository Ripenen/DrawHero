using Processes;
using UnityEngine;

namespace Leveling.Collisions
{
    public class TntTrigger : Trigger
    {
        [SerializeField] private float _radius;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private float _force;

        public override void OnTriggered()
        {
            var inRadius = Physics.OverlapSphere(transform.position, _radius);

            foreach (var inRadiusCollider in inRadius)
            {
                if(inRadiusCollider.TryGetComponent<EntityCollisionReceiver>(out var collisionReceiver))
                    collisionReceiver.OnCollide(null);

                if(!inRadiusCollider.attachedRigidbody)
                    continue;

                inRadiusCollider.attachedRigidbody.AddForce((inRadiusCollider.transform.position - transform.position) * _force);
            }

            _explosion.transform.parent = null;
            _explosion.Play();
            Sound.Sound.PlayExplosion();
            Destroy(_explosion.gameObject, _explosion.main.duration);
        
            Destroy(gameObject);
        }
    }
}