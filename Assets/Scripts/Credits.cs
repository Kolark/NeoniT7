using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Credits : MonoBehaviour
{

    

    [Header("Credits Info")]
    [SerializeField] float MovementX;
    [SerializeField] float Duration;
    [SerializeField] RectTransform rectTransform;


    

    private void Start()
    {
        if(Comic.Instance != null)
        {
            Comic.Instance.SetOnComicEnd(()=> { startCredits(); });
        }
        
    }

    public void startCredits()
    {
        Comic.Instance.gameObject.SetActive(false);

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
