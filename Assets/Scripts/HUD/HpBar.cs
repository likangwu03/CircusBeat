using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    #region references
    [SerializeField]
    private Slider slider;
    #endregion

    #region methods
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }
    #endregion
}
