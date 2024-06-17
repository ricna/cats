using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Unrez.Essential.Audio
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
        public void Play(AudioSource audioSource, AudioClip audioClip, bool sfx = true, bool loop = false)
        {
            if (audioSource.isPlaying)
            {
                Debug.Log("AudioSource was Playing");
                audioSource.Stop();
            }
            audioSource.outputAudioMixerGroup = sfx ? _groupSFX : _groupMusic;
            audioSource.loop = loop;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}