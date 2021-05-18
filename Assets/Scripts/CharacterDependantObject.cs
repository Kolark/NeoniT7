using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDependantObject : MonoBehaviour
{
    Transform[] characterOBJ;
    private void Awake()
    {
        characterOBJ = new Transform[3];
        for (int i = 0; i < characterOBJ.Length; i++)
        {
            characterOBJ[i] = transform.GetChild(i);
            characterOBJ[i].gameObject.SetActive(false);
        }
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        characterOBJ[(int)GameManager.Instance.Current.character].gameObject.SetActive(true);
    }
}
