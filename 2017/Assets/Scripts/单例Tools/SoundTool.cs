using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace K
{
    /// <summary>
    /// load file from Resources folder
    /// </summary>
    public class SoundTool : MonoBehaviour
    {
        public static SoundTool Instance { get; private set; }

        private int maxCount = 3;
        private int curIndex = 0;
        private bool isMute = false;

        private Dictionary<string, AudioClip> clips;
        private AudioSource[] audios;
        private AudioSource music;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(gameObject);
                }
            }

            _Init();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void _Init()
        {
            clips = new Dictionary<string, AudioClip>();

            music = gameObject.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = true;

            audios = new AudioSource[maxCount];
            for (int i = 0; i < maxCount; i++)
            {
                AudioSource audio = gameObject.AddComponent<AudioSource>();
                audio.playOnAwake = false;
                audio.loop = false;
                audios[i] = audio;
            }
        }


        #region volume

        public void SetMute(bool isMute)
        {
            for (int i = 0; i < maxCount; i++)
            {
                audios[i].mute = isMute;
            }
            music.mute = isMute;
        }

        public void SetAudioVolume(float value)
        {
            for (int i = 0; i < audios.Length; i++)
            {
                audios[i].volume = value;
            }
        }

        public void SetMusicVolume(float value)
        {
            music.volume = value;
        }

        #endregion

        #region audio

        public void PlayAudio(string path)
        {
            if (clips.ContainsKey(path))
            {
                _PlayAudio(clips[path]);
            }
            else
            {
                _LoadAudio(path);
            }
        }

        private void _PlayAudio(AudioClip clip)
        {
            if (clip == null) return;
            curIndex++;
            curIndex = curIndex % maxCount;
            audios[curIndex].PlayOneShot(clip);
        }

        #endregion

        #region music

        public void PlayMusic(string path)
        {
            if (clips.ContainsKey(path))
            {
                _PlayMusic(clips[path]);
            }
            else
            {
                _LoadAudio(path, true);
            }
        }

        private void _PlayMusic(AudioClip clip)
        {
            if (music.isPlaying)
                music.Stop();

            music.clip = clip;
            music.Play();
        }

        public void PauseMusic()
        {
            music.Pause();
        }

        public void UnPauseMusic()
        {
            music.UnPause();
        }

        public void StopMusic()
        {
            music.Stop();
        }

        public void SetMusicLoop(bool isLoop)
        {
            music.loop = isLoop;
        }

        #endregion

        private void _LoadAudio(string path, bool isBg = false)
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                clips.Add(path, clip);
                if (isBg)
                    _PlayMusic(clip);
                else
                    _PlayAudio(clip);
            }
        }
    }
}
