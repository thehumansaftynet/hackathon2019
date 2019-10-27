using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickInteraction : MonoBehaviour {

    public UnityEvent customEvent;

    void OnMouseDown()
    {
        Debug.Log("Clicked " + gameObject.name);
        customEvent.Invoke();
    }
}
