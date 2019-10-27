using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour {

    public GameObject target;
    public float yOffset = -3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        pos.y = target.transform.position.y + yOffset;
        transform.position = pos;
	}
}
