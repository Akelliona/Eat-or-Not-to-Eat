using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    float DragSensivity = 0.3f;

    Vector2 _startDragPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp((_startDragPos - eventData.position).x / (Screen.width * 0.5f) * 45, -45, 45));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var delta = (_startDragPos - eventData.position).x / (Screen.width * 0.5f);

        if (Mathf.Abs(delta) > DragSensivity) {
            GameEventBus.main.SendEvent(delta > 0 ? GameEvent.SwipeLeft : GameEvent.SwipeRight);
        }

        transform.rotation = Quaternion.identity;
    }
}
