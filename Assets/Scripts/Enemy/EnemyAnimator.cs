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
        // TODO: evento cambio de fase
        pAnimator.ResetTrigger("At_R");
        pAnimator.ResetTrigger("Idle");
        pAnimator.SetTrigger("At_B");
    }

    public void at_R()
    {
        // TODO: evento cambio de fase
        pAnimator.ResetTrigger("At_B");
        pAnimator.ResetTrigger("Idle");
        pAnimator.SetTrigger("At_R");
    }

    public void e_Idle()
    {
        // TODO: evento cambio de fase
        pAnimator.ResetTrigger("At_B");
        pAnimator.ResetTrigger("At_R");
        pAnimator.SetTrigger("Idle");
    }
    #endregion
}
