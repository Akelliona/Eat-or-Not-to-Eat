using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class GameOverPanel : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    RectTransform gameOverPanel;

    [SerializeField]
    LocalizeStringEvent goalText;

    public void Start()
    {
        GameEventBus.main.SubscribeOn(GameEvent.GameOver, this);
    }

    private void OnDestroy()
    {
        GameEventBus.main.UnsubscribeOn(GameEvent.GameOver, this);
    }
    public void OnEvent(GameEvent key, Dictionary<string, object> param)
    {

        gameOverPanel.gameObject.SetActive(true);
        goalText.StringReference.Arguments = new object[] { new StringVariable { Value = param["star"].ToString() } };
        goalText.RefreshString();
    }

    public void OnRepeat()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameEventBus.main.SendEvent(GameEvent.NewGame);
    }
}
