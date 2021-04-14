using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    [SerializeField] GameObject popUpScore;
    TextMeshProUGUI scoreText;
    public int Score { get => score;}
    public float TimePlayed { get => timePlayed;}

    private int score;
    private float timePlayed;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }


    private void Update()
    {
        if (GameManager.Instance.IsPaused || !BasicCharacter.Instance.IsAlive) return;
        timePlayed += Time.deltaTime;
    }

    public void AddScore(Vector3 position,int toAdd)
    {
        score += toAdd;
        scoreText.text = score.ToString();
        GameObject toInstance = Instantiate(popUpScore,position, Quaternion.identity);
        Debug.Log("pos: - " + position);
        PopUpScore popUp = toInstance.GetComponent<PopUpScore>();
        popUp.SetScore(toAdd);
    }



}
