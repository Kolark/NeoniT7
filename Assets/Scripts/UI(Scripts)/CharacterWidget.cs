using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWidget : UIWidget
{
    [SerializeField] CharacterType character;
    protected override void OnPointerDown()
    {
        AudioManager.Instance.Play("Nota-001");
        GameManager.Instance.SetCharacter(character);
        GameManager.Instance.ChangeScene(sceneToGo);

    }

    protected override void OnSelect()
    {
        base.OnSelect();
        ChooseCharacterManager.Instance.ChangeVisual(character);
    }
}
