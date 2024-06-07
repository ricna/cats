using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Unrez
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer _audioMixer;
        public AudioMixerGroup _groupMusic;
        public AudioMixerGroup _groupSFX;
        public AudioSource _audioSourceSFX;
        public AudioSource _audioSourceMusic;

        public void Play(AudioClip clip, bool sfx = true)
        {
            if (sfx)
            {
                _audioSourceSFX.PlayOneShot(clip);
            }
            else
            {
                _audioSourceMusic.PlayOneShot(clip);
            }
        }
    }
}