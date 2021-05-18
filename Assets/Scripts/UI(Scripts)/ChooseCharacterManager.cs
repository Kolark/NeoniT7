using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChooseCharacterManager : MonoBehaviour
{

    private static ChooseCharacterManager instance;
    public static ChooseCharacterManager Instance { get => instance;}
    [SerializeField] Image currentCharacter;
    [SerializeField] Sprite[] characters;
    [SerializeField] RectTransform[] characterInfo;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    
    public void ChangeVisual(CharacterType type)
    {
        currentCharacter.sprite = characters[(int)type];
        for (int i = 0; i < characterInfo.Length; i++)
        {
            characterInfo[i].gameObject.SetActive(false);
        }
        characterInfo[(int)type].gameObject.SetActive(true);
        Debug.Log("Changevisual: " + type.ToString());
    }

}
