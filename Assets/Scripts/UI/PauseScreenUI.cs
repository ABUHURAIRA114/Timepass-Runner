using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] private RectTransform pauseScreen;

    private void Update() 
    {
        PauseUI();
    }

    private void PauseUI()
    {
        if(GameManager.instance.GetState() == GameState.Paused)
            pauseScreen.gameObject.SetActive(true);
        else
            pauseScreen.gameObject.SetActive(false);
    }

    public void Restart()
    {
        StartCoroutine(GameManager.instance.Restart());
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        if(GameManager.instance.GetState() == GameState.Started || GameManager.instance.GetState() == GameState.Dead) return;
        
        if(GameManager.instance.GetState() == GameState.Scenic)
        {
            SceneManager.LoadScene(0);
            return;
        }
        GameManager.instance.SetState(GameState.Playing);
    }
}
