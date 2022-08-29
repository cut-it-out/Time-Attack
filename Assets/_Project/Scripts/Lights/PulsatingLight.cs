using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TimeAttack
{
    public class PulsatingLight : MonoBehaviour
    {

        private Light light;

        private void Start()
        {
            light = GetComponent<Light>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
