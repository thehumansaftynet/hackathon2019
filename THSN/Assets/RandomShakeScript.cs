using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShakeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScheduleShake());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ScheduleShake()
    {
        float time;
        while (true)
        {
            time = Random.value * 10f + 5f;
            //time = 2f;
            yield return new WaitForSeconds(time);
            gameObject.AddComponent<ShakeObject>().StartShakeObject(1f, 6, 0.10f);

        }

    }
}
