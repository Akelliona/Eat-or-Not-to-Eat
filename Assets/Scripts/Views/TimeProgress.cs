using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeProgress : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    Slider slider;

    bool _isRunning = false;
    public void OnEvent(GameEvent key, Dictionary<string, object> param)
    {
        if (key == GameEvent.TimerEvent) {
            switch (param["action"].ToString()) {
                case "start":
                    _isRunning = true;
                    slider.maxValue = (float)param["time"];
                    slider.value = slider.maxValue;
                    break;
                case "stop":
                    _isRunning = false;
                    break;
            }

        }
    }
    void Start()
    {
        GameEventBus.main.SubscribeOn(GameEvent.TimerEvent, this);
    }

    void OnDestroy()
    {
        GameEventBus.main.UnsubscribeOn(GameEvent.TimerEvent, this);
    }

    void Update()
    {
        if (_isRunning) {
            slider.value -= Time.deltaTime;
        }
    }
}
