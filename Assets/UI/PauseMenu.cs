using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void OnRestart()
        {
            GameManager.instance.RestartGame();
        }
    }
}