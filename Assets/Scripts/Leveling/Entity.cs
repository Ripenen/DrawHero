using Data;
using UnityEngine;
using UnityEngine.Events;

namespace Leveling
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SkinApplier _applier;
        [SerializeField] private Collider _collider;
        [SerializeField] private UnityEvent _onDied;
        
        public bool Killed => !_animator.enabled;

        public void ApplySkin(Skin skin)
        {
            _applier.Apply(skin);
            SetRagDollState(false);
        }

        public void Kill()
        {
            SetRagDollState(true);
            _animator.enabled = false;
            _collider.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            
            _applier.Active.Died.Play();
            _applier.Active.Smile.gameObject.SetActive(true);
            
            _onDied?.Invoke();
        }

        private void SetRagDollState(bool value)
        {
            foreach (var rigidbody in _applier.RagDoll)
            {
                rigidbody.isKinematic = !value;
            }
        }
    }
}