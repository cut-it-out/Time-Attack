using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] GameObject target = null;
        [SerializeField] float speed = 10f;
        [SerializeField] float damageMin = 10f;
        [SerializeField] float damageMax = 30f;
        [SerializeField] bool damageIsNegative = true;
        [SerializeField] float shootInterval = 1f;

        [SerializeField] GameEventVoidSO onGameStart;
        [SerializeField] GameEventVoidSO onGameOver;

        Coroutine shooterCR;

        private void Start()
        {
            onGameStart.OnEventRaised += InitShooting;
            onGameOver.OnEventRaised += StopShooting;
        }

        private void InitShooting()
        {
            shooterCR = StartCoroutine(StartShooting());
        }

        IEnumerator StartShooting()
        {
            while (true)
            {
                Vector2 direction = (target.transform.position - this.transform.position).normalized;
                float damageValue = (damageIsNegative == true ? -1f : 1f) * UnityEngine.Random.Range(damageMin, damageMax);
                Projectile.Create(projectilePrefab.transform, transform.position, speed, direction, damageValue);
                
                yield return new WaitForSeconds(shootInterval);
            }
        }

        private void StopShooting()
        {
            if (shooterCR != null) StopCoroutine(shooterCR);
            shooterCR = null;
        }


    }
}
