using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkinChangeButton : ActionButton<Skin>
    {
        [SerializeField] private Image _ad;

        protected override void OnClick()
        {
            _ad.gameObject.SetActive(!Open);
        }

        protected override void OnInteractableChange(bool value)
        {
            _ad.gameObject.SetActive(!value);
            _ad.color = Color.white;
        }
    }
}