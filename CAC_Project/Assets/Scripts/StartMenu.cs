using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject HowToMenu;

    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void LoadHelpMenu() 
    {
        MainMenu.SetActive(false);
        HowToMenu.SetActive(true);
    }
    public void HelpToMain() 
    {
        MainMenu.SetActive(true);
        HowToMenu.SetActive(false);
    }
}
