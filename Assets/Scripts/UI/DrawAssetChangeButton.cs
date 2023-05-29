using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DrawAssetChangeButton : ActionButton<DrawAsset>
    {
        [SerializeField] private Image _ad;

        protected override void OnClick()
        {
            if(_ad)
                _ad.gameObject.SetActive(!Open);
        }

        protected override void OnInteractableChange(bool value)
        {
            if (_ad)
            {
                _ad.gameObject.SetActive(!value);
                _ad.color = Color.white;
            }
        }
    }
}