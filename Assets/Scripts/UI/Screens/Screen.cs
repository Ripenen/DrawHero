using Processes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        private void Start()
        {
            foreach (var button in GetComponentsInChildren<Button>(true))
            {
                button.onClick.AddListener(Sound.Sound.PlayClick);
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}