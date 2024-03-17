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

    private TMP_Text comboNumberText;
    private TMP_Text scoreNumberText;
    private GameObject comboGO;
    private Animator comboAnimator;

    [SerializeField]
    private AudioClip mainAudio;
    [SerializeField]
    private int streak = 10;
    private int combo;
    private int score;
    //private bool isStreak = false;

    private AudioSource speaker;

    public GameObject player { get; private set; }
    public PlayerLifeComponent pLC { get; private set; }

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

    private void Start()
    {
        combo = 0; score = 0;
    }

    // Segundo en llamarse (cuando el objeto se activa)
    private void OnEnable()
    {
        // Aregar un metodo a esta notificacion
        // (sus metodos se ejecutan cada vez que se carga una escena)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void loadGame()
    {
        // JUEGO
        combo = 0; score = 0;
        player = GameObject.Find("Player");
        if (player != null)
        {
            pLC = player.GetComponent<PlayerLifeComponent>();
        }
        GameObject comboNumber = GameObject.Find("ComboNumberText");
        if (comboNumber != null)
        {
            comboNumberText = comboNumber.GetComponent<TMP_Text>();
        }
        GameObject scoreNumber = GameObject.Find("ScoreNumberText");
        if (comboNumber != null)
        {
            scoreNumberText = scoreNumber.GetComponent<TMP_Text>();
        }
        GameObject comboImage = GameObject.Find("Image");
        if (comboImage != null)
        {
            comboAnimator = comboImage.GetComponent<Animator>();
        }
        comboGO = GameObject.Find("Combo");
        if (comboGO != null)
        {
            comboGO.SetActive(false);
        }
    }

    private void loadMenu()
    {
        GameObject playIcon = GameObject.Find("Play Icon");
        if (playIcon != null)
        {
            Button playButton = playIcon.GetComponent<Button>();
            playButton.onClick.AddListener(() => this.startGame());
        }
        GameObject exitIcon = GameObject.Find("Exit Icon");
        if (exitIcon != null)
        {
            Button exitButton = exitIcon.GetComponent<Button>();
            exitButton.onClick.AddListener(() => this.quit());
        }
        GameObject creditsIcon = GameObject.Find("Credits Icon");
        if (creditsIcon != null)
        {
            Button creditsButton = creditsIcon.GetComponent<Button>();
            creditsButton.onClick.AddListener(() => this.startCredits());
        }
    }

    private void loadGameOver()
    {
        GameObject mainMenuIcon = GameObject.Find("Main Menu Icon");
        if (mainMenuIcon != null)
        {
            Button mainMenuButton = mainMenuIcon.GetComponent<Button>();
            mainMenuButton.onClick.AddListener(() => this.startMenu());
        }
        GameObject exitIcon = GameObject.Find("EXIT");
        if (exitIcon != null)
        {
            Button exitButton = exitIcon.GetComponent<Button>();
            exitButton.onClick.AddListener(() => this.quit());
        }
    }

    private void loadWin()
    {
        GameObject mainMenuIcon = GameObject.Find("Main Menu Icon");
        if (mainMenuIcon != null)
        {
            Button mainMenuButton = mainMenuIcon.GetComponent<Button>();
            mainMenuButton.onClick.AddListener(() => this.startMenu());
        }
    }

    private void winGame()
    {
        if (pLC.getLife() > 0)
        {
            startWin();
        }
    }

    // Tercero en llamarse
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "InGame 2")
        {
            speaker = SoundManager.instance.GetComponent<AudioSource>();
            speaker.clip = mainAudio;
            stopMusic();
            playMusic();
            CancelInvoke("winGame");
            Invoke("winGame", mainAudio.length);
        }
        loadGame();
        loadMenu();
        loadGameOver();
        loadWin();
    }

    // Se llama cuando el objeto se desactiva
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // TRANSICIONES
    public void startMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void startGame()
    {
        SceneManager.LoadScene("InGame 2");
    }
    public void startCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void startGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void startWin()
    {
        SceneManager.LoadScene("Win");
    }
    public void quit()
    {
        Application.Quit();
    }

    // MUSICA
    public void playMusic()
    {
        speaker.Play();
    }
    public void stopMusic()
    {
        speaker.Stop();
    }
    public void pauseMusic()
    {
        speaker.Pause();
    }
    public float musicTime()
    {
        return speaker.time;
    }

    public void addScore(int score)
    {
        this.score += score * ((combo / 10) + 1);
        updateScoreText();
    }

    // COMBO
    private void streakHeal()
    {
        //if (this.score > streak && isStreak)
        if (this.combo % streak == 0) /*&& this.score != 0)*/
        {
            pLC.heal();
            //isStreak = false;
        }
    }
    public void addCombo(int combo)
    {
        this.combo += combo;
        //if (this.score % streak == 0)
        //{
        //    isStreak = true;
        //}
        streakHeal();
        updateComboText();
    }
    public void setCombo(int combo)
    {
        if(combo == 0)
        {
            comboAnimator.SetTrigger("Hit");
        }
        this.combo = combo;
        updateComboText();
    }
    public void updateComboText()
    {
        if (comboGO == null)
            return;

        if (combo == 0)
        {
            comboGO.SetActive(false);
        }
        else if(combo > 0)
        {
            comboGO.SetActive(true);
        }

        if (comboAnimator != null)
        {
            comboAnimator.SetInteger("Combo", combo);
            comboNumberText.text = combo.ToString();            
        }
    }
    
    public void updateScoreText()
    {
        scoreNumberText.text = score.ToString();            
    }
}
