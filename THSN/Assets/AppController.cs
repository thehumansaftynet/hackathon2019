using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{

    //public static AppController Instance;

    private void Awake()
    {
        //if(Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //Instance = this;
        //DontDestroyOnLoad(gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartPersoneHouseScene()
    {
        SceneManager.LoadScene("HausKundenPersona");
    }

    public void StartMarketScene()
    {
        SceneManager.LoadScene("MarktplatzScene");
    }

    public void StartMapScene()
    {
        SceneManager.LoadScene("MainScene");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
