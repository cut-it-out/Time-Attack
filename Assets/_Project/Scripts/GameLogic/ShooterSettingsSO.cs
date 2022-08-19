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
        [SerializeField] Vector2 rotationLimits = new Vector2(-75f, 0f);
        [SerializeField] float scaleUpTargetMultiplier = 10f;
        [SerializeField][Tooltip("this is for scaling")] float smoothTime = 2.5f;

        [Header("Damage")]
        [SerializeField] Vector2 damageLimits = new Vector2(15f, 30f);
        [SerializeField] bool damageIsNegative = true;

        [Header("Lifetime")]
        [SerializeField] float aliveTime = 5f;

        public GameObject ProjectilePrefab { get { return projectilePrefab; } }
        public AttackType AttackType { get { return attackType; } }
        public float Speed { get { return speed; } }
        public float ShootInterval { get { return shootInterval; } }
        public float RandomDamage { get { 
            return (damageIsNegative == true ? -1f : 1f) * UnityEngine.Random.Range(damageLimits.x, damageLimits.y); } }
        public float ProjectileAliveTime { get { return aliveTime; } }
        public float RandomRotation { get { return UnityEngine.Random.Range(rotationLimits.x, rotationLimits.y); } }
        public Vector2 RotationLimits { get { return rotationLimits; } }
        public float ScaleUpTargetMultiplier { get { return scaleUpTargetMultiplier; } }
        public float SmoothTime { get { return smoothTime; } }
    }
}
