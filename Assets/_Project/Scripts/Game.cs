using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeAttack
{
    public class Game : Singleton<Game>
    {
        [SerializeField] float initialTimerValue = 70f;

        [Header("Listening for")]
        [SerializeField] GameEventFloatSO ModifyTimerEvent;
        [SerializeField] GameEventBoolSO PauseGameEvent;
        [SerializeField] GameEventVoidSO onStartButtonClick;
        [SerializeField] GameEventVoidSO onRestartButtonClick;
        [SerializeField] GameEventVoidSO onQuitButtonClick;

        [Header("Broadcasting on")]
        [SerializeField] GameEventFloatSO onTimerUpdated;
        [SerializeField] GameEventVoidSO onGameOver;

        public float GameCountDownTime { get; private set; }
        public float GameScoreTime { get; private set; }
        private Coroutine countdownTimerCR;
        private Coroutine gameScoreTimerCR;


        // Game related
        public bool IsPaused { get; private set; }
        public bool IsGameOver { get; private set; } = false;

        // cached vars
        private CanvasManager canvasManager;

        protected override void Awake()
        {
            base.Awake();            
        }

        private void Start()
        {
            canvasManager = CanvasManager.GetInstance();
            ModifyTimerEvent.OnEventRaised += UpdateCountdownTimer;
            PauseGameEvent.OnEventRaised += PauseGame;
            onStartButtonClick.OnEventRaised += StartGame;
            onRestartButtonClick.OnEventRaised += RestartGame;
            onQuitButtonClick.OnEventRaised += ExitGame;

            InputManager.GetInstance().OnPauseMenuToggle += OnPauseKeyToggle;

            //StartGame();
        }

        private void OnPauseKeyToggle()
        {
            PauseGameEvent.RaiseEvent(!IsPaused);
        }

        #region Basic Game Stuff (start, reset, pause, etc)

        private void StartGame()
        {
            StopCountdownTimer();
            StopGameScoreTimer();
            IsGameOver = false;
            PauseGame(false); // to make sure we don't stuck in pause

            canvasManager.SwitchCanvas(CanvasType.GameUI);
            InitCountdownTimer(initialTimerValue);
            StartGameScoreTimer();

        }

        public void RestartGame()
        {            
            StartGame();
        }
        private void GameOver()
        {
            onGameOver.RaiseEvent();
            StopCountdownTimer();
            StopGameScoreTimer();

            // display gameover screen
            IsGameOver = true;
            canvasManager.SwitchCanvas(CanvasType.GameOverUI);
        }

        private void PauseGame(bool value)
        {
            if (value)
            {
                canvasManager.SwitchCanvas(CanvasType.PauseMenuUI);
            } 
            else
            {
                canvasManager.SwitchCanvas(CanvasType.GameUI);
            }
            Time.timeScale = value == true ? 0f : 1f;
            IsPaused = value;
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        #endregion

        #region Timer Functions

        private void StartGameScoreTimer()
        {
            StopGameScoreTimer();
            gameScoreTimerCR = StartCoroutine(GameCountDownTimer());
        }

        private void StopGameScoreTimer()
        {
            if (gameScoreTimerCR != null)
            {
                StopCoroutine(gameScoreTimerCR);
                gameScoreTimerCR = null;
            }
        }

        IEnumerator GameCountDownTimer()
        {
            GameScoreTime = 0f;
            
            while (true)
            {
                yield return new WaitForSeconds(1f);
                GameScoreTime += 1f;
            }
        }

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
