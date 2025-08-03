using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject controlsPanel;

        [SerializeField] private GameObject buttonControl;

        private Animator contAnim;
        private bool controlsOpen;


        private void OnEnable()
        {
            contAnim = buttonControl.GetComponent<Animator>();
            controlsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
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
            contAnim.SetTrigger("Unpressed");
            controlsPanel.SetActive(!controlsPanel.activeSelf);
            controlsOpen = !controlsOpen;
        }
    }
}