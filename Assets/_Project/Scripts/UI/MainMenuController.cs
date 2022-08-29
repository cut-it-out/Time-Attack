using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TimeAttack
{
    public class MainMenuController : CanvasController
    {
        [SerializeField] Button startGameButton;

        private void OnEnable()
        {
            startGameButton.Select();
            InputManager.GetInstance().OnSpaceKeyPressed += OnSpaceKeyPressed;
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
