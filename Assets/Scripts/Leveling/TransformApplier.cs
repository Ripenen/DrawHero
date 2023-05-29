using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leveling
{
    public class TransformApplier : MonoBehaviour
    {
        [SerializeField] private Transform _transformForApply;
        [SerializeField] private Vector3Int _positionConstraints;
        
        private Quaternion _savedRotation;

        private void Start()
        {
            _savedRotation = transform.rotation;
        }

        private void Update()
        {
            Transform transform1 = transform;
            
            transform1.position = Vector3.Scale(transform1.position, _positionConstraints) +
                                  Vector3.Scale(_transformForApply.position, Xor(_positionConstraints));

            transform1.rotation = _savedRotation;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3Int Xor(Vector3Int vector)
        {
            for (int i = 0; i < 3; i++)
            {
                if (vector[i] >= 1)
                    vector[i] = 0;
                else
                    vector[i] = 1;

            }

            return vector;
        }
    }
}