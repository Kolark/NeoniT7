using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
[System.Serializable]
public class Viñeta
{
    public Viñeta(RectTransform rect)
    {
        rectT = rect;
    }

    private static readonly Vector2[] Directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    [Header("Public Attributes")]
    public int tweenLength;
    public TweenEnterDirection directionEnter;
    public RectTransform rectT;
    public float DurationTween = 1;


    bool HasDoneTweening = false;
    bool hasCompletedAllTexts = false;
    int txtIndex=0;
    Vector2 posToGo;

    TextMeshProUGUI[] texts;

    public bool HasCompletedAllTexts { get => hasCompletedAllTexts;}

    public void DoTweening(Action action = null)
    {
        if (!HasDoneTweening)
        {
            rectT.DOAnchorPos(posToGo, DurationTween, false)
                .OnComplete(() =>
                {
                    action?.Invoke();
                });
            showText();
            HasDoneTweening = true;
        }
        else
        {
            showText();
        }
    }

    void showText()
    {
        if(txtIndex < texts.Length)
        {
            texts[txtIndex].DOFade(1, 0.25f);
            txtIndex++;
            hasCompletedAllTexts =  txtIndex == texts.Length;
        }
    }


    public virtual void INIT()
    {
        posToGo = rectT.anchoredPosition;
        rectT.anchoredPosition += Directions[(int)directionEnter]*tweenLength;
        texts = rectT.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = new Color(0, 0, 0, 0);
        }
    }
}
public enum TweenEnterDirection
{
    up,down,left,right
}