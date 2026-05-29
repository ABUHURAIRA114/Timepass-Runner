using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scores : MonoBehaviour, ISavable
{
    [Header("Scores")]
    [SerializeField] private int lvlMltplr;
    [SerializeField] private int baseMltplr;
    private int currentScore;
    [SerializeField] private List<int> highScore = new List<int>();
    private int currentScene;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI bgScoreUI;
    [SerializeField] private TextMeshProUGUI highScoreUI;
    [SerializeField] private TextMeshProUGUI bgHighScoreUI;
    private float scores;

    private void LateUpdate() 
    {
        if (GameManager.instance.GetState() == GameState.Started)
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;

            scores = 0;
            currentScore = (int)scores;
        }

        SetUI();

        SetScores();
    }

    private void SetScores()
    {
        if (GameManager.instance.GetState() == GameState.Dead || GameManager.instance.GetState() == GameState.Dead)
            return;

        scores += Time.deltaTime * baseMltplr * lvlMltplr;
        currentScore = (int)scores;
        
        if(currentScore > highScore[currentScene-1])
            highScore[currentScene-1] = currentScore;
    }

    private void SetUI()
    {
        scoreUI.text = currentScore.ToString();
        bgScoreUI.text = currentScore.ToString();

        highScoreUI.text = highScore[currentScene-1].ToString();
        bgHighScoreUI.text = highScore[currentScene-1].ToString();
    }

    public object CaptureState() 
    {
        CrossSceneData.i.highScores  = highScore;

        return highScore; 
    }

    public void RestoreState(object state)
    {
        highScore = (List<int>)state;

        CrossSceneData.i.highScores = highScore;
    }
}
