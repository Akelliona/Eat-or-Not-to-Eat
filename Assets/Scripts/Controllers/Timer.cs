using System;
using UnityEngine;

public class Timer
{
    public float Seconds { get => _remainTime; }
    float _time;
    float _remainTime;
    bool _running;
    Action _onTimerFinish;
    public Timer(float time, Action onTimerFinish)
    {
        _running = false;
        _time = time;
        _onTimerFinish = onTimerFinish;
    }

    public void Update(float delta)
    {
        if (_running) {
            _remainTime = Mathf.Max(_remainTime - delta, 0);
            if (_remainTime <= 0) {
                Finish();
            }
        }
    }

    public void Reset()
    {
        _remainTime = _time;
        _running = true;
    }

    public void Stop()
    {
        _running = false;
    }

    void Finish()
    {
        _onTimerFinish?.Invoke();
    }
}
