using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameState gameState;
    private float time = 0;
    public bool scenic { get; private set;}
    public bool unPaused{get; private set;}
    private bool wasPaused;

    private void Awake() {

        Random.InitState(2232007);

        if (instance == this) 
            return;

        if (instance == null) 
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private int sceneCount;
    private void Update() {

        StartCoroutine(UnPaused());

        if (SceneManager.GetActiveScene().buildIndex != sceneCount )
        {
            SavingSystem.i.Load("save1");

            if (SceneManager.GetActiveScene().buildIndex > 0) 
                gameState = scenic ? GameState.Scenic : GameState.Started;
            else
                gameState = GameState.None;

            Time.timeScale = 1;
            time = 0;
        }

        StartGame();
        PerformTask();
        PauseToggle();

        sceneCount = SceneManager.GetActiveScene().buildIndex;
    }

    private bool hasSaved, inRestartQueue;
    private void PerformTask() {

        if(gameState == GameState.Paused || gameState == GameState.None)
        {
            Time.timeScale = gameState == GameState.Paused?0:1;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(gameState != GameState.Paused)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        if((gameState == GameState.Dead || gameState == GameState.Paused) && !hasSaved)
        {
            SavingSystem.i.Save("save1");

            if (gameState == GameState.Dead && !inRestartQueue)
            {
                inRestartQueue = true;
                StartCoroutine(Restart());
            }
            hasSaved = true;
        }
        else if (gameState != GameState.Dead && gameState != GameState.Paused)
            hasSaved = false;

    }

    public IEnumerator Restart() {
        
        Time.timeScale = 1;
        gameState = GameState.Restarting;

        yield return new WaitForSeconds(0.01f);

        if (gameState == GameState.Dead) 
            yield return new WaitForSeconds(5);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gameState = GameState.Started;
        time = 0;
        inRestartQueue = false;
    }

    private void PauseToggle() {

        if(!Input.GetKeyDown(KeyCode.Escape)) return;

        if(gameState == GameState.Started || gameState == GameState.Dead || gameState == GameState.None) return;
        
        if(gameState == GameState.Scenic)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if(gameState == GameState.Paused)
            gameState = GameState.Playing;
        else
            gameState = GameState.Paused;
    }

    private void StartGame() {

        if (gameState != GameState.Started) return;
        
        time += Time.deltaTime;

        if (time >= 1) gameState = GameState.Playing;
    }

    private IEnumerator UnPaused()
    {
        if (gameState == GameState.Paused)
            wasPaused = true;
        if (gameState != GameState.Paused && wasPaused) 
        {
            unPaused = true;
            yield return new WaitForSeconds(0.2f);
            unPaused = false;
            wasPaused = false;
        }
    }

    public void SetState(GameState state) {

        time = 0;
        gameState = state;                                            
    }

    public void SetScenic(bool _scenic) {

        scenic = _scenic;
    }

    public GameState GetState()
    {
        return gameState;
    }
}

public enum GameState
{
    None,
    Started,
    Playing,
    Scenic,
    Paused,
    Restarting,
    Dead
}
