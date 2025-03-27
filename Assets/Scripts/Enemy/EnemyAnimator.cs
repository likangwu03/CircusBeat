using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    #region references
    [SerializeField]
    private Animator pAnimator;
    #endregion

    #region methods
    public void at_B()
    {
        pAnimator.ResetTrigger("At_R");
        pAnimator.ResetTrigger("Idle");
        pAnimator.SetTrigger("At_B");
    }

    public void at_R()
    {
        pAnimator.ResetTrigger("At_B");
        pAnimator.ResetTrigger("Idle");
        pAnimator.SetTrigger("At_R");
    }

    public void e_Idle()
    {
        pAnimator.ResetTrigger("At_B");
        pAnimator.ResetTrigger("At_R");
        pAnimator.SetTrigger("Idle");
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
