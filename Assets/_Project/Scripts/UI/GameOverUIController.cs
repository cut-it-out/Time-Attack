using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TimeAttack
{
    public class GameOverUIController : CanvasController
    {
        [SerializeField] TMP_Text gameScoreTimeText;

        private void OnEnable()
        {
            gameScoreTimeText.text = Helpers.GetFormattedTimer(Game.GetInstance().GameScoreTime);
        }

    }
}
