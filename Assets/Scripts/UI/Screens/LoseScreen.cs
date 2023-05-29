using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LoseScreen : Screen
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _fadeTime;

        public void Construct(Action onEnd)
        {
            _image.color = new Color(0, 0, 0, 0);
        
            _image.DOColor(Color.black, _fadeTime).OnComplete(() =>
            {
                onEnd?.Invoke();
                Destroy();
            });
        }

        private void OnDestroy()
        {
            _image.DOKill();
        }
    }
}