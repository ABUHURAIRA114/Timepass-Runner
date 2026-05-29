using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private enum Screen
    {
        None,
        SelectMenu,
        Settings,
        Tutorial,
        About
    }

    [SerializeField] private RectTransform loadScreen;
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform randomText;
    [SerializeField] private RectTransform tutorial;
    [SerializeField] private RectTransform settings;
    [SerializeField] private RectTransform about;
    [SerializeField] private RectTransform gameScreen;
    [SerializeField] private RectTransform menu;
    [SerializeField] private Toggle scenicToggle;

    [SerializeField] private List<GameObject> endlessMapSelectArrow;
    [SerializeField] private List<GameObject> arenaMapSelectArrows;

    private Screen currentScreen;
    private int lvl = 1;
    private int SlMenu = 0;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            currentScreen = Screen.None;
        GameManager.instance.SetScenic(scenicToggle.isOn);
        HandleUI();
    }

    public void LoadGame()
    {
        loadScreen.gameObject.SetActive(true);
        SceneManager.LoadScene(lvl);
    }

    public void LevelSet(int l)
    { lvl = l; }

    public void SelectMenuSet(int l)
    { SlMenu = l; }
    
    public void SelectScreenToggle()
    {
        if(currentScreen != Screen.SelectMenu) currentScreen = Screen.SelectMenu;
        else currentScreen = Screen.None;
    }

    public void SettingsToggle()
    {
        if(currentScreen != Screen.Settings) currentScreen = Screen.Settings;
        else currentScreen = Screen.None;
    }

    public void TutorialToggle()
    {
        if(currentScreen != Screen.Tutorial) currentScreen = Screen.Tutorial;
        else currentScreen = Screen.None;
    }
   
    public void AboutToggle()
    {
        if(currentScreen != Screen.About) currentScreen = Screen.About;
        else currentScreen = Screen.None;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void HandleUI()
    {        
        gameScreen.gameObject.SetActive(currentScreen == Screen.SelectMenu);
        menu.gameObject.SetActive(currentScreen != Screen.SelectMenu);
        about.gameObject.SetActive(currentScreen == Screen.About);
        settings.gameObject.SetActive(currentScreen == Screen.Settings);
        tutorial.gameObject.SetActive(currentScreen == Screen.Tutorial);
        title.gameObject.SetActive(currentScreen == Screen.None);
        randomText.gameObject.SetActive(currentScreen == Screen.None);

        foreach (GameObject arrow in endlessMapSelectArrow)
        {
            if (endlessMapSelectArrow.IndexOf(arrow) == lvl-1) arrow.SetActive(true);
            else arrow.SetActive(false);
        }

        foreach (GameObject arrow in arenaMapSelectArrows)
        {
            // if (arenaMapSelectArrows.IndexOf(arrow) == lvl-1) arrow.SetActive(true);
            /* else */ arrow.SetActive(false);
        }
    }
}
