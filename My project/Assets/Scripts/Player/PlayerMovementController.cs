using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform trans;

    [SerializeField]
    private float[] positions;

    private int lane = 2;

    public int Lane {
        get => lane;
        private set {
            lane = value;
            trans.position = new Vector3(positions[lane], trans.position.y, trans.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if(Lane < 4) Lane++;
        }
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(Lane > 0) Lane--;
        }
    }
}
