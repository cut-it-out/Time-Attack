using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

namespace TimeAttack
{
    public class PulsatingLight : MonoBehaviour
    {
        [SerializeField] float intensityMin;
        [SerializeField] float intensityMax;
        [SerializeField] float interval;
        [SerializeField] float intervalVariance;

        private Light2D spotLight;
        private float currentIntensity;

        private void Start()
        {
            spotLight = GetComponent<Light2D>();
            currentIntensity = spotLight.intensity;

            //spotLight.DOIntensity(intensityMax, Random.Range(interval - intervalVariance, interval + intervalVariance));
            //spotLight.DOColor(new Color(1f,0f,0f),interval);

            DOTween.Sequence()
            .Append(DOTween.To(() => currentIntensity, x=> currentIntensity = x, intensityMax, Random.Range(interval - intervalVariance, interval + intervalVariance)))
            .Append(DOTween.To(() => currentIntensity, x=> currentIntensity = x, intensityMin, Random.Range(interval - intervalVariance, interval + intervalVariance)))
            .SetLoops(-1, LoopType.Restart);

            //.Append(spotLight.DOIntensity(intensityMin, Random.Range(interval - intervalVariance, interval + intervalVariance)))            
        }

        private void Update()
        {
            spotLight.intensity = currentIntensity;
        }

    }
}
