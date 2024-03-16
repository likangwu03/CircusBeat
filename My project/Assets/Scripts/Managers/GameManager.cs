using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player { get; private set; }

    [SerializeField]
    GameObject ComboNumber;
    private TMP_Text ComboNumberText;

    int score = 0;

    // Primero en llamrase
    private void Awake()
    {
        // Patron Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Segundo en llamarse (cuando el objeto se activa)
    private void OnEnable()
    {
        // Aregar un metodo a esta notificacion
        // (sus metodos se ejecutan cada vez que se carga una escena)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        ComboNumberText = ComboNumber.GetComponent<TMP_Text>();
    }

    // Tercero en llamarse
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.Find("Player");
    }

    // Se llama cuando el objeto se desactiva
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void addScore(int score)
    {
        this.score += score;
        UpdateScore();
        Debug.Log("Score:" + this.score);
    }

    public void UpdateScore()
    {
        ComboNumberText.text = score.ToString();
    }
}
