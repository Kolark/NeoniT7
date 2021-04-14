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
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        text.text = scoreText.ToString();
        transform.DOMove((Vector2)transform.position + Vector2.up * 2 + Vector2.right * UnityEngine.Random.Range(-1f, 1f), 0.35f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
