using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject controlsPanel;
        private bool controlsOpen;


        private void OnEnable()
        {
            controlsPanel.SetActive(false);
            controlsOpen = false;
        }

        public void OnRestart()
        {
            GameManager.instance.RestartGame();
        }

        public void OnExit()
        {
            Application.Quit();
        }

        public void OnMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
        
        public void OnControls()
        {
            controlsPanel.SetActive(!controlsPanel.activeSelf);
            controlsOpen = !controlsOpen;
        }
    }
}