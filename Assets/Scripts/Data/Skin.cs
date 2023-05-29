using Leveling;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Skin", fileName = "Skin", order = 0)]
    public class Skin : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private SkinView _skinView;
        [SerializeField] private int _id;
        public SkinView View => _skinView;

        public int ID => _id;
    }
}