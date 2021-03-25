using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(EventTrigger2))]
public class UIWidget : MonoBehaviour
{
    EventTrigger2 @event;
    RectTransform rectTransform;
    Vector2 initPos;
    Vector2 initScale = Vector2.one;
    [SerializeField] Vector2 finalPos;
    [SerializeField] Vector2 finalScale;
    [SerializeField] Ease easeType;
    [SerializeField] float duration;
    [SerializeField] protected GameScene sceneToGo;
    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        @event = GetComponent<EventTrigger2>();
        initPos = rectTransform.anchoredPosition;
        initScale = rectTransform.localScale;
        @event.triggers[0].callback.AddListener((eventData)=> { OnPointerDown(); });
        @event.triggers[1].callback.AddListener((eventData)=> { OnPointerDown(); });
        @event.triggers[2].callback.AddListener((eventData)=> { OnSelect(); });
        @event.triggers[3].callback.AddListener((eventData) => { OnDeselect(); });
    }
    protected virtual void OnPointerDown()
    {
        AudioManager.Instance.Play("Ui_Confirm");
        GameManager.Instance.ChangeScene(sceneToGo);
    }
    protected virtual void OnSelect()
    {
        rectTransform.DOAnchorPos(initPos + finalPos, duration).SetEase(easeType);
        rectTransform.DOScale(initScale + finalScale, duration).SetEase(easeType);
        AudioManager.Instance.Play("Ui_Navigation");
    }
    protected virtual void OnDeselect()
    {
        rectTransform.DOAnchorPos(initPos, duration).SetEase(easeType);
        rectTransform.DOScale(initScale, duration).SetEase(easeType);
    }

    protected virtual void OnValidate()
    {
        @event = GetComponent<EventTrigger2>();
        if(@event.triggers.Count != 4)
        {
            @event.triggers = new List<EventTrigger2.Entry>();
            EventTrigger2.Entry p_down = new EventTrigger2.Entry();
            EventTrigger2.Entry submit = new EventTrigger2.Entry();
            EventTrigger2.Entry select = new EventTrigger2.Entry();
            EventTrigger2.Entry deselect = new EventTrigger2.Entry();
            p_down.eventID = EventTriggerType.PointerDown;
            submit.eventID = EventTriggerType.Submit;
            select.eventID = EventTriggerType.Select;
            deselect.eventID = EventTriggerType.Deselect;
            @event.triggers.Add(p_down);
            @event.triggers.Add(submit);
            @event.triggers.Add(select);
            @event.triggers.Add(deselect);
        }
    }
    public void changeScene(GameScene scene)
    {
        sceneToGo = scene;
    }
    public enum Widgetsounds
    {
        Select,
        PointerDown
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
