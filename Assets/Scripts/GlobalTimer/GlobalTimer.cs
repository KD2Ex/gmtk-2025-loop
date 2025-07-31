using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public float currentTime ;
    public bool timerIsRunning = false;

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
    }
}