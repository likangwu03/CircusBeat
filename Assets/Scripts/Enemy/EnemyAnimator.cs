using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    #region references
    [SerializeField]
    private Animator pAnimator;

    TrackerComponent trackerComp;
    #endregion

    #region methods
    private void Start()
    {
        trackerComp = TrackerComponent.Instance;
    }

    public void at_B()
    {
        // TRACKER EVENT fase 1
        if (trackerComp != null && trackerComp.Tracker != null)
        {
            trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(0));
        }

        pAnimator.ResetTrigger("At_R");
        pAnimator.ResetTrigger("Idle");
        pAnimator.SetTrigger("At_B");
    }

    public void at_R()
    {
        // TRACKER EVENT fase 2
        if (trackerComp != null && trackerComp.Tracker != null)
        {
            trackerComp.SendEvent(trackerComp.Tracker.CreatePhaseChangeEvent(1));
        }

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
}
