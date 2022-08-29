using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace TimeAttack
{
    public class Player : MonoBehaviour
    {
        [Header("General settings")]
        [SerializeField] Color defaultColor = new Color(1, 1, 1, 1);

        [Header("Player Movement")]
        [SerializeField] float playerSpeed = 5f;
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

        [Header("Color feedback")]
        [SerializeField, Range(0,2)] float colorFeedbackInterval = 1f;
        [SerializeField] SpriteRenderer borderObjectSprite;
        [SerializeField] TMP_Text digitalClockText;
        [SerializeField] Color goodHitColor = new Color(0, 1, 0, 1);
        [SerializeField] Color badHitColor = new Color(1, 0, 0, 1);


        private Vector2 minBounds;
        private Vector2 maxBounds;
        private Vector2 movingDirection;

        private Coroutine colorFeedbackCR;

        // cached variables
        private Game game;
        private InputManager inputManager;
        private Camera mainCamera;        

        private void Awake()
        {
            game = Game.GetInstance();
            inputManager = InputManager.GetInstance();

            borderObjectSprite.color = defaultColor;
            digitalClockText.color = defaultColor;

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
                        StartColorFeedback(badHitColor);
                    }
                    else
                    {
                        if (goodHitSound != null)
                            soundEventChannel.RaisePlayEvent(goodHitSound);
                        StartColorFeedback(goodHitColor);
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

        private void StartColorFeedback(Color color)
        {
            borderObjectSprite.color = color;
            digitalClockText.color = color;

            borderObjectSprite.DOColor(defaultColor, colorFeedbackInterval);
            digitalClockText.DOColor(defaultColor, colorFeedbackInterval);
        }

    }
}
