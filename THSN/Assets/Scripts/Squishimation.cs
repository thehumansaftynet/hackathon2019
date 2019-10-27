using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishimation : MonoBehaviour {

    public float animationSpeed = 1f;
    public float strength = 1f;

    public bool fixGround = false;
    public bool invert = false;

    public Vector3 originalScale;
    

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;

        // Used to make sure the object stays grounded
        if (fixGround)
        {
            var go = new GameObject("Grounder for " + gameObject.name);
            var dy = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            var newPos = transform.position;
            if(!invert)
                newPos.y -= dy;
            else
                newPos.y += dy;
            go.transform.position = newPos;
            go.transform.parent = transform.parent;
            transform.parent = go.transform;
            var ani = go.AddComponent<Squishimation>();
            ani.animationSpeed = animationSpeed;
            ani.strength = strength;
            ani.fixGround = false;
            Destroy(this);
        }
            

        StartCoroutine(SquashAnimation());
	}

    private IEnumerator SquashAnimation()
    {
        float offset = 2 * Mathf.PI * Random.value;

        while (true)
        {
            var s = Mathf.Sin(offset + Time.time * animationSpeed);

            var hori = originalScale.x + strength * s;
            var vert = originalScale.y + strength * (1 - s);

            transform.localScale = new Vector3(hori, vert, originalScale.z);
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
