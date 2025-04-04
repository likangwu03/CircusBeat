using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    float skipSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Animator anim = GetComponent<Animator>();
            anim.speed = anim.speed * skipSpeed;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
