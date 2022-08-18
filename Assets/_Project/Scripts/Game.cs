using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Game : Singleton<Game>
    {
        [SerializeField] float initialTimerValue = 70f;

        [Header("Events")]
        [SerializeField] GameEventFloatSO TimerModified;
        [SerializeField] GameEventFloatSO onTimerUpdated;

        public float GameCountDownTime { get; private set; }
        private Coroutine countdownTimerCR;


        // Game related
        public bool IsPaused { get; private set; }
        public bool IsGameOver { get; private set; } = false;

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            TimerModified.OnEventRaised += UpdateCountdownTimer;

            StartGame();
        }

        #region Basic Game Stuff (start, reset, pause, etc)

        private void StartGame()
        {
            IsGameOver = false;
            PauseGame(false); // to make sure we don't stuck in pause

            InitCountdownTimer(initialTimerValue);
        }

        public void RestartGame()
        {
            StopCountdownTimer();
            IsPaused = true;

            StartGame();
        }
        private void GameOver()
        {
            StopCountdownTimer();
            
            // display gameover screen
            IsGameOver = true;
            IsPaused = true;
        }

        private void PauseGame(bool value)
        {
            Time.timeScale = value == true ? 0f : 1f;
            IsPaused = value;
        }

        #endregion

        #region Timer Functions

        private void StartCountdownTimer()
        {
            StopCountdownTimer(); // check to only have one timer running
            countdownTimerCR = StartCoroutine(CountdownTimer());
        }

        private void StopCountdownTimer()
        {
            if (countdownTimerCR != null)
            {
                StopCoroutine(countdownTimerCR);
                countdownTimerCR = null;
            }
        }

        private void UpdateCountdownTimer(float timeToAdd)
        {
            if (countdownTimerCR != null)
            {
                if (timeToAdd < 0 && GameCountDownTime <= timeToAdd)
                {
                    TimerIsUp();
                }
                else
                {
                    GameCountDownTime += timeToAdd;
                    onTimerUpdated.RaiseEvent(GameCountDownTime);
                }
            }
        }

        private void InitCountdownTimer(float timeToSetTo)
        {
            initialTimerValue = timeToSetTo;
            StartCountdownTimer();
        }

        private void TimerIsUp()
        {
            GameCountDownTime = 0f;
            onTimerUpdated.RaiseEvent(GameCountDownTime);
            GameOver();
        }

        IEnumerator CountdownTimer()
        {
            GameCountDownTime = initialTimerValue;
            onTimerUpdated.RaiseEvent(GameCountDownTime);
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if (GameCountDownTime <= 1f)
                {
                    TimerIsUp();

                    break;
                }
                else
                {
                    UpdateCountdownTimer(-1f);
                }
            }
        }


        #endregion

    }
}
