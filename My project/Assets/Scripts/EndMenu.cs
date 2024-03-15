using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void toMainMenu()
    {
        Debug.Log("AAAAAAAAA");
        SceneManager.LoadScene("Main Menu");

    }

    public void quitGame()
    {
        Application.Quit();
    }
}
