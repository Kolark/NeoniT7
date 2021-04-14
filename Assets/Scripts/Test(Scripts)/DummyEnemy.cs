using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DummyEnemy : MonoBehaviour, IEnemyHurtBox
{
    SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    public void OnReceiveDamage()
    {
        Debug.Log("enemy attacked");
        ScoreManager.Instance.AddScore(this.transform.position,100);
        rend.DOColor(Color.red, 0.15f).OnComplete(()=> { rend.DOColor(Color.white, 0.15f);});
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<PlayerDamageHandler>().OnReceiveDamage();
        }
    }
}
