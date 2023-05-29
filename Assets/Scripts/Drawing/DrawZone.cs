using UnityEngine;

namespace Drawing
{
    [RequireComponent(typeof(RectTransform))]
    public class DrawZone : MonoBehaviour
    {
        public Rect ScreenRect => GetDrawZoneScreenRect();
    
        private Rect GetDrawZoneScreenRect()
        {
            var rectTransform = transform as RectTransform;
            Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
            size *= 0.925f;
            return new Rect((Vector2)rectTransform.position - (size * 0.5f), size);
        }
    }
}