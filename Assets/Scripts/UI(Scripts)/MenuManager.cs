using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MenuManager : MonoBehaviour
{

    private static MenuManager instance;
    public static MenuManager Instance => instance;

    bool canPause = false;
    bool hasEndedTransition = true;
    RectTransform pausePanel;
    Vector2 initPos;
    Button2[] uIButtons;
    //UIWidget[] uIWidgets;
    EventSystem eventSystem;
    [SerializeField] Image transitionImage;

    [SerializeField] RectTransform ContinueButton;
    [SerializeField] RectTransform LastCheckPointButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        pausePanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        uIButtons = pausePanel.GetChild(1).GetComponentsInChildren<Button2>(); 
        initPos = pausePanel.anchoredPosition;
    }

    private void Start()
    {
        InputController.Instance.Pause += Pause;

        transitionImage.gameObject.SetActive(true);
        transitionImage.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() => {

            transitionImage.gameObject.SetActive(false);
            canPause = true;
        });
        
    }
    public void Pause()
    {
        if (!canPause) return;
        if (!hasEndedTransition) return;
        hasEndedTransition = false;

        EvaluateBottonOne();

        if (!BasicCharacter.Instance.IsAlive && GameManager.Instance.IsPaused)
        {
            return;
        }

        if (!GameManager.Instance.IsPaused)
        {
            GameManager.Instance.Pause();
            pausePanel.gameObject.SetActive(true);
            for (int i = 0; i < uIButtons.Length; i++)
            {
                uIButtons[i].interactable = true;
            }
            pausePanel.DOAnchorPos(Vector2.zero + Vector2.up * 20, 0.15f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(()=> { hasEndedTransition = true; });
        }
        else
        {
            GameManager.Instance.Unpause();
            for (int i = 0; i < uIButtons.Length; i++)
            {
                uIButtons[i].interactable = false;
            }
            pausePanel.DOAnchorPos(initPos, 0.35f).SetEase(Ease.InBack).SetUpdate(true)
            .OnComplete(()=> 
            {
                pausePanel.gameObject.SetActive(false);
                hasEndedTransition = true;
            });
        }
    }


    void EvaluateBottonOne()
    {
        ContinueButton.gameObject.SetActive(BasicCharacter.Instance.IsAlive);
        LastCheckPointButton.gameObject.SetActive(!BasicCharacter.Instance.IsAlive);
    }

    public void LastCheckPoint()
    {
        SceneController.Instance.GoToLastCheckpoint();
        Pause();
    }

    public void NextLevelTransition()
    {
        transitionImage.gameObject.SetActive(true);
        transitionImage.DOColor(new Color(0, 0, 0, 1), 4f);
    }
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
    public void MainScreen()
    {
        GameManager.Instance.MainScreen();
    }
    private void OnDestroy()
    {
        InputController.Instance.Pause-= Pause;
    }

}

public enum MenuButtons
{
    Continue,
    Configuration,
    MainScreen,
    Restart
}