using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(menuName = "Create GameSounds", fileName = "GameSounds", order = 0)]
    public class GameSounds : ScriptableObject
    {
        public AudioClip Win;
        public AudioClip Explosion;
        public AudioClip Lose;
        public AudioClip Draw;
        public AudioClip EnemyDead;
        public AudioClip HostageDead;
        public AudioClip Background;
        public AudioClip Click;
    }
}