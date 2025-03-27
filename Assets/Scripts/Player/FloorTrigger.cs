using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorTrigger : MonoBehaviour {
    
    public UnityAction accionEntrar = null, accionSalir = null;

    private void OnTriggerEnter(Collider collision) {
        if(!collision.isTrigger) accionEntrar?.Invoke();
    }

    private void OnTriggerExit(Collider collision) {
        if(!collision.isTrigger) accionSalir?.Invoke();
    }
}
