using UnityEngine;

namespace Leveling.Collisions
{
    public class Pusher : MonoBehaviour
    {
        [SerializeField] private float _force;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody)
            {
                collision.rigidbody.AddForce((transform.position - collision.transform.position) * _force, ForceMode.Force);
            }            
        }
    }
}