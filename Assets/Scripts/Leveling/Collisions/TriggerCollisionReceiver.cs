using System.Collections.Generic;
using UnityEngine;

namespace Leveling.Collisions
{
    public class TriggerCollisionReceiver : CollisionReceiver
    {
        [SerializeField] private List<Trigger> _triggers;
        [SerializeField] private float _massForInteract;
    
        public override void OnCollide(ICollisionProvider provider)
        {
            if(provider.Rigidbody.mass < _massForInteract)
                return;
            
            foreach (var trigger in _triggers)
            {       
                trigger.OnTriggered();
            }
        
            Destroy(this);
        }
    }
}