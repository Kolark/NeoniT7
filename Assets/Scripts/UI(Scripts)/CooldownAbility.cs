using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class CooldownAbility
{
    public RectTransform parent;
    Image abilityImage;
    TextMeshProUGUI secondsDisplay;
    public void INIT(Sprite sprite)
    {
        abilityImage = parent.GetChild(0).GetComponent<Image>();
        secondsDisplay = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        abilityImage.sprite = sprite;
    }

    public void CoolDownAnimation(float durationInSeconds)
    {
        abilityImage.DOFade(0.4f, 0.15f);
        float timer = 0;
        DOVirtual.DelayedCall(durationInSeconds, () =>
        {
            abilityImage.DOFade(1f, 0.1f);
            secondsDisplay.text = "";
        })
        .OnUpdate(() =>
        {
            timer += Time.deltaTime;
            secondsDisplay.text = (durationInSeconds - timer).ToString("f1");
        });
    }
}
