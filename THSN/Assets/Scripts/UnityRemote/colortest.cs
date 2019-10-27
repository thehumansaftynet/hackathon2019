using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colortest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RemoteScript removeScript = FindObjectOfType<RemoteScript>();
        removeScript.ButtonPressed.AddListener(changeColor);
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void changeColor(int arg0)
    {
        switch (arg0)
        {
            
            case 1:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case 4:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
