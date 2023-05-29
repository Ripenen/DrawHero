using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drawing
{
    public class DrawPositionsProvider
    {
        private readonly Camera _camera;
        private readonly Vector3 _projectionDistance;
        private IEnumerable<Rect> _screenDrawRects;
    
        private Vector3 _previousTickMousePosition;
        private Rect _currentDrawRect;

        public DrawPositionsProvider(Camera camera, float projectionDistance)
        {
            _camera = camera;
            _projectionDistance = new Vector3(0, 0, projectionDistance);
        }

        public bool CanProvide { get; private set; }
        public bool Provided { get; private set; }

        public Vector3 Provide()
        {
            Provided = true;
            return _camera.ScreenToWorldPoint(Input.mousePosition + _projectionDistance);
        }

        public void Tick()
        {
            if(_currentDrawRect == default && Input.GetMouseButton(0))
                _currentDrawRect = _screenDrawRects.FirstOrDefault(x => x.Contains(Input.mousePosition));
        
            CanProvide = NotEquals(Input.mousePosition, _previousTickMousePosition) 
                         && _currentDrawRect.Contains(Input.mousePosition);

            _previousTickMousePosition = Input.mousePosition;
        }

        private bool NotEquals(Vector3 first, Vector3 second) => (first - second).magnitude > 0.1f;

        public void SetDrawZone(IEnumerable<Rect> drawZones)
        {
            _screenDrawRects = drawZones;
        }

        public void Reset()
        {
            _currentDrawRect = default;
        }
    }
}