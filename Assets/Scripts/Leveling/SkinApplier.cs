using System.Collections;
using System.Collections.Generic;
using Data;
using UniRx;
using UnityEngine;

namespace Leveling
{
    public class SkinApplier : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SkinView _active;

        public IEnumerable<Rigidbody> RagDoll => _active.RagDoll;
        public SkinView Active => _active;

        public void Apply(Skin skin)
        {
            DestroyImmediate(_active.gameObject);

            _active = Instantiate(skin.View, transform);

            StartCoroutine(OneFrame());

            IEnumerator OneFrame()
            {
                yield return new WaitForEndOfFrame();

                _animator.Rebind();
            }
        }
    }
}