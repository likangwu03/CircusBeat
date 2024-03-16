using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //private const float DEATH_THRESHOLD = 0.001f;

    private Transform trans;

    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private int score = 1;

    public Vector3 targetPos { private get; set; }

    private void Start()
    {
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        trans.position = Vector3.MoveTowards(trans.position, targetPos, step);

        // Check if the position of the cube and sphere are approximately equal.
        //if (Vector3.Distance(trans.position, target.transform.position) < DEATH_THRESHOLD)
        //{
        //    // Destroy the gameobject
        //    Destroy(this.gameObject);
        //}
    }

    private void OnDestroy()
    {
        //Notify gamemanager 
        if(GameManager.instance != null)
        {
            Debug.Log("Score:" + this.score);
            GameManager.instance.addScore(score);
        }
    }

    // se llama cuando el objeto se sale de la camara
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
