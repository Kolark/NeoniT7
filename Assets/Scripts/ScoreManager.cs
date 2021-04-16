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
    public int Score { get => savedScore + score; }
    public float TimePlayed { get => savedTimePlayed + timePlayed; }


    private int savedScore;
    private float savedTimePlayed;

    private float timePlayed;
    private int score;

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

    private void Start()
    {
        ChamberManager.Instance.onChamberUpdate += onChamberUpdated;
    }
    private void Update()
    {
        if (GameManager.Instance.IsPaused || !BasicCharacter.Instance.IsAlive) return;
        timePlayed += Time.deltaTime;
    }


    public void ResetUnsavedValues()
    {
        score = 0;
        timePlayed = 0;
    }


    public void onChamberUpdated(int chamber)
    {
        savedScore = score;
        savedTimePlayed = timePlayed;
        GameManager.Instance.SetScore(savedScore, timePlayed);
        ResetUnsavedValues();
        GameManager.Instance.Save();
    }

    public void AddScore(Vector3 position,int toAdd)
    {
        score += toAdd;
        scoreText.text = Score.ToString();
        GameObject toInstance = Instantiate(popUpScore,position, Quaternion.identity);
        PopUpScore popUp = toInstance.GetComponent<PopUpScore>();
        popUp.SetScore(toAdd);
    }



}
