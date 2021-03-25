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
    bool hasPerformedStart = false;
    Image[] chambers;
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
        InstatiateRooms();
        hasPerformedStart = transform;
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

    void onChamberIncrease(int current)
    {
        chambers[current].sprite = unlockedChamberSprite;
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
