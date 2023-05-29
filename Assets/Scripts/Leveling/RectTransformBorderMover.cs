using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Leveling
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformBorderMover : MonoBehaviour
    {
        [SerializeField] private RectTransform _border;

        private void Start()
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.SetParent(_border);
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.anchorMin = Vector2.one * 0.5f;
            var rect = _border.rect;
            
            var path = new[]
            {
                rect.max,
                new Vector2(rect.xMax, rect.yMin),
                rect.min,
                new Vector2(rect.xMin, rect.yMax)
            };
            
            var offset = new Vector2(rectTransform.rect.width * 0.5f, -rectTransform.rect.height * 0.5f);

            Move(path, offset,0);
        }

        private void Move(IReadOnlyList<Vector2> path, Vector2 offset, int lastMovedIndex)
        {
            if(path.Count == 0)
                return;
            
            var rectTransform = transform as RectTransform;

            if (lastMovedIndex >= path.Count)
                lastMovedIndex = 0;
            
            rectTransform.DOAnchorPos(path[lastMovedIndex] + offset, 1).OnComplete(() =>
            {
                lastMovedIndex++;
                
                Move(path, offset, lastMovedIndex);
            });
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}