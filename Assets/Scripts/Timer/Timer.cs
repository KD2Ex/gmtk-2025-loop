using System;

public class Timer
{
    public Action Timeout;

    protected float waitTime;
    protected float timeLeft;
    protected bool isRunning;
    protected bool oneShot;

    public bool IsRunning => isRunning;
        
    public Timer(float waitTime, bool oneShot)
    {
        this.waitTime = waitTime;
        this.oneShot = oneShot;
    }

    public void Tick(float delta)
    {
        if (!isRunning) return;
            
        timeLeft -= delta;
        if (timeLeft <= 0.0f)
        {
            Stop();
        }
    }

    public void Start()
    {
        timeLeft = waitTime;
        isRunning = true;
    }

    public virtual void Stop()
    {
        isRunning = false;

        Timeout?.Invoke();

        if (oneShot) return;

        Start();
    }

    public virtual void Pause()
    {
        isRunning = false;
    }

    public virtual void Resume()
    {
        isRunning = true;
    }

    public void UpdateWaitTime(float time)
    {
        waitTime = time;
    }
}