using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public enum AttackType
    {
        MoveTowardPlayer,
        ScaleUp
    }

    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameEventVoidSO ClearProjectilesEvent;

        public AttackType AttackType { get; private set; }
        public Vector2 MovementDirection { get; private set; }
        public float MovementSpeed { get; private set; }
        public float TimeValue { get; private set; }
        public float SelfDestructInterval { get; private set; }
        public float CircleStartRadius { get; private set; }
        public float CircleTargetRadius { get; private set; }
        public float CircleRotation { get; private set; }
        public int CircleOpenSlice { get; private set; }

        private Coroutine selfDestroyCountdownCR;
        private Coroutine movementCR;

        public static Projectile Create(
            Transform prefabTransform,
            Vector3 initPosition,
            float timeValue,
            float selfDestructInterval,
            AttackType attackType,
            Vector2 moveDirection,
            float speed,
            float circleStartRadius,
            float circleTargetRadius,
            float circleRotation, 
            int circleOpenSlice
            )
        {
            Transform projectileTransform = Instantiate(prefabTransform, initPosition, Quaternion.identity);

            Projectile projectile = projectileTransform.GetComponent<Projectile>();

            //if (attackType == AttackType.ScaleUp)
            //{
            //    projectileTransform.Rotate(0, 0, circleRotation);
            //}

            projectile.CircleRotation = circleRotation;
            projectile.TimeValue = timeValue;
            projectile.SelfDestructInterval = selfDestructInterval;
            projectile.AttackType = attackType;
            projectile.MovementDirection = moveDirection;
            projectile.MovementSpeed = speed;
            projectile.CircleStartRadius = circleStartRadius;
            projectile.CircleTargetRadius = circleTargetRadius;
            projectile.CircleOpenSlice = circleOpenSlice;

            if (attackType == AttackType.ScaleUp)
            {
                projectile.gameObject.GetComponent<CircleRenderer>().InitCircleRenderer(
                    projectile.transform.position,
                    projectile.CircleStartRadius, 
                    projectile.CircleTargetRadius,
                    projectile.MovementSpeed,
                    projectile.CircleOpenSlice,
                    projectile.CircleRotation);
            }

            return projectile;
        }

        private void Start()
        {
            selfDestroyCountdownCR = StartCoroutine(CountdownToSelfDestruct());
        }

        private void Update()
        {
            if (AttackType == AttackType.MoveTowardPlayer)
            {
                HandleMovement();
            }
        }

        private void OnEnable()
        {
            ClearProjectilesEvent.OnEventRaised += DeleteProjectile;
        }

        private void OnDisable()
        {
            ClearProjectilesEvent.OnEventRaised -= DeleteProjectile;
        }

        private void DeleteProjectile()
        {
            DestroySelf(0f);
        }

        private void HandleMovement()
        {
            Vector3 delta = MovementSpeed * Time.deltaTime * MovementDirection;
            transform.position += delta;
        }

        IEnumerator CountdownToSelfDestruct()
        {            
            yield return new WaitForSeconds(SelfDestructInterval);
            DestroySelf();
        }

        public void DestroySelf(float delay = 0.05f)
        {            
            Destroy(gameObject, delay);
        }
    }
}