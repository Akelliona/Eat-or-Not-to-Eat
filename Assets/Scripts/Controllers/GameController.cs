using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameController : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    GameSettings _settings;

    CardView _cardView;
    GameObject _cardPrefab;

    Player _player;
    IItem _curItem;
    Timer _timer;
    ItemGenerator _generator;

    void Start()
    {
        _generator = new ItemGenerator();

        Addressables.LoadAssetAsync<GameObject>("Card").Completed += OnLoadDone;
        GameEventBus.main.SubscribeOn(GameEvent.SwipeRight, this);
        GameEventBus.main.SubscribeOn(GameEvent.SwipeLeft, this);
        GameEventBus.main.SubscribeOn(GameEvent.NewGame, this);
    }

    void Update()
    {
        _timer?.Update(Time.deltaTime);
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Result != null) {
            _cardPrefab = obj.Result;
            NewGame();
        }
    }

    private void OnDestroy()
    {
        GameEventBus.main.UnsubscribeOn(GameEvent.SwipeRight, this);
        GameEventBus.main.UnsubscribeOn(GameEvent.SwipeLeft, this);
        GameEventBus.main.UnsubscribeOn(GameEvent.NewGame, this);
    }


    void Continue()
    {
        _cardView.Disappear();
        NewTurn();
    }

    void NewGame()
    {
        _player = new Player(_settings.MaxLife);
        _timer = new Timer(_settings.TimeForChoose, TurnEnd);

        SendStarEvent();
        SendHeartEvent();

        NewTurn();
    }

    void NewTurn()
    {
        _curItem = _generator.CreateItem();
        _timer.Reset();
        GameEventBus.main.SendEvent(GameEvent.TimerEvent, new Dictionary<string, object> {
            { "action", "start" },
            { "time", _timer.Seconds}
        });
        _cardView = Instantiate(_cardPrefab, transform).GetComponent<CardView>();
        _cardView.Init(_curItem);
    }

    void TurnEnd()
    {
        PlayerUnsuccesfulChoice();
    }

    void Restart()
    {
        Destroy(_cardView.gameObject);
        NewGame();
    }

    bool isGameOver()
    {
        return _player.Hearts == 0;
    }
    void GameOver()
    {
        _timer.Stop();
        GameEventBus.main.SendEvent(GameEvent.TimerEvent, new Dictionary<string, object> { { "action", "stop" } });
        GameEventBus.main.SendEvent(GameEvent.GameOver, new Dictionary<string, object> { { "star", _player.StarsAmount } });
    }

    void GameOverOrContinue()
    {
        if (isGameOver()) {
            GameOver();
        } else {
            Continue();
        }
    }

    void PlayerSuccesfulChoice()
    {
        AddStar();
        GameOverOrContinue();
    }

    void PlayerUnsuccesfulChoice()
    {
        RemoveHeart();
        GameOverOrContinue();
    }

    void AddStar()
    {
        ++_player.StarsAmount;
        SendStarEvent();
    }

    void RemoveHeart()
    {
        --_player.Hearts;
        SendHeartEvent();
    }
    public void OnEvent(GameEvent key, Dictionary<string, object> param)
    {
        switch (key) {
            case GameEvent.SwipeRight:
                if (_curItem.IsEatable) {
                    PlayerSuccesfulChoice();
                } else {
                    PlayerUnsuccesfulChoice();
                }
                break;
            case GameEvent.SwipeLeft:
                if (_curItem.IsEatable) {
                    PlayerUnsuccesfulChoice();
                } else {
                    PlayerSuccesfulChoice();
                }
                break;
            case GameEvent.NewGame:
                Restart();
                break;
        }
    }

    void SendHeartEvent()
    {
        GameEventBus.main.SendEvent(GameEvent.HeartAmountChanged, new Dictionary<string, object> { { "hearts", _player.Hearts }, { "maxHearts", _player.MaxHeart } });
    }

    void SendStarEvent()
    {
        GameEventBus.main.SendEvent(GameEvent.StarAmountChanged, new Dictionary<string, object> { { "star", _player.StarsAmount } });
    }
}
