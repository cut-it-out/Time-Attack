using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Player : MonoBehaviour
    {
        [Header("Player Movement")]
        [SerializeField] float playerSpeed = 5f;

        [Header("Player Movement padding")]
        [SerializeField] float paddingLeft;
        [SerializeField] float paddingRight;

        [Header("Player StartPos")]
        [SerializeField] float playerXPosition = 0f;
        [SerializeField] float playerYPosition = -12f;
        
        [Header("Events")]
        [SerializeField] GameEventFloatSO ModifyTimerEvent;

        [Header("Sound")]
        [SerializeField] GameEventAudioSO soundEventChannel;
        [SerializeField] AudioFileSO goodHitSound;
        [SerializeField] AudioFileSO badHitSound;

        private Vector2 minBounds;
        private Vector2 maxBounds;
        private Vector2 movingDirection;

        // cached variables
        private Game game;
        private InputManager inputManager;
        private Camera mainCamera;

        private void Awake()
        {
            game = Game.GetInstance();
            inputManager = InputManager.GetInstance();
            mainCamera = Camera.main;
            InitBounds();
            ResetPlayerPositionToStart();
        }

        public void ResetPlayerPositionToStart()
        {
            transform.position = new Vector2(playerXPosition, playerYPosition); // set playerto start
        }

        private void OnEnable()
        {
            // subscribe for movement
            inputManager.OnMovement += InputManager_OnMovement;
        }

        private void OnDisable()
        {
            inputManager.OnMovement -= InputManager_OnMovement;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                if (soundEventChannel != null)
                {
                    if (projectile.TimeValue < 0)
                    {
                        if (badHitSound != null) 
                            soundEventChannel.RaisePlayEvent(badHitSound);
                    }
                    else
                    {
                        if (goodHitSound != null)
                            soundEventChannel.RaisePlayEvent(goodHitSound);
                    }
                }
                ModifyTimerEvent.RaiseEvent(projectile.TimeValue);
                projectile.DestroySelf();
            }
        }

        private void InitBounds()
        {
            minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
            maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        }

        private void InputManager_OnMovement(Vector2 direction)
        {
            movingDirection = direction;
        }


        private void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            Vector2 delta = movingDirection * playerSpeed * Time.deltaTime;
            Vector2 newPos = new Vector2();

            newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
            newPos.y = playerYPosition;

            transform.position = newPos;
        }
    }
}
