using TMPro;
using UnityEngine;

public class PlayerLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int maxLife = 3;
    private int currentLife;
    #endregion

    #region references
    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    private GameObject obj;
    private TMP_Text text;

    TrackerComponent trackerComp;

    #endregion


    #region methods
    // Start is called before the first frame update
    void Start()
    {
        text = obj.GetComponent<TMP_Text>();

        currentLife = maxLife;

        text.text = maxLife.ToString();

        trackerComp = TrackerComponent.Instance;
    }

    public void damage()
    {
        if (currentLife > 0)
            currentLife -= 1;
        text.text = currentLife.ToString();

        if (currentLife <= 0)
        {
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<PlayerMovementController>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
            Invoke("startGameOver", 3.0f);
        }
    }
    private void startGameOver()
    {
        GameManager.Instance.startGameOver();
    }
    public int getLife() { return currentLife; }
    public void heal()
    {
        if (currentLife < maxLife)
        {
            currentLife += 1;

            // TRACKER EVENT Recuperar vida
            if (trackerComp != null && trackerComp.Tracker != null)
            {
                trackerComp.SendEvent(trackerComp.Tracker.CreateGenericGameEvent(GameEventType.RECOVER_HEALTH));
            }

            text.text = currentLife.ToString();

        }
    }
    public void restart() { currentLife = 3; }
    #endregion

}
