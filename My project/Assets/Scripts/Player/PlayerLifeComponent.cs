using System.Collections;
using System.Collections.Generic;
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
    private HpBar hpBar;
    #endregion

    #region methods
    public void damage() {
        if (currentLife > 0)
            currentLife -= 1;
        hpBar.SetHealth(currentLife);

        if(currentLife <= 0)
        {
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<PlayerMovementController>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
            Invoke("startGameOver", 3.0f);
        }
    }
    private void startGameOver()
    {
        GameManager.instance.startGameOver();
    }
    public int getLife() { return currentLife; }
    public void heal() { 
        if(currentLife < maxLife)
            currentLife += 1;
        hpBar.SetHealth(currentLife);
    }
    public void restart() { currentLife = 3; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hpBar = GameObject.Find("HP_Bar").GetComponent<HpBar>();

        currentLife = maxLife;

        hpBar.SetMaxHealth(maxLife);
        hpBar.SetHealth(currentLife);
    }
}
