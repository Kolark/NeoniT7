using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Comic : MonoBehaviour
{
    private static Comic instance;
    public static Comic Instance => instance;

    [SerializeField] Hoja hoja;
    Action onComicCompletion;


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
    }

    public void INIT_COMIC(Action onComicEnd)
    {
        InputController.Instance.Jump += Step;
        InputController.Instance.SpecialAbility += Skip;
        onComicCompletion += onComicEnd;
        hoja.onCompleted += onCompleted;
    }


    public void Skip()
    {
        hoja.onCompleted?.Invoke();
    }
    public void onCompleted()
    {
        InputController.Instance.Jump -= Step;
        InputController.Instance.SpecialAbility -= Skip;
        gameObject.SetActive(false);
        onComicCompletion?.Invoke();        
    }
    public void Step()
    {
        hoja.NextVignete();
    }

    private void OnDestroy()
    {
        InputController.Instance.Jump -= Step;
        InputController.Instance.SpecialAbility -= Skip;
    }
}
