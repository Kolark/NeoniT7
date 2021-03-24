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
    bool hasPerformedStart = false;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        cooldownAbilityUlti.INIT(BasicCharacter.Instance.UltAbility);
        cooldownAbilityThrow.INIT(BasicCharacter.Instance.ThrowAbility);
        cooldownAbilityDefense.INIT(BasicCharacter.Instance.DefenseAbility);
        BasicCharacter.Instance.onDefenseAbility += cooldownAbilityDefense.CoolDownAnimation;
        BasicCharacter.Instance.onThrowAbility += cooldownAbilityThrow.CoolDownAnimation;
        BasicCharacter.Instance.onUltAbility += cooldownAbilityUlti.CoolDownAnimation;
        hasPerformedStart = transform;
    }


    private void OnEnable()
    {
        if (!hasPerformedStart) return;
        BasicCharacter.Instance.onDefenseAbility += cooldownAbilityDefense.CoolDownAnimation;
        BasicCharacter.Instance.onThrowAbility += cooldownAbilityThrow.CoolDownAnimation;
        BasicCharacter.Instance.onUltAbility += cooldownAbilityUlti.CoolDownAnimation;
    }



    private void OnDestroy()
    {
        BasicCharacter.Instance.onDefenseAbility -= cooldownAbilityDefense.CoolDownAnimation;
        BasicCharacter.Instance.onThrowAbility -= cooldownAbilityThrow.CoolDownAnimation;
        BasicCharacter.Instance.onUltAbility -= cooldownAbilityUlti.CoolDownAnimation;
    }
}
