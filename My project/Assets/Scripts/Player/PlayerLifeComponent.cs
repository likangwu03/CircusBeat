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
    private HpBar hpBar;
    #endregion

    #region methods
    public void damage() {
        if (currentLife > 0)
            currentLife -= 1;
        hpBar.SetHealth(currentLife);
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
        currentLife = maxLife;

        hpBar.SetMaxHealth(maxLife);
        hpBar.SetHealth(currentLife);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)){
            damage();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            heal();
        }
    }
}
