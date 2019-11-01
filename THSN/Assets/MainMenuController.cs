using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public SpriteRenderer HausPersonas;
    public GameObject[] Houses;
    public Text HouseTitleField;
    public Text punkte;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("personas_done"))
        {
            var i = PlayerPrefs.GetInt("personas_done");
            if(i > 0)
            {
                HausPersonas.color = Color.green;
            }
        }

        if (PlayerPrefs.HasKey("punkte"))
        {
            punkte.text = $"Punke: {PlayerPrefs.GetInt("punkte")}";
        }
        else
        {
            PlayerPrefs.SetInt("punkte", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float min = float.MaxValue;
        int minID = 0;

        for(int i = 0; i < Houses.Length; i++)
        {
            var v = Mathf.Abs(Houses[i].transform.position.x);
            if (v < min)
            {
                min = v;
                minID = i;
            }
        }

        HouseTitleField.text = Houses[minID].name;



    }
}
