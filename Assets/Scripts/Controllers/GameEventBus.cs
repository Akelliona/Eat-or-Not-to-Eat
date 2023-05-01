using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    SwipeRight,
    SwipeLeft,

    EndOfTurn,
    GameOver,
    NewGame,

    HeartAmountChanged,
    StarAmountChanged,

    TimerEvent
}


public interface IGameEventListener
{
    void OnEvent(GameEvent key, Dictionary<string, object> param);
}

public class GameEventBus : MonoBehaviour
{
    public static GameEventBus main { get; private set; }
    Dictionary<GameEvent, List<IGameEventListener>> _eventListeners = new Dictionary<GameEvent, List<IGameEventListener>>();

    private void Awake()
    {
        if (main != null) {
            Debug.LogWarning("There is few GameEventBus in project");
        }

        main = this;
    }
    public void SubscribeOn(GameEvent name, IGameEventListener listener)
    {
        if (!_eventListeners.ContainsKey(name)) {
            _eventListeners[name] = new List<IGameEventListener>();
        }
        _eventListeners[name].Add(listener);
    }

    public void UnsubscribeOn(GameEvent name, IGameEventListener listener)
    {
        if (!_eventListeners.ContainsKey(name)) {
            Debug.Log($"Such event {name} is not registered in Bus");
            return;
        }

        _eventListeners[name].Remove(listener);
    }

    public void SendEvent(GameEvent name)
    {
        if (!_eventListeners.ContainsKey(name)) {
            Debug.Log($"Such event {name} is not registered in Bus");
            return;
        }

        foreach(var l in _eventListeners[name]) {
            l.OnEvent(name, null);
        }
    }

    public void SendEvent(GameEvent name, Dictionary<string, object> param)
    {
        if (!_eventListeners.ContainsKey(name)) {
            Debug.Log($"Such event {name} is not registered in Bus");
            return;
        }

        foreach (var l in _eventListeners[name]) {
            l.OnEvent(name, param);
        }
    }


}
