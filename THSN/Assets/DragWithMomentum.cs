using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWithMomentum : MonoBehaviour
{
    public float Min, Max;
    public bool YAxis = true;
    public GameObject ScrollPane;

    private bool ConfineCamera = true;
    private Vector3 lastPos;
    private const float force = 8f;
    private List<Vector3> lastValues;

    private Vector3 _dragAnchor;
    private Rigidbody2D rb;

    void Start()
    {
        lastValues = new List<Vector3>();
        rb = null;
    }

    void Update()
    {
        if (ConfineCamera)
        {
            var pos = ScrollPane.transform.position;


            if (YAxis)
            {
                if (pos.y < Min)
                {
                    pos.y = Min;
                    ScrollPane.transform.position = pos;
                }
                else if (pos.y > Max)
                {
                    pos.y = Max;
                    ScrollPane.transform.position = pos;
                }
            }
            else
            {
                if (pos.x < Min)
                {
                    pos.x = Min;
                    ScrollPane.transform.position = pos;
                }
                else if (pos.x > Max)
                {
                    pos.x = Max;
                    ScrollPane.transform.position = pos;
                }
            }
        }

    }

    void OnMouseDown()
    {
        StopAllCoroutines();
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dragAnchor = pos - ScrollPane.transform.position;
        lastValues.Clear();

        if (rb != null)
            Destroy(rb);
    }

    void OnMouseDrag()
    {

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = pos - _dragAnchor;
        if (YAxis)
        {
            pos.x = 0;
        }
        else
        {
            pos.y = 0;
        }
        ScrollPane.transform.position = pos;

        lastPos = ScrollPane.transform.position;
        lastValues.Add(ScrollPane.transform.position);
    }

    IEnumerator LerpToAnchor(GameObject a)
    {
        while (Vector3.Distance(ScrollPane.transform.position, a.transform.position) > Vector3.kEpsilon)
        {
            ScrollPane.transform.position = Vector3.Lerp(ScrollPane.transform.position, a.transform.position, 0.25f);
            yield return null;
        }
    }

    void OnMouseUp()
    {
        rb = ScrollPane.gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 5f;
        if (lastValues.Count >= 5)
        {
            lastPos = lastValues[lastValues.Count - 5];
            var delta = ScrollPane.transform.position - lastPos;
            delta *= force;

            if (rb != null)
            {
                rb.velocity = new Vector2(delta.x, delta.y);
            }

            lastPos = Vector3.zero;
        }
    }
}
