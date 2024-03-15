using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    #region references
    [SerializeField]
    private GameObject lifeBar;
    #endregion

    #region methods
    public void SetPlayerLifeBar(bool enabled)
    {
        lifeBar.SetActive(enabled);
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
