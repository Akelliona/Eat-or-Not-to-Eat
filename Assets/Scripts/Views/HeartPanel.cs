using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartPanel : MonoBehaviour, IGameEventListener
{
    List<RectTransform> _hearts;

    public void OnEvent(GameEvent key, Dictionary<string, object> param)
    {
        if (key == GameEvent.HeartAmountChanged) {
            uint max = (uint)param["maxHearts"];
            uint cur = (uint)param["hearts"];
            while (_hearts.Count < max) {
                _hearts.Add(Instantiate(_hearts[0], transform));
            }

            if (max < _hearts.Count) {
                for (var i = (int)max; i < _hearts.Count; i++) {
                    Destroy(_hearts[i].gameObject);
                }

                _hearts.RemoveRange((int)max, _hearts.Count - (int)max);
            }

            for(int i = 0; i < _hearts.Count; i++) {
                _hearts[i].GetComponent<Image>().color = i < cur ? Color.red : Color.gray;
            }
        }
    }

    void Start()
    {
        GameEventBus.main.SubscribeOn(GameEvent.HeartAmountChanged, this);
        _hearts= new List<RectTransform>();
        for(int i = 0; i < transform.childCount; i++) {
            _hearts.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }

    void OnDestroy()
    {
        GameEventBus.main.UnsubscribeOn(GameEvent.HeartAmountChanged, this);
    }
}

