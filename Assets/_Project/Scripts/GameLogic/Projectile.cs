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
        public Vector2 MovementDirection { get; private set; }
        public float MovementSpeed { get; private set; }
        public float TimeValue { get; private set; }
        public AttackType AttackType { get; private set; }
        public float SelfDestructInterval { get; private set; }
        public Vector3 TargetScale { get; private set; }
        public float ScaleUpMultiplier { get; private set; }
        public float SmoothTime { get; private set; }

        private Coroutine selfDestroyCountdownCR;
        private Vector3 velocity = Vector3.zero; //for smoothdamp

        public static Projectile Create(
            Transform prefabTransform,
            Vector3 worldPosition,
            AttackType attackType,
            float speed,
            float smoothTime,
            Vector2 moveDirection,
            float scaleUpTargetMultiplier,
            float rotation,
            float timeValue,
            float selfDestructInterval
            )
        {
            Transform projectileTransform = Instantiate(prefabTransform, worldPosition, Quaternion.identity);

            Projectile projectile = projectileTransform.GetComponent<Projectile>();

            if (attackType == AttackType.ScaleUp)
            {
                projectileTransform.Rotate(0, 0, rotation);
            }

            //projectile.SetSpeed(speed);
            //projectile.SetDirection(moveDirection);
            //projectile.SetTimeValue(timeValue);
            //projectile.SetAttackType(attackType);
            projectile.MovementSpeed = speed;
            projectile.MovementDirection = moveDirection;
            projectile.TimeValue = timeValue;
            projectile.AttackType = attackType;
            projectile.SelfDestructInterval = selfDestructInterval;
            projectile.TargetScale = projectileTransform.localScale * scaleUpTargetMultiplier;
            projectile.ScaleUpMultiplier = scaleUpTargetMultiplier;
            projectile.SmoothTime = smoothTime;

            return projectile;
        }

        //public void SetSpeed(float speed) => MovementSpeed = speed;
        //public void SetDirection(Vector2 direction) => MovementDirection = direction;
        //public void SetTimeValue(float value) => TimeValue = value;
        //public void SetAttackType(AttackType type) => AttackType = type;
        //public void SetSelfDestructInterval(float value) => SelfDestructInterval = value;

        private void Start()
        {
            selfDestroyCountdownCR = StartCoroutine(CountdownToSelfDestruct());
        }

        private void Update()
        {
            switch (AttackType)
            {
                case AttackType.MoveTowardPlayer:
                    HandleMovement();
                    break;
                case AttackType.ScaleUp:
                    HandleScaleUp();
                    break;
                default:
                    break;
            }
        }

        private void HandleScaleUp()
        {
            //transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * ScaleUpMultiplier, Time.deltaTime * MovementSpeed);
            //transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, Time.deltaTime * MovementSpeed);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, TargetScale, ref velocity, SmoothTime);
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