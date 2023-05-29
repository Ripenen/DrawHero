using System;
using UnityEngine;

namespace Leveling
{
    public class Rotatable : MonoBehaviour
    {
        [SerializeField] private Vector3 _axis;
        [SerializeField] private float _angleSpeed;

        private void Update()
        {
            transform.Rotate(_axis, _angleSpeed);
        }
    }
}