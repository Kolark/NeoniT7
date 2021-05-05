using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWidget : UIWidget
{
    protected override void OnPointerDown()
    {
        AudioManager.Instance.Play("Ui_Confirm");
        GameManager.Instance.ChangeScene(sceneToGo);
    }
}
