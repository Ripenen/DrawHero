using UnityEngine;

namespace Leveling.Collisions
{
    public abstract class Trigger : MonoBehaviour
    {
        public abstract void OnTriggered();
    }
}