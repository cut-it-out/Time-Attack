using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TimeAttack
{
    public class AudioManager : MonoBehaviour
    {
		[Header("SoundEmitter setup")]
		[SerializeField] AudioSource soundEmitterPrefab;
		[SerializeField] int prewarmSize = 10;

		[Header("Music player setup")]
		[SerializeField] SoundEmitter musicEmitter;

		[Header("Listening on")]
		[SerializeField] GameEventAudioSO musicEventChannel = default;
		[SerializeField] GameEventAudioSO SFXEventChannel = default;
        [SerializeField] GameEventFloatSO musicVolumeEventChannel = default;        
		[SerializeField] GameEventFloatSO SFXVolumeEventChannel = default;


        [Header("Audio control")]
        [SerializeField] AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] float masterVolume = .8f;
        [Range(0f, 1f)]
        [SerializeField] float musicVolume = .8f;
        [Range(0f, 1f)]
        [SerializeField] float sfxVolume = .8f;

		public float MasterVolume => masterVolume;
		public float SFXVolume => sfxVolume;
		public float MusicVolume => musicVolume;

		private const string MASTER_VOLUME_PARAM_NAME = "MasterVolume";
		private const string MUSIC_VOLUME_PARAM_NAME = "MusicVolume";
		private const string SFX_VOLUME_PARAM_NAME = "SFXVolume";
        private const float MIXER_VOLUME_MULTIPLIER = 30f;

        private AudioSourcePool soundEmitterPool;

        private void Awake()
        {
			soundEmitterPool = new AudioSourcePool(soundEmitterPrefab, this.transform, prewarmSize);
		}

        private void OnEnable()
        {
            musicVolumeEventChannel.OnEventRaised += ChangeMusicVolume;
            SFXVolumeEventChannel.OnEventRaised += ChangeSFXVolume;

			musicEventChannel.OnPlayEventRaised += PlayMusicTrack;
			musicEventChannel.OnStopEventRaised += StopMusicTrack;
			SFXEventChannel.OnPlayEventRaised += PlaySFX;
		}

        private void OnDisable()
        {
			musicVolumeEventChannel.OnEventRaised -= ChangeMusicVolume;
			SFXVolumeEventChannel.OnEventRaised -= ChangeSFXVolume;

			musicEventChannel.OnPlayEventRaised -= PlayMusicTrack;
			musicEventChannel.OnStopEventRaised -= StopMusicTrack;
			SFXEventChannel.OnPlayEventRaised -= PlaySFX;
		}

        private void PlayMusicTrack(AudioFileSO audioFile)
        {
			musicEmitter.PlayAudioClip(audioFile.Clip, audioFile.Settings, audioFile.IsLooping);
        }

		private void StopMusicTrack()
		{
			if (musicEmitter != null && musicEmitter.IsPlaying())
			{
				musicEmitter.Stop();
			}
		}

		private void PlaySFX(AudioFileSO audioFile)
        {
			var soundEmitter = soundEmitterPool.Request();
			soundEmitter.PlayAudioClip(audioFile.Clip, audioFile.Settings, audioFile.IsLooping);
		}

        /// <summary>
        /// This is only used in the Editor, to debug volumes.
        /// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
        /// </summary>
        void OnValidate()
		{
			if (Application.isPlaying)
			{
				SetGroupVolume(MASTER_VOLUME_PARAM_NAME, masterVolume);
				SetGroupVolume(MUSIC_VOLUME_PARAM_NAME, musicVolume);
				SetGroupVolume(SFX_VOLUME_PARAM_NAME, sfxVolume);
			}
		}
		void ChangeMasterVolume(float newVolume)
		{
			masterVolume = newVolume;
			SetGroupVolume(MASTER_VOLUME_PARAM_NAME, masterVolume);
		}
		void ChangeMusicVolume(float newVolume)
		{
			musicVolume = newVolume;
			SetGroupVolume(MUSIC_VOLUME_PARAM_NAME, musicVolume);
		}
		void ChangeSFXVolume(float newVolume)
		{
			sfxVolume = newVolume;
			SetGroupVolume(SFX_VOLUME_PARAM_NAME, sfxVolume);
		}
		public void SetGroupVolume(string parameterName, float normalizedVolume)
		{
			bool volumeSet = audioMixer.SetFloat(parameterName, SliderValueToLog10(normalizedVolume));
			if (!volumeSet)
				Debug.LogError("The AudioMixer parameter was not found");
		}

		public float GetGroupVolume(string parameterName)
		{
			if (audioMixer.GetFloat(parameterName, out float rawVolume))
			{
				return MixerValueToNormalized(rawVolume);
			}
			else
			{
				Debug.LogError("The AudioMixer parameter was not found");
				return 0f;
			}
		}

		// Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
		/// when using UI sliders normalized format
		private float MixerValueToNormalized(float mixerValue)
		{
			// We're assuming the range [-80dB to 0dB] becomes [0 to 1]
			return 1f + (mixerValue / 80f);
		}
		private float NormalizedToMixerValue(float normalizedValue)
		{
			// We're assuming the range [0 to 1] becomes [-80dB to 0dB]
			// This doesn't allow values over 0dB
			return (normalizedValue - 1f) * 80f;
		}

		private float SliderValueToLog10(float value)
        {
			return Mathf.Log10(value) * MIXER_VOLUME_MULTIPLIER;
        }

	}
}
