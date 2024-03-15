using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;

    public void enterPause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void quitPause()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private void setPause()
    {
        if (_pauseMenu.active) quitPause();
        else enterPause();
    }

    public void toMainMenu()
    {
        quitPause();
        SceneManager.LoadScene("Main Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        quitPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) setPause();
    }
}
