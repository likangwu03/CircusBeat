using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    #region references
    [SerializeField]
    private Animator pAnimator;
    #endregion

    #region methods
    public void dashIz()
    {
        pAnimator.ResetTrigger("DashDr");
        pAnimator.ResetTrigger("Jump");
        pAnimator.SetTrigger("DashIz");
    }

    public void dashDr()
    {
        pAnimator.ResetTrigger("DashIz");
        pAnimator.ResetTrigger("Jump");
        pAnimator.SetTrigger("DashDr");
    }

    public void jump()
    {
        pAnimator.ResetTrigger("DashIz");
        pAnimator.ResetTrigger("DashDr");
        pAnimator.SetTrigger("Jump");
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
