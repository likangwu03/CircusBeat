using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField]
    private float jumpingColliderHeight = 1.7f;
    private Animator anim;
    private float initialColliderHeight;

    private void Start() {
        boxCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        initialColliderHeight = boxCollider.center.y;
    }

    private void Update() {
        AnimatorStateInfo aa = anim.GetCurrentAnimatorStateInfo(0);
        if(aa.IsName("BunnyJump")) {
            boxCollider.center = new Vector3(boxCollider.center.x, jumpingColliderHeight, boxCollider.center.z);
        } else {
            boxCollider.center = new Vector3(boxCollider.center.x, initialColliderHeight, boxCollider.center.z);
        }
    }
}
