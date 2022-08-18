using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TimeAttack
{
    public class DisplayTime : MonoBehaviour
    {
        [SerializeField] GameEventFloatSO onTimerChanged;

        private TMP_Text textObject;

        private void OnEnable()
        {
            onTimerChanged.OnEventRaised += UpdateTimer;
            textObject = gameObject.GetComponent<TMP_Text>();
            if (textObject == null)
                Debug.LogError("no TMP_Text found to display the timer on");
        }

        private void OnDisable()
        {
            onTimerChanged.OnEventRaised -= UpdateTimer;
        }

        private void UpdateTimer(float value)
        {
            textObject.text = Helpers.GetFormattedTimer(value);
        }
    }
}
