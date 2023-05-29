using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create DrawAsset", fileName = "DrawAsset", order = 0)]
    public class DrawAsset : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Material _material;
        [SerializeField] private ParticleSystem _trail;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _id;

        public Material Material => _material;

        public ParticleSystem Trail => _trail;

        public Sprite Icon => _icon;

        public int ID => _id;
    }
}