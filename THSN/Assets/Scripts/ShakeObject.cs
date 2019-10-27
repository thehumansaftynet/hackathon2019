using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour {
    

    public void StartShakeObject(float duration, float shakes, float intensity)
    {
        StartCoroutine(ShakeObjectCoroutine(duration, shakes, intensity));
    }

    private IEnumerator ShakeObjectCoroutine(float duration, float shakes, float intensity)
    {
        float x = transform.position.x;
        float start = Time.time;


        while (Time.time - start < duration)
        {
            var d = (Time.time - start) / duration;
            var s = d * shakes * Mathf.PI * 2;

            var pos = transform.position;
            var diminishedIntensity = intensity * (1 - d);
            pos.x = x + diminishedIntensity * Mathf.Sin(s);
            transform.position = pos;

            yield return null;
        }

        var pos2 = transform.position;
        pos2.x = x;
        transform.position = pos2;

        Destroy(this);
    }
}
