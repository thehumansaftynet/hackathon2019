using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{

    public static AppController Instance;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator LoadScene(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        while (!asyncLoad.isDone)
        {
            asyncLoad.allowSceneActivation = true;
            yield return null;
        }

    }

    public void StartPersoneHouseScene()
    {
        StartCoroutine(LoadScene("HausKundenPersona"));
    }

    public void StartMarketScene()
    {
        StartCoroutine(LoadScene("MarktplatzScene"));
    }

    public void StartMapScene()
    {
        StartCoroutine(LoadScene("MainScene"));

    }

    public void StartScene(string name)
    {
        StartCoroutine(LoadScene(name));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
