using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TimeAttack
{
    public class UIAudioSlider : MonoBehaviour
    {
        public event UnityAction<float> SaveVolume = delegate { };

        private void OnEnable()
        {
            GetComponent<Slider>().onValueChanged.AddListener(HandleSliderValueChanged);
        }

        private void OnDisable()
        {
            GetComponent<Slider>().onValueChanged.RemoveListener(HandleSliderValueChanged);
        }

        public void SetSliderValue(float newVolume)
        {
            GetComponent<Slider>().value = newVolume;
        }

        private void HandleSliderValueChanged(float newVolume)
        {
            SaveVolume.Invoke(newVolume);
        }
    }
}
