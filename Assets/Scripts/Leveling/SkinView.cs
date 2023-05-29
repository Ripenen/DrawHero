using System.Collections.Generic;
using UnityEngine;

namespace Leveling
{
    public class SkinView : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _ragDoll;
        [SerializeField] private ParticleSystem _died;
        [SerializeField] private TransformApplier _smile;
        public IEnumerable<Rigidbody> RagDoll => _ragDoll;

        public ParticleSystem Died => _died;

        public TransformApplier Smile => _smile;
    }
}