using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance => instance;

    bool canPause = false;
    bool hasEndedTransition = true;
    RectTransform pausePanel;
    Vector2 initPos;
    Button[] uIButtons;

    EventSystem eventSystem;
    [SerializeField] Image transitionImage;
    [SerializeField] RectTransform ContinueButton;

    Button button2Continue;
    [SerializeField] RectTransform LastCheckPointButton;
    Button button2LastCheckpoint;
    [Header("Volume Sliders")]
    [SerializeField] Slider gameplayVolumeSlider;
    [SerializeField] Slider uiVolumeSlider;


    [Header("Start Transition Components")]
    [SerializeField] Text placeholderText;
    [SerializeField] TextMeshProUGUI realText;

    [Header("Ending Transition Events")]
    [SerializeField] Image endingTransitionImage;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] Transform ContinueObj;
    Vector2[] scoreRectsInitPos;


    //sceneControllerDependent
    CinemachineVirtualCamera endingAnimationCamera;
    Transform trainSprite;
    Transform[] spritesPersonajes;

    public int getScore { get => ScoreManager.Instance.Score;}
    public float getTimetaken{ get => ScoreManager.Instance.TimePlayed;}

    Action onEndingTransitionComplete;

    private void Awake()
    {
        #region singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        #region getcomponents
        button2Continue = ContinueButton.GetComponent<Button>();
        button2LastCheckpoint = LastCheckPointButton.GetComponent<Button>();

        pausePanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        uIButtons = pausePanel.GetChild(1).GetComponentsInChildren<Button>(); 
        initPos = pausePanel.anchoredPosition;
        eventSystem = GetComponentInChildren<EventSystem>();
        #endregion

        scoreRectsInitPos = new Vector2[scoreTexts.Length];
        for (int i = 0; i < scoreRectsInitPos.Length; i++)
        {
            scoreRectsInitPos[i] = scoreTexts[i].rectTransform.anchoredPosition;
        }
    }

    private void Start()
    {
        endingAnimationCamera = SceneController.Instance.EndingAnimationVirtualCam;
        trainSprite = SceneController.Instance.Train;
        spritesPersonajes = SceneController.Instance.SpritePersonajes;
        float gameplayValue;
        float uiValue;
        AudioManager.Instance.mixers[(int)MixerChannel.Gameplay].audioMixer.GetFloat("mixerVol",out gameplayValue);
        AudioManager.Instance.mixers[(int)MixerChannel.UI].audioMixer.GetFloat("mixerVol",out uiValue);
        gameplayVolumeSlider.value = gameplayValue;
        uiVolumeSlider.value = uiValue;


        InputController.Instance.Pause += Pause;
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
        #region logicPause
        if (!GameManager.Instance.IsPaused)
        {
            GameManager.Instance.Pause();
            //pausePanel.gameObject.SetActive(true);
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
                //pausePanel.gameObject.SetActive(false);
                hasEndedTransition = true;
            });
        }
        #endregion
    }

    #region todeletesoon
    void EvaluateBottonOne()
    {
        //ContinueButton.gameObject.SetActive(BasicCharacter.Instance.IsAlive);
        //LastCheckPointButton.gameObject.SetActive(!BasicCharacter.Instance.IsAlive);
        if (BasicCharacter.Instance.IsAlive)
        {

            eventSystem.SetSelectedGameObject(ContinueButton.gameObject);
        }
        //else
        //{
        //    eventSystem.SetSelectedGameObject(LastCheckPointButton.gameObject);
        //}
    }

    //public void LastCheckPoint()
    //{
    //    SceneController.Instance.GoToLastCheckpoint();
    //    Pause();
    //}
    #endregion


    public void StartLevelTransition(string levelName)
    {
        transitionImage.color = new Color(0, 0, 0, 1);

        transitionImage.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(placeholderText.DOText(levelName, 1f).OnUpdate(() => { realText.text = placeholderText.text; }))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                realText.DOColor(new Color(0, 0, 0, 0), 2f);
                transitionImage.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() =>
                {
                    transitionImage.gameObject.SetActive(false);
                    canPause = true;
                    BasicCharacter.Instance.Activate();
                });
            });
    }

    public void NextLevelTransition(System.Action onEnd)///ACA VAN A IR LA TRANSICIÓN DEL SCORE FINAL Y EL TREN FINAL
    {

        spritesPersonajes[(int)GameManager.Instance.Current.character].gameObject.SetActive(true);

        onEndingTransitionComplete = onEnd;
        endingTransitionImage.gameObject.SetActive(true);
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].rectTransform.anchoredPosition = Vector2.left * 2000 + Vector2.up * scoreTexts[i].rectTransform.anchoredPosition;
        }
        BasicCharacter.Instance.Deactivate();
        endingTransitionImage.DOColor(new Color(0, 0, 0, 1), 4f).OnComplete(()=> { textTween(0); });
    }

    void textTween(int index)
    {
        Debug.Log("INDEX: " + index);
        scoreTexts[index].rectTransform.DOAnchorPos(scoreRectsInitPos[index], 1).SetEase(Ease.InBounce).OnComplete(() => {
            if(index+1 < scoreTexts.Length)
            {
                textTween(index + 1);
            }
            else
            {
                Debug.Log("EEEEEEEND RECURSION");
                ContinueObj.gameObject.SetActive(true);
                InputController.Instance.Jump += StartEndingAnimationSequence;
            }
        });
    }

    void StartEndingAnimationSequence()
    {
        ContinueObj.gameObject.SetActive(false);
        InputController.Instance.Jump -= StartEndingAnimationSequence;
        
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].DOColor(new Color(0, 0, 0, 0), 1f);
        }
        DOTween.Sequence()
            .Append(DOVirtual.DelayedCall(1f, null).OnComplete(() => { endingAnimationCamera.Priority = 100; }))
            .Append(endingTransitionImage.DOColor(new Color(0,0,0,0),1.5f))
            .Append(trainSprite.DOMoveX(trainSprite.transform.position.x + 130, 1f).SetEase(Ease.Linear).OnComplete(()=> {
                spritesPersonajes[(int)GameManager.Instance.Current.character].gameObject.SetActive(false);
            }))
            .Append(trainSprite.DOMoveX(trainSprite.transform.position.x + 260, 1f).SetEase(Ease.Linear))
            .Append(endingTransitionImage.DOColor(new Color(0, 0, 0,1), 0.5f).OnComplete(()=> {onEndingTransitionComplete?.Invoke();}));
        
    }
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
    public void MainScreen()
    {
        GameManager.Instance.MainScreen();
    }

    public void ChangeGameMixerVolume()
    {
        AudioManager.Instance.SetMixerVolume(MixerChannel.Gameplay, gameplayVolumeSlider.value);
    }
    public void ChangeUIMixerVolume()
    {
        AudioManager.Instance.SetMixerVolume(MixerChannel.UI, uiVolumeSlider.value);
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