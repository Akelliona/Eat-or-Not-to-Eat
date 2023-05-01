using System.Collections.Generic;
using UnityEngine;

public class StarsPanel : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    TMPro.TMP_Text starsAmount;

    public void Start()
    {
        GameEventBus.main.SubscribeOn(GameEvent.StarAmountChanged, this);
    }

    public void OnDestroy()
    {
        GameEventBus.main.UnsubscribeOn(GameEvent.StarAmountChanged, this);
    }
    public void OnEvent(GameEvent key, Dictionary<string, object> param)
    {
        if (key == GameEvent.StarAmountChanged) {
            starsAmount.text = param["star"].ToString();
        }
    }
}
