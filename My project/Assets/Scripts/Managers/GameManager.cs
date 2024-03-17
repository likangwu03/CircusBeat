using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player { get; private set; }
    public PlayerLifeComponent pLC { get; private set; }

    [SerializeField]
    GameObject ComboNumber;
    private TMP_Text ComboNumberText;

    [SerializeField]
    private int streak = 10;
    private int score = 0;
    //private bool isStreak = false;

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
        // no hace falta el if
        // si no lo encuentra, es null y no se usa
        pLC = player.GetComponent<PlayerLifeComponent>();
    }

    // Se llama cuando el objeto se desactiva
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void streakHeal()
    {
        //if (this.score > streak && isStreak)
        if (this.score % streak == 0 && this.score != 0)
        {
            pLC.heal();
            //isStreak = false;
        }
    }

    public void addScore(int score)
    {
        this.score += score;
        //if (this.score % streak == 0)
        //{
        //    isStreak = true;
        //}
        streakHeal();
        UpdateScore();
    }

    public void setScore(int score)
    {
        //if (score == 0)
        //{
        //    isStreak = false;
        //}
        this.score = score;
        UpdateScore();
    }

    public void UpdateScore()
    {
        ComboNumberText.text = score.ToString();
    }
}
