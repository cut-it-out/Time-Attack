using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    [CreateAssetMenu(fileName = "ShooterSettings", menuName = "GameLogic/ShooterSettings SO")]
    public class ShooterSettingsSO : ScriptableObject
    {        
        [Header("GameObjects")]
        [SerializeField] GameObject projectilePrefab;
                
        [Header("Movement and Scaling")]
        [SerializeField] AttackType attackType;
        [SerializeField] float shootInterval = 1f;
        [SerializeField, Space] float speed = 10f;
        [SerializeField] Vector2 circleRotationLimits = new Vector2(0f, 75f);
        [SerializeField] float circleRandomTreshold = 25f;
        [SerializeField] float circleStartRadius = 1f;
        [SerializeField] float circleTargetRadius = 5f;
        [SerializeField,
            Tooltip("how big size should be missing from the circle"), 
            Range(5, 50)] 
                int circleOpenSlice = 30;

        [Header("Damage")]
        [SerializeField] Vector2 damageLimits = new Vector2(15f, 30f);
        [SerializeField] bool damageIsNegative = true;

        [Header("Lifetime")]
        [SerializeField] float aliveTime = 5f;

        private float previousRandomRotation = 0f;

        public GameObject ProjectilePrefab { get { return projectilePrefab; } }
        public AttackType AttackType { get { return attackType; } }
        public float Speed { get { return speed; } }
        public float ShootInterval { get { return shootInterval; } }
        public float RandomDamage { get { 
            return (damageIsNegative == true ? -1f : 1f) * Random.Range(damageLimits.x, damageLimits.y); } }
        public float ProjectileAliveTime { get { return aliveTime; } }
        public float CircleRandomRotation 
        { 
            get 
            {
                float randomRotation =
                    Mathf.Abs(
                        Mathf.Clamp(
                            UnityEngine.Random.Range(                        
                                previousRandomRotation - circleRandomTreshold,
                                previousRandomRotation + circleRandomTreshold),
                            circleRotationLimits.x,
                            circleRotationLimits.y)
                        );

                previousRandomRotation = randomRotation;
                Debug.Log($"previousRandomRotation: {previousRandomRotation}, circleRotationLimits.x: {circleRotationLimits.x}, circleRotationLimits.y: {circleRotationLimits.y}");
                Debug.Log($"randomRotation: {randomRotation}");
                return randomRotation;
            } 
        }
        public Vector2 RotationLimits { get { return circleRotationLimits; } }
        public float CircleStartRadius { get { return circleStartRadius; } }
        public float CircleTargetRadius { get { return circleTargetRadius; } }
        public int CircleOpenSlice { get { return circleOpenSlice; } }

        private void Awake()
        {
            previousRandomRotation = Mathf.Abs(Random.Range(circleRotationLimits.x, circleRotationLimits.y));

        }

    }
}
