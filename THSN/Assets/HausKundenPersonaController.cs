using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HausKundenPersonaController : MonoBehaviour
{

    public GameObject InterviewTextIntro;
    public GameObject PersonaTextIntro;
    public GameObject Creator;

    public GameObject BackgroundManu;

    public int[] maxValues;

    public DragWithMomentum TextScollingScript;


    private int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        //PersonaTextIntro.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStep()
    {
        state++;

        Transform canvas;

        switch (state)
        {
            case 1:
                PersonaTextIntro.SetActive(false);
                InterviewTextIntro.SetActive(true);
                canvas = InterviewTextIntro.transform.Find("Canvas");
                TextScollingScript.ScrollPane = canvas.gameObject;
                break;
            case 2:
                InterviewTextIntro.SetActive(false);
                BackgroundManu.SetActive(false);
                canvas = InterviewTextIntro.transform.Find("Canvas");
                TextScollingScript.ScrollPane = canvas.gameObject;
                Creator.SetActive(true);
                break;
            case 3:
                Creator.SetActive(false);
                break;
        }

    }
}
