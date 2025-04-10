using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject _pauseMenu;

    [SerializeField] private float _countdownTime = 3.5f;
    [SerializeField] private GameObject _countdown;
    TextMeshProUGUI _countdownText = null;
    bool exiting;
    float counter;

    public void enterPause()
    {
        exiting = false;
        _canvas.SetActive(true);
        _pauseMenu.SetActive(true);
        _countdown.SetActive(false);

        // no pasa el tiempo (es como si estuviera en pausa)
        Time.timeScale = 0;
        GameManager.Instance.pauseMusic();
    }

    public void quitPause()
    {
        exiting = true;
        _canvas.SetActive(true);
        _pauseMenu.SetActive(false);
        _countdown.SetActive(true);

        // + 1 porque el contador muestra su valor truncado,
        // y nada mas comienza la cuenta atras, _countdownTime
        // deja de ser el numero indicado y se pasa a mostrar
        // el entero anterior
        counter = _countdownTime + 1;

    }

    private void setPause()
    {
        if (_pauseMenu.activeSelf) quitPause();
        else enterPause();
    }

    public void toMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.startMenu();
        //TODO Abandono del nivel
    }


    // Start is called before the first frame update
    void Start()
    {
        _countdownText = _countdown.GetComponent<TextMeshProUGUI>();
        enterPause();
        quitPause();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) setPause();

        if (exiting)
        {
            _countdownText.text = ((int)counter).ToString();
            counter -= Time.unscaledDeltaTime;

            if (counter < 1)
            {
                _canvas.SetActive(false);
                _countdown.SetActive(false);
                exiting = false;
                Time.timeScale = 1;
                GameManager.Instance.playMusic();
            }
        }
    }
}
