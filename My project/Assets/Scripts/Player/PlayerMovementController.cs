using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform trans;

    [SerializeField]
    private float[] positions;

    [SerializeField]
    private float jumpForce = 3.0f;
    
    private PlayerAnimations pAnimations;
    private Rigidbody rigid;

    [SerializeField]
    private FloorTrigger trigger;

    [SerializeField]
    private Transform highlight;

    private int lane = 2;

    public int Lane {
        get => lane;
        private set {
            lane = value;
            trans.position = new Vector3(positions[lane], trans.position.y, trans.position.z);
            highlight.position = new Vector3(positions[lane], highlight.position.y, highlight.position.z);
        }
    }

    public bool InGround { get; private set; } = true;

    void Start() {
        pAnimations = gameObject.GetComponent<PlayerAnimations>();
        trans = transform;
        rigid = GetComponent<Rigidbody>();
        trigger.accionEntrar = () => { InGround = true; };
    }

    void Update() {
        if (Time.timeScale > 0.0f) {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                if(InGround) {
                    InGround = false;
                    rigid.AddForce(new Vector2(0, jumpForce));
                    pAnimations.jump();
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                if (Lane < 4) {
                    pAnimations.dashDr();
                    Lane++;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (Lane > 0) {
                    pAnimations.dashIz();
                    Lane--;
                }
            }
        }
    }
}
