using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerOLD : MonoBehaviour
{

    public static GameControllerOLD _INSTANCE;
    public SpriteRenderer BlackGround;
    public SpriteRenderer Credits;

    public Transform BaumLinks;
    public Transform BaumRechts;

    public Transform FinalNutSpawn;

    public AudioSource TitleTheme;

    public AudioSource EndingMusic;

    private float targetVolume = 1f;
    private float currentVolume = 1f;

    void Awake()
    {
        if (_INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

        _INSTANCE = this;
        //DontDestroyOnLoad(this);

    }

    internal void ShakeCam(float duration, float shakes, float intensity)
    {
        StartCoroutine(ShakeCamCoroutine(duration, shakes, intensity));
    }

    private IEnumerator ShakeCamCoroutine(float duration, float shakes, float intensity)
    {
        float x = Camera.main.transform.position.x;
        float start = Time.time;


        while (Time.time - start < duration)
        {
            var d = (Time.time - start) / duration;
            var s = d * shakes * Mathf.PI * 2;

            var pos = Camera.main.transform.position;
            var diminishedIntensity = intensity * (1 - d);
            pos.x = x + diminishedIntensity * Mathf.Sin(s);
            Camera.main.transform.position = pos;

            yield return null;
        }

        var pos2 = Camera.main.transform.position;
        pos2.x = x;
        Camera.main.transform.position = pos2;


    }

    void Start()
    {
        
    }

    public void StartGame()
    {
        // if (notStarted)
        // {
        //     StartCoroutine(FadeOutTitle());
        //     StartCoroutine(StartGameCoroutine());
        //     notStarted = false;
        // }

    }

    private IEnumerator FadeOutTitle()
    {
        var start = Time.time;
        var dur = 3f;
        while (Time.time - start <= dur)
        {
            var delta = (Time.time - start) / dur;
            TitleTheme.volume = (1 - delta);

            yield return null;
        }

        TitleTheme.volume = 0f;
    }

    void Update()
    {
        if (targetVolume < currentVolume)
        {
            currentVolume -= Time.deltaTime * 1f;
        }

        if (targetVolume > currentVolume)
        {
            currentVolume += Time.deltaTime * 1f;
        }

        currentVolume = Mathf.Clamp(currentVolume, 0f, 1f);

    }

    internal void ShowCredits()
    {
        StartCoroutine(ShowCreditsCoroutine());
    }

    private IEnumerator ShowCreditsCoroutine()
    {
        var t1 = Time.time;

        float d = 5f;

        while (Time.time - t1 <= d)
        {
            var c = BlackGround.color;
            c.a = (Time.time - t1) / d * 0.5f;
            BlackGround.color = c;
            yield return null;
        }

        t1 = Time.time;
        while (Time.time - t1 <= d)
        {
            var c = Credits.color;
            c.a = (Time.time - t1) / d * 1f;
            Credits.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(15f);

        t1 = Time.time;
        while (Time.time - t1 <= d)
        {
            var delta = (Time.time - t1) / d;

            var c = Credits.color;
            c.a = (1 - delta);
            Credits.color = c;

            c = BlackGround.color;
            c.a = 0.5f + delta * 0.5f;
            BlackGround.color = c;

            yield return null;
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("TumbleHogMain");
    }

    private IEnumerator SpawnLater(GameObject go, Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);
        var newObject = Instantiate(go, t);
    }

    private IEnumerator MoveOverTime(Transform o, float duration, Vector3 d)
    {
        var startTime = Time.time;
        var startPos = o.position;

        while (Time.time - startTime <= duration)
        {
            var delta = (Time.time - startTime) / duration;
            var nextPos = startPos + delta * d;
            o.position = nextPos;
            yield return null;
        }

        o.position = startPos + d;
    }

    public void FadeBackgroundMusic(bool fadeOut)
    {
        targetVolume = fadeOut ? 0f : 1f;
    }
}
