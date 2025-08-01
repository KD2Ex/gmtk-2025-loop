using TMPro;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public float currentTime ;
    public bool timerIsRunning = false;

    public TMP_Text timerText;

    private void Start()
    {
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            currentTime += Time.deltaTime;
        }

        if (timerText)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            string formattedTime = $"{minutes / 10}{minutes % 10}:{seconds / 10}{seconds % 10}";
            timerText.text = formattedTime;
        }
    }

    public void Pause()
    {
        timerIsRunning = false;
    }

    public void Resume()
    {
        timerIsRunning = true;
    }
}