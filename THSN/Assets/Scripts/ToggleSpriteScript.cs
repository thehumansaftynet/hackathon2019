using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSpriteScript : MonoBehaviour {

    public Sprite alt_sprite;
    private Sprite normal_sprite;
    private SpriteRenderer sp;
    private bool on = false;

	// Use this for initialization
	void Start () {
        sp = GetComponent<SpriteRenderer>();
        normal_sprite = sp.sprite;
	}
	
	public void ToggleSpriteFor(float seconds)
    {
        if (!on)
            StartCoroutine(ToggleSpriteCoroutine(seconds));
    }

    private IEnumerator ToggleSpriteCoroutine(float seconds)
    {
        on = true;
        sp.sprite = alt_sprite;
        yield return new WaitForSeconds(seconds);
        sp.sprite = normal_sprite;
        on = false;
    }
}
