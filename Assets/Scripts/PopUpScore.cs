using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PopUpScore : MonoBehaviour
{
    TextMeshPro text;
    public void SetScore(int scoreText)
    {
        text = GetComponent<TextMeshPro>();
        text.text = scoreText.ToString();
        transform.DOMoveY(2, 0.35f).OnComplete(() => {
            Destroy(gameObject);
        });
    }

}
