using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithChild : MonoBehaviour {
    private Transform child;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        if (transform.childCount == 0)
        {
            Destroy(this);
            return;
        }
        child = transform.GetChild(0);
        offset = child.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        var sy = child.GetComponent<SpriteRenderer>().bounds.size.y;
        var posc = child.transform.position;
        posc.y -= sy;
        transform.position = posc - offset;
    }
}
