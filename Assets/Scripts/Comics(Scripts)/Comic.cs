using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Comic : MonoBehaviour
{
    [SerializeField] Hoja hoja;


    private void Start()
    {
        InputController.Instance.Jump += Step;
        InputController.Instance.SpecialAbility += Skip;
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
        SceneController.Instance.NextLevel();
        
    }
    public void Step()
    {
        hoja.NextVignete();
    }
    

}
