using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBox : MonoBehaviour
{

    SpriteRenderer sr;
    float target = 0f;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        var color = sr.color;
        color.a = 1;
        sr.color = color;
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        var color = sr.color;

        color.a = Mathf.Lerp(color.a, target, speed * Time.deltaTime);
        sr.color = color;

    }

    public void FadeIn()
    {
        target = 0f;
    }

    public void FadeOut()
    {
        target = 1f;
    }


}
