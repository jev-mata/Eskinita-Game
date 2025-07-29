using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string story;
    public string endless;
    public string mainmenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);

        // Play the main menu music when this scene loads
        SoundManager.Instance.PlayMusic("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Endless()
    {
        SceneManager.LoadScene(endless);
        SoundManager.Instance.PlayMusic("InGame"); // play endless ingame music
    }
    public void Story()
    {
        SceneManager.LoadScene(story);
    }
    public void MainMenu()
    {
        SoundManager.Instance.PlayMusicInstant("MainMenu");
        SceneManager.LoadScene(mainmenu);
    }

    public void ApplicationExit()
    {
        Application.Quit();
    }
}
