using UnityEngine;

namespace Leveling
{
    public class LevelFactory
    {
        private readonly Level[] _levels;
        private readonly Transform _parent;

        public LevelFactory(Level[] levels, Transform parent)
        {
            _levels = levels;
            _parent = parent;
        }

        public Level Create(int id)
        {
            int trueId = id;

            if (id > _levels.Length)
                id = Random.Range(2, _levels.Length);
            
            AppMetricaWeb.Event("lvl" + id);
            
            var level = Object.Instantiate(_levels[id - 1], _parent);
            
            level.Init(trueId);

            return level;
        }
    }
}