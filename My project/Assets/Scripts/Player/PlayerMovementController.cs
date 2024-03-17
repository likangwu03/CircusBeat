using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform trans;

    [SerializeField]
    private float[] positions;
    
    private PlayerAnimations pAnimations;

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
        pAnimations = gameObject.GetComponent<PlayerAnimations>();
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Lane < 4)
                {
                    pAnimations.dashDr();
                    Lane++;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Lane > 0)
                {
                    pAnimations.dashIz();
                    Lane--;
                }
            }
        }
    }
}
