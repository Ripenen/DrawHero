using System;
using DG.Tweening;
using Leveling.Collisions;
using UnityEngine;

namespace Leveling
{
    public class MoveTrigger : Trigger
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _distance;
        [SerializeField] private float _duration;
        [SerializeField] private bool _localValue;
        [SerializeField] private bool _localDirection;
        [SerializeField] private bool _looped;
        [SerializeField] private bool _fromStart;

        private void Start()
        {
            if(_fromStart)
                OnTriggered();
        }

        public override void OnTriggered()
        {
            var position = transform.position;
            
            var direction = _localDirection ? transform.InverseTransformVector(_direction) : _direction;
            var newPos = direction * _distance + transform.position;

            transform.DOMove(newPos, _duration).OnComplete(() =>
            {
                if(_looped)
                    Move(position);
            });
        }

        private void Move(Vector3 pos)
        {
            var position = transform.position;

            transform.DOMove(pos, _duration).OnComplete(() =>
            {
                if(_looped)
                    Move(position);
            });
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}