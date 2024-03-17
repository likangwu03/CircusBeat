using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform trans;
    bool col = false;

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
    }

    // se llama cuando el objeto se sale de la camara
    private void OnBecameInvisible()
    {
        if (!col)
        {
            Destroy(this.gameObject);
            GameManager.instance.addScore(score);
        }
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            GameManager.instance.setScore(0);
            GameManager.instance.pLC.damage();
            Destroy(gameObject);
            col = true;
        }
    }
}
