using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    public class Sound
    {
        private static GameObject Sounds
        {
            get
            {
                if (!_sounds)
                    _sounds = new GameObject("Sounds");

                return _sounds;
            }
        }

        private static GameObject _sounds;

        private static GameSounds _gameSounds;
        private static AudioMixer _mixer;

        public static bool Pause { get; private set; }

        public Sound(GameSounds gameSounds)
        {
            _gameSounds = gameSounds;
        }

        public static void PlayWin() => Play(_gameSounds.Win);

        public static void PlayExplosion() => Play(_gameSounds.Explosion);
        public static void PlayEnemyDie() => Play(_gameSounds.EnemyDead);
        public static void PlayHostageDie() => Play(_gameSounds.HostageDead);
        public static void PlayBackground() => Play(_gameSounds.Background, true);
        public static void PlayLose() => Play(_gameSounds.Lose);
        public static void PlayClick() => Play(_gameSounds.Click);
        public static AudioSource PlayDraw() => Play(_gameSounds.Draw, true);

        public static void SetPause(bool value)
        {
            AudioListener.pause = Pause = value;

            AudioListener.volume = value ? 0 : 1;
        }

        private static AudioSource Play(AudioClip clip, bool loop = false)
        {
            var source = Sounds.AddComponent<AudioSource>();

            source.clip = clip;
            source.loop = loop;
            source.Play();
            
            if(!loop)
                Object.Destroy(source, clip.length);

            return source;
        }
    }
}