using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePresenter : MonoBehaviour
{
    public Text scoreText;

    private string score = "";

    private void Awake()
    {
        int numsScorePresenter = FindObjectsOfType<ScorePresenter>().Length;
        if (numsScorePresenter > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScoreText(string value)
    {
        score += value;
        scoreText.text = score;
    }
}