using System.Collections;
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
            StartCoroutine(StartPlaying(audioSource, audioClip, sfx, loop));
        }

        private IEnumerator StartPlaying(AudioSource audioSource, AudioClip audioClip, bool sfx = true, bool loop = false)
        {
            if (audioSource.isPlaying || audioSource.volume > 0)
            {
                yield return StartCoroutine(StopPlaying(audioSource));
            }
            while (audioSource.volume > 0)
            {
                yield return null;
            }
            audioSource.outputAudioMixerGroup = sfx ? _groupSFX : _groupMusic;
            audioSource.loop = loop;
            audioSource.clip = audioClip;
            audioSource.Play();
            audioSource.volume = 1;
        }

        public void Stop(AudioSource audioSource)
        {
            StartCoroutine(StopPlaying(audioSource));
        }

        private IEnumerator StopPlaying(AudioSource audioSource)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.time / 0.1f;
                audioSource.volume = 1 - t;
                yield return null;
            }
            audioSource.Stop();
        }
    }
}