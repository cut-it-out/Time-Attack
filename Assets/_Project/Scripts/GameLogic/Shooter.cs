using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] ShooterSettingsSO shooterSettings;

        [SerializeField] GameEventVoidSO onGameStart;
        [SerializeField] GameEventVoidSO onGameOver;

        public bool IsShooting { get; private set; } = false;

        private Coroutine shooterCR;
        private GameObject playerObject;

        private void Start()
        {
            playerObject = Game.GetInstance().Player;
            onGameStart.OnEventRaised += InitShooting;
            onGameOver.OnEventRaised += StopShooting;
        }

        private void InitShooting()
        {
            IsShooting = true;
            shooterCR = StartCoroutine(StartShooting());
        }

        IEnumerator StartShooting()
        {
            while (IsShooting)
            {
                Vector2 direction = (playerObject.transform.position - this.transform.position).normalized;
                Projectile.Create(
                    shooterSettings.ProjectilePrefab.transform, 
                    transform.position,
                    shooterSettings.AttackType,
                    shooterSettings.Speed,
                    shooterSettings.SmoothTime,
                    direction,
                    shooterSettings.ScaleUpTargetMultiplier,
                    shooterSettings.RandomRotation,
                    shooterSettings.RandomDamage,
                    shooterSettings.ProjectileAliveTime);
                
                yield return new WaitForSeconds(shooterSettings.ShootInterval);
            }
        }

        private void StopShooting()
        {
            if (shooterCR != null) StopCoroutine(shooterCR);
            shooterCR = null;
            IsShooting = false;
        }


    }
}
