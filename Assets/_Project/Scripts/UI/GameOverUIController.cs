using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TimeAttack
{
    public class GameOverUIController : CanvasController
    {
        [SerializeField] TMP_Text gameScoreTimeText;
        [SerializeField] Button startGameButton;

        private void OnEnable()
        {
            gameScoreTimeText.text = Helpers.GetFormattedTimer(Game.GetInstance().GameScoreTime);
            InputManager.GetInstance().OnSpaceKeyPressed += OnSpaceKeyPressed;
            startGameButton.Select();
        }

        private void OnSpaceKeyPressed()
        {
            startGameButton.onClick.Invoke();
        }

        private void OnDisable()
        {
            if (InputManager.GetInstance() != null)
            {
                InputManager.GetInstance().OnSpaceKeyPressed -= OnSpaceKeyPressed;
            }
        }

    }
}
