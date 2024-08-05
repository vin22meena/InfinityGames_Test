using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AudioManager.cs Class, Handles all the Game Music and Special Effects Events.
/// </summary>

namespace LoopEnergyClone
{
    public class AudioManager : GenericSingleton<AudioManager>
    {
        [SerializeField] List<GameAudioData> _gameAudioData = new List<GameAudioData>(); //Audio Data Reference
        [SerializeField] AudioSource _gameSFXAudioSource; //SFX Audio Source
        [SerializeField] AudioSource _gameMusicAudioSource; //Music Audio Source

        bool IsAudioMute = false; //Mute Toggle
        bool CanVibrate = true; //Vibration Toggle

        /// <summary>
        /// Play SFX based on specific audio key
        /// </summary>
        /// <param name="audioKey"></param>
        public void PlaySFX(string audioKey)
        {
            AudioClip clip = GetAudioClip(audioKey);

            if (clip == null)
                return;

            _gameSFXAudioSource.PlayOneShot(clip);
        }


        /// <summary>
        /// Get Audio Clip from Audio Data
        /// </summary>
        /// <param name="audioKey"></param>
        /// <returns></returns>
        AudioClip GetAudioClip(string audioKey)
        {
            return _gameAudioData.Find((match) => match.audioKey == audioKey).audioClip;
        }



        /// <summary>
        /// Toggle Mute
        /// </summary>
        public void ToggleMute()
        {
            IsAudioMute = !IsAudioMute;

            if (IsAudioMute)
            {
                _gameMusicAudioSource.volume = 0f;
                _gameSFXAudioSource.volume = 0f;
            }
            else
            {
                _gameMusicAudioSource.volume = 1f;
                _gameSFXAudioSource.volume = 1f;
            }
        }

        /// <summary>
        /// Toggle Vibrate
        /// </summary>
        public void VibrateToggle()
        {
            CanVibrate = !CanVibrate;
        }

        /// <summary>
        /// Perform Vibration
        /// </summary>
        public void Vibrate()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (CanVibrate)
            {
                Handheld.Vibrate();
            }
#endif
        }



        /// <summary>
        /// Audio Data Class
        /// </summary>
        [Serializable]
        public class GameAudioData
        {
            public string audioKey;
            public AudioClip audioClip;
        }

    }

}