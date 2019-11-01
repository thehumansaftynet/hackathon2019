using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonaCratorController : MonoBehaviour
{

    public Toggle[] Bildung;
    public Toggle[] Geschlecht;
    public Toggle[] Alter;
    public Toggle[] Einkommen;
    public Toggle[] Haushalt;
    public Toggle[] Familienstand;

    public Sprite[] PersonaBilder;
    public Sprite Questionmark;
    public Sprite Upload;
    public Image[] Avatar;

    public Text[] Kundennamen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NameChanged(string name)
    {
        foreach(var n in Kundennamen)
        {
            n.text = name;
        }
    }

    public void CheckboxChanged()
    {
        string imagename = $"{getActiveID(Bildung)}_{getActiveID(Geschlecht)}_{getActiveID(Alter)}";

        Debug.Log($"Change {imagename}");
        bool found = false;
        foreach (var s in PersonaBilder)
        {
            if (StringComparison(s.name, imagename))
            {
                found = true;
                foreach(var a in Avatar)
                {
                    a.GetComponent<Image>().sprite = s;
                }
            }
        }

        if (!found)
        {
            Debug.Log($"Not found {imagename}");
            foreach (var a in Avatar)
            {
                a.GetComponent<Image>().sprite = Questionmark;
            }
        }
    }

    public static bool StringComparison(string s1, string s2)
    {
        if (s1.Length != s2.Length) return false;
        for (int i = 0; i < s1.Length; i++)
        {
            if (s1[i] != s2[i])
            {
                return false;
            }
        }
        return true;
    }

    private int getActiveID(Toggle[] toggles)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                return i + 1;
            }
        }

        return 0;
    }

    public void FinishPersone()
    {

        PlayerPrefs.SetInt("personas_done", 1);
        string imagename = $"{getActiveID(Bildung)}_{getActiveID(Geschlecht)}_{getActiveID(Alter)}";
        PlayerPrefs.SetString("personas_image", imagename);
        if (PlayerPrefs.HasKey("punkte"))
        {
            PlayerPrefs.SetInt("punkte", PlayerPrefs.GetInt("punkte") + 100);
        }

        AppController.Instance.StartMapScene();
    }

}
