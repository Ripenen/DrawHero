using System;
using Data;
using Leveling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class SkinPreviewScreen : Screen
    {
        [SerializeField] private Button _getSkinButton;
        [SerializeField] private Button _refuseButton;
        [SerializeField] private Entity _entity;
        [SerializeField] private Image _drawAssetIcon;
        [SerializeField] private TextMeshProUGUI _name;

        public void Construct(Skin skin, Action onGetClicked, Action refuseClicked)
        {
            _entity.gameObject.SetActive(true);
            _entity.ApplySkin(skin);

            _name.text = "Новый персонаж";
            
            _getSkinButton.onClick.AddListener(() => onGetClicked?.Invoke());
            _refuseButton.onClick.AddListener(() => refuseClicked?.Invoke());
        }
        
        public void Construct(DrawAsset asset, Action onGetClicked, Action refuseClicked)
        {
            _drawAssetIcon.gameObject.SetActive(true);
            _drawAssetIcon.sprite = asset.Icon;
            
            _name.text = "Новый карандаш";
            
            _getSkinButton.onClick.AddListener(() => onGetClicked?.Invoke());
            _refuseButton.onClick.AddListener(() => refuseClicked?.Invoke());
        }
    }
}