using UnityEngine;

namespace Drawing
{
    [CreateAssetMenu(menuName = "Create DrawerConfig", fileName = "DrawerConfig", order = 0)]
    public class DrawerConfig : ScriptableObject
    {
        [SerializeField] private Material _material;
        [SerializeField] private float _lineWidth;
        [SerializeField] private float _drawDistance;
        [SerializeField] private string _lineMeshLayerName;
        [SerializeField] private Color _color;

        public Material Material => _material;

        public float LineWidth => _lineWidth;

        public string LineMeshLayerName => _lineMeshLayerName;

        public float DrawDistance => _drawDistance;
        public Color Color => _color;
    }
}