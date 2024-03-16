using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]  GameObject _pauseMenu;

    [SerializeField] private float _countdownTime = 3.5f;
    [SerializeField] private GameObject _countdown;
    TextMeshProUGUI _countdownText = null;
    bool exiting;
    float counter;
    
    public void enterPause()
    {
        exiting = false;
        _pauseMenu.SetActive(true);
        _countdown.SetActive(false);
        // no pasa el tiempo (es como si estuviera en pausa)
        Time.timeScale = 0;
    }
    public void quitPause()
    {
        _pauseMenu.SetActive(false);
        _countdown.SetActive(true);
        exiting = true;

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
        quitPause();
        SceneManager.LoadScene("Main Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        quitPause();

        _countdownText = _countdown.GetComponent<TextMeshProUGUI>();
        _countdown.SetActive(false);
        exiting = false;
        Time.timeScale = 1;
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
                _countdown.SetActive(false);
                exiting = false;
                Time.timeScale = 1;
                Debug.Log("Unpaused");
            }
        }
    }
}
