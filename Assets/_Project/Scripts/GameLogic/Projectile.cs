using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Projectile : MonoBehaviour
    {
        public Vector2 MovementDirection { get; private set; }
        public float MovementSpeed { get; private set; }
        public float TimeValue { get; private set; }

        public static Projectile Create(Transform prefabTransform, Vector3 worldPosition, float speed, Vector2 moveDirection, float timeValue)
        {
            Transform projectileTransform = Instantiate(prefabTransform, worldPosition, Quaternion.identity);

            Projectile projectile = projectileTransform.GetComponent<Projectile>();

            projectile.SetSpeed(speed);
            projectile.SetDirection(moveDirection);
            projectile.SetTimeValue(timeValue);

            return projectile;
        }

        public void SetSpeed(float speed) => MovementSpeed = speed;
        public void SetDirection(Vector2 direction) => MovementDirection = direction;
        public void SetTimeValue(float value) => TimeValue = value;

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector3 delta = MovementDirection * MovementSpeed * Time.deltaTime;
            transform.position += delta;
        }

        public void DestroySelf(float delay = 0.1f)
        {            
            Destroy(gameObject, delay + 0.1f);
        }
    }
}