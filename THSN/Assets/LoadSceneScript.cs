using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneScript : MonoBehaviour
{
    // Start is called before the first frame update

        
    public new string name;
    void Start()

    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {
        switch (name)
        {
            case "market":
                AppController.Instance.StartMarketScene();
                break;
            case "persona":
                AppController.Instance.StartPersoneHouseScene();
                break;
            case "marktplatz":
                AppController.Instance.StartScene("MarktplatzScene");
                break;
            case "connect":
                AppController.Instance.StartScene("ConnectScreen");
                break;
        }
    }

    public void LoadScene(string name)
    {
        AppController.Instance.StartScene(name);
    }
}
