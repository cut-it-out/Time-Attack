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

        [Header("Sound")]
        [SerializeField] GameEventAudioSO soundEventChannel;
        [SerializeField] AudioFileSO soundFile;

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
            StopShooting();
            IsShooting = true;
            shooterCR = StartCoroutine(Shooting());
        }

        IEnumerator Shooting()
        {
            while (IsShooting)
            {   
                Vector2 direction = (shooterSettings.RandomDirection - this.transform.position).normalized;
                Projectile.Create(
                    shooterSettings.ProjectilePrefab.transform,
                    this.transform.position,
                    shooterSettings.RandomDamage,
                    shooterSettings.ProjectileAliveTime,
                    shooterSettings.AttackType,
                    direction,
                    shooterSettings.Speed,
                    shooterSettings.CircleStartRadius,
                    shooterSettings.CircleTargetRadius,
                    shooterSettings.CircleRandomRotation,
                    shooterSettings.CircleOpenSlice
                    );

                if (soundEventChannel != null && soundFile != null)
                {
                    soundEventChannel.RaisePlayEvent(soundFile);
                }

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
