using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimeAttack
{
    public enum CanvasType
    {        
        GameUI,
        MainMenuUI,
        PauseMenuUI,
        GameOverUI
    }

    /// <summary>
	/// This class managing the canvases and changing between them.
	/// </summary>
    public class CanvasManager : Singleton<CanvasManager>
    {
        [Tooltip("This canvas will be loaded when the game is started.")]
        [SerializeField] CanvasType starterCanvasType;

        List<CanvasController> canvasControllerList;
        CanvasController lastActiveCanvas;

        protected override void Awake()
        {
            base.Awake();
            canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
            canvasControllerList.ForEach(x => x.gameObject.SetActive(false));

            SwitchCanvas(starterCanvasType);

        }

        // Disables last active canvas and enables the canvas passed as parameter
        public void SwitchCanvas(CanvasType cType)
        {
            if (lastActiveCanvas != null)
            {
                if (lastActiveCanvas.canvasType == cType) return;
                lastActiveCanvas.gameObject.SetActive(false);
            }

            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == cType);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            }
            else { Debug.LogWarning($"The {desiredCanvas.canvasType} canvas was not found!"); }
        }

        // Sets active state of the canvas what is passed as a parameter
        public void ActivateCanvas(CanvasType cType, bool desiredActiveState)
        {
            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == cType);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(desiredActiveState);
            }
            else { Debug.LogWarning($"The {desiredCanvas.canvasType} canvas was not found!"); }
        }

        public CanvasController GetCanvasController(CanvasType cType)
        {
            return canvasControllerList.Find(x => x.canvasType == cType);
        }
    }
}
