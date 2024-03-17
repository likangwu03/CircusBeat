using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("InGame");

    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");

    }

}
