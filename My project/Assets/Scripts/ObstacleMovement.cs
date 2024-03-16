using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    float speed = 3.5f;

    void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            // Destroy the gameobject
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        print("Balls");
        //Notify gamemanager 
    }
}
