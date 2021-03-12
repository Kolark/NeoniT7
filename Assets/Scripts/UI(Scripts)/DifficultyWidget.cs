using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyWidget : UIWidget
{
    [SerializeField] Difficulty difficulty;
    protected override void OnPointerDown()
    {
        AudioManager.Instance.Play("Nota-001");
        GameManager.Instance.SetDifficulty(difficulty);
        GameManager.Instance.Save();

        SaveSystem.debugSaveinfo(GameManager.Instance.Current);
        GameManager.Instance.ChangeScene(GameManager.Instance.Current.currentScene);

    }


}
