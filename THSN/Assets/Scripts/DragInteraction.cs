using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInteraction : MonoBehaviour
{

    private Vector3 originalPosition;
    private Rigidbody2D rb;

    public bool canRespawn = true;
    public GameObject SpawnPoof;

    public AudioClip DragSound;
    public AudioClip DropSound;

    private bool resetIsActive = false;
    private Vector3 lastPos;
    private const float force = 8f;
    private List<Vector3> lastValues;

    public bool PhysicsEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        lastValues = new List<Vector3>();
    }

    void Update()
    {
        if (!PhysicsEnabled) return;
        if (resetIsActive)
        {
            var pos = Camera.main.WorldToViewportPoint(transform.position);

            var r = new Rect(-0.2f, -0.2f, 1.4f, 2f);

            if (!r.Contains(pos))
            {
                if (canRespawn)
                {
                    Respawn();

                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

    }

    public void Respawn()
    {
        resetIsActive = false;
        Destroy(rb);
        transform.position = originalPosition;
        if (SpawnPoof != null)
        {
            var spawnPos = transform.position;
            spawnPos.z -= 0.1f;
            Instantiate(SpawnPoof, spawnPos, Quaternion.identity);

        }
    }

    void OnMouseDown()
    {
        SoundManager._INSTANCE.PlaySound(DragSound);

        if (!PhysicsEnabled) return;
        lastValues.Clear();
        if(rb != null)
            Destroy(rb);
    }

    void OnMouseDrag()
    {
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;
        transform.position = pos;

        lastPos = transform.position;
        lastValues.Add(transform.position);
        
    }

    void OnMouseUp()
    {
        SoundManager._INSTANCE.PlaySound(DropSound);
        resetIsActive = true;

        if (!PhysicsEnabled) return;

        rb = gameObject.AddComponent<Rigidbody2D>();
        if(lastValues.Count >= 5)
        {
            lastPos = lastValues[lastValues.Count - 5];
            var delta = transform.position - lastPos;
            Debug.Log(delta.x + " " + delta.y);
            delta *= force;

            if (rb != null)
            {
                rb.velocity = new Vector2(delta.x, delta.y);
            }


            lastPos = Vector3.zero;
        }

        
    }
}
