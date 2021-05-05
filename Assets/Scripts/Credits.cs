using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Credits : MonoBehaviour
{
    [SerializeField] float MovementX;
    [SerializeField] float Duration;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    

    private void Start()
    {
        InputController.Instance.SpecialAbility += onCreditsCompleted;

        rectTransform.DOAnchorPosY(MovementX, Duration).SetEase(Ease.Linear).OnComplete(onCreditsCompleted);
    }


    void onCreditsCompleted()
    {
        InputController.Instance.SpecialAbility -= onCreditsCompleted;
        SceneController.Instance.NextLevel();
    }
    private void OnDestroy()
    {
        InputController.Instance.SpecialAbility -= onCreditsCompleted;
    }
}
