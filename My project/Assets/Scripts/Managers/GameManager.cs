using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player { get; private set; }

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
        Debug.Log("Score:" + this.score);
    }
}
