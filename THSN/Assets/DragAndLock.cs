using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndLock : MonoBehaviour
{

    public GameObject[] LockPositions;
    public float XMin, XMax;

    private bool ConfineCamera = true;
    private Vector3 lastPos;
    private const float force = 8f;
    private List<Vector3> lastValues;

    private Vector3 _dragAnchor;

    void Start()
    {
        lastValues = new List<Vector3>();
    }

    void Update()
    {
        if (ConfineCamera)
        {
            var pos = transform.position;

            if(pos.x < XMin)
            {
                pos.x = XMin;
                transform.position = pos;
            }
            else if (pos.x > XMax)
            {
                pos.x = XMax;
                transform.position = pos;
            }
        }

    }

    void OnMouseDown()
    {
        StopAllCoroutines();
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dragAnchor = pos - transform.position;
        lastValues.Clear();
    }

    void OnMouseDrag()
    {

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = pos - _dragAnchor;
        pos.y = 0;
        transform.position = pos;

        lastPos = transform.position;
        lastValues.Add(transform.position);
    }

    IEnumerator LerpToAnchor(GameObject a)
    {
        while(Vector3.Distance(transform.position, a.transform.position) > Vector3.kEpsilon)
        {
            transform.position = Vector3.Lerp(transform.position, a.transform.position, 0.25f);
            yield return null;
        }
    }

    void OnMouseUp()
    {
        float mindist = float.MaxValue;
        GameObject closestAnchor = null;
        foreach(var go in LockPositions)
        {
            var dist = Vector3.Distance(transform.position, go.transform.position);
            if(dist < mindist)
            {
                mindist = dist;
                closestAnchor = go;
            }
        }

        if(closestAnchor != null)
            StartCoroutine(LerpToAnchor(closestAnchor));


        //if (lastValues.Count >= 5)
        //{
        //    lastPos = lastValues[lastValues.Count - 5];
        //    var delta = transform.position - lastPos;
        //    Debug.Log(delta.x + " " + delta.y);
        //    delta *= force;


        //    lastPos = Vector3.zero;
        //}
    }

}
