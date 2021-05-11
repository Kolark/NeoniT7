using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    [SerializeField] CooldownAbility cooldownAbilityUlti;
    [SerializeField] CooldownAbility cooldownAbilityThrow;
    [SerializeField] CooldownAbility cooldownAbilityDefense;
    [SerializeField]Image[] lifes;
    [SerializeField] Sprite unlockedChamberSprite;
    [SerializeField] RectTransform enemyChamberIMG;
    [SerializeField] RectTransform currentChamberIndicator;
    [SerializeField] RectTransform GoObject;
    bool hasPerformedStart = false;
    Image[] chambers;


    private void Awake()
    {
        if (GameManager.Instance.Current.difficulty == Difficulty.Hardcore)
        {
            Image[] tochange = new Image[1];
            tochange[0] = lifes[0];
            for (int i = 1; i < lifes.Length; i++)
            {
                Destroy(lifes[i].gameObject);
            }
            lifes = tochange;
        }
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);

        cooldownAbilityUlti.INIT(BasicCharacter.Instance.UltAbility);
        cooldownAbilityThrow.INIT(BasicCharacter.Instance.ThrowAbility);
        cooldownAbilityDefense.INIT(BasicCharacter.Instance.DefenseAbility);
        BasicCharacter.Instance.onDefenseAbility += cooldownAbilityDefense.CoolDownAnimation;
        BasicCharacter.Instance.onThrowAbility += cooldownAbilityThrow.CoolDownAnimation;
        BasicCharacter.Instance.onUltAbility += cooldownAbilityUlti.CoolDownAnimation;
        BasicCharacter.Instance.onLifeChange += SetLifes;
        ChamberManager.Instance.onChamberUpdate += onChamberIncrease;
        ChamberManager.Instance.onChamberUnlocked += OnLevelUnlocked;
        ChamberManager.Instance.OnCurrentChamberChange += OnChamberChange;
        InstatiateRooms();
        SetChambers();
        OnLevelUnlocked();
        DOVirtual.DelayedCall(0.25f, () => { OnChamberChange(ChamberManager.Instance.CurrentChamber); });
        
        hasPerformedStart = transform;
    }

    void OnChamberChange(int current)
    {
        Debug.Log("Current: " + current);
        currentChamberIndicator.anchoredPosition = chambers[current].rectTransform.anchoredPosition;
    }
    void InstatiateRooms()
    {
        int nRooms = ChamberManager.Instance.ChamberLength;
        Transform parent = enemyChamberIMG.parent;
        for (int i = 0; i < nRooms-2; i++)
        {
            Instantiate(enemyChamberIMG, parent);
        }
        chambers = parent.GetComponentsInChildren<Image>();
    }
    void OnLevelUnlocked()
    {
        GoObject.gameObject.SetActive(true);
    }
    void onChamberIncrease(int current)
    {
        chambers[current].sprite = unlockedChamberSprite;
        GoObject.gameObject.SetActive(false);
    }
    void SetChambers()
    {
        for (int i = 0; i < ChamberManager.Instance.UnlockedChambers+1; i++)
        {
            onChamberIncrease(i);
        }
    }
    void SetLifes(int currentLife)
    {
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].enabled = false;
        }
        for (int i = 0; i < currentLife; i++)
        {
            Debug.Log("i: " + i);
            lifes[i].enabled = true;
        }
    }

    

    private void OnEnable()
    {
        if (!hasPerformedStart) return;
        BasicCharacter.Instance.onDefenseAbility += cooldownAbilityDefense.CoolDownAnimation;
        BasicCharacter.Instance.onThrowAbility += cooldownAbilityThrow.CoolDownAnimation;
        BasicCharacter.Instance.onUltAbility += cooldownAbilityUlti.CoolDownAnimation;
        BasicCharacter.Instance.onLifeChange += SetLifes;
        ChamberManager.Instance.onChamberUpdate += onChamberIncrease;
    }



    //private void OnDestroy()
    //{
    //    BasicCharacter.Instance.onDefenseAbility -= cooldownAbilityDefense.CoolDownAnimation;
    //    BasicCharacter.Instance.onThrowAbility -= cooldownAbilityThrow.CoolDownAnimation;
    //    BasicCharacter.Instance.onUltAbility -= cooldownAbilityUlti.CoolDownAnimation;
    //    BasicCharacter.Instance.onLifeChange -= SetLifes;
    //    ChamberManager.Instance.onChamberUpdate -= onChamberIncrease;
    //}
}
