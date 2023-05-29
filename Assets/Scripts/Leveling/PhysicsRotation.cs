using UnityEngine;

namespace Leveling
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsRotation : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        [SerializeField] private Vector3 _axis;
        [SerializeField] private float _angleSpeed;

        [SerializeField] private float _angle;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _angle += _angleSpeed;

            _rigidbody.MoveRotation(Quaternion.AngleAxis(_angle, _axis));
        }
    }
}