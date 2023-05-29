using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI
{
    public abstract class ActionButton<T> : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Button _buy;
        [SerializeField] private T _element;
        [SerializeField] private Color _active = Color.white;
        [SerializeField] private TextMeshProUGUI _text;

        public T Element => _element;
        
        public bool Open { get; private set; }

        public Button Buy => _buy;

        protected virtual void OnClick(){}
        protected virtual void OnInteractableChange(bool value) {}

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void SetOnClick(Action<T> action)
        {
            _button.onClick.AddListener(() => action?.Invoke(_element));
        }

        public void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public void SetInteractable(bool value)
        {
            Open = value;
            
            var childrenGraphics = GetComponentsInChildren<Graphic>();

            foreach (var childrenGraphic in childrenGraphics)
            {
                childrenGraphic.color = value ? _active : Color.gray;
            }
            
            _button.image.color = value ? Color.white : Color.gray;
            
            if(_text) 
                _text.color = Color.white;

            OnInteractableChange(value);
            
            if (_buy)
            {
                _buy.GetComponent<Graphic>().color = Color.white;
                _buy.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                
                _buy.gameObject.SetActive(!value);
            }
        }
    }
}