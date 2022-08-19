using System;
using UnityEngine;
using UnityEngine.Events;

namespace TimeAttack
{
	public class UIAudioSliderManager : MonoBehaviour
    {
		[SerializeField] UIAudioSlider musicVolumeSlider;
		[SerializeField] UIAudioSlider sfxVolumeSlider;

		[Header("Broadcasting")]
		[SerializeField] GameEventFloatSO sfxVolumeEventChannel = default;
		[SerializeField] GameEventFloatSO musicVolumeEventChannel = default;
		//[SerializeField, Space] GameEventVoidSO saveSettingsEvent = default;

		private float musicVolume { get; set; }
		private float sfxVolume { get; set; }

        private void Start()
        {			
			musicVolumeSlider.SaveVolume += SaveMusicVolume;
			sfxVolumeSlider.SaveVolume += SaveSfxVolume;

			Setup();
        }

        public void Setup()
		{			
			musicVolume = Game.GetInstance().AudioManager.MusicVolume;
			sfxVolume = Game.GetInstance().AudioManager.SFXVolume;

            SetMusicVolumeSlider();
            SetSfxVolumeSlider();            
        }

        private void OnEnable()
        {
            Setup();
        }

        private void OnDisable()
        {
			SaveAudioSettings();
        }

		void SaveAudioSettings()
		{	
			//saveSettingsEvent.RaiseEvent();
		}

        #region SaveVolume Functions

		private void SaveMusicVolume(float newMusicVolume)
		{
			musicVolume = newMusicVolume;
			SetMusicVolume();
		}

		private void SaveSfxVolume(float newSfxVolume)
		{
			sfxVolume = newSfxVolume;
			SetSfxVolume();
		}
        #endregion

		private void SetMusicVolumeSlider()
        {
			musicVolumeSlider.SetSliderValue(musicVolume);
			SetMusicVolume();
		}

		private void SetSfxVolumeSlider()
		{
			sfxVolumeSlider.SetSliderValue(sfxVolume);
			SetSfxVolume();
		}

		private void SetMusicVolume()
		{
			musicVolumeEventChannel.RaiseEvent(musicVolume);
		}
		private void SetSfxVolume()
		{
			sfxVolumeEventChannel.RaiseEvent(sfxVolume);

		}		
	}
}
