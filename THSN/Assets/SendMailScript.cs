using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendMailScript : MonoBehaviour
{

    public string mail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendEmail()
    {
        string email = mail;
        string subject = MyEscapeURL("Beratungsanfrage Biz Allee");
        string body = MyEscapeURL("Hallo!\r\n\r\nIch habe die Angebote der App genutzt und möchte mich nun gerne eingehend Beraten lassen.\r\n\r\nMit freundlichen Grüßen");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
        //return WWW.EscapeURL(url).Replace("+", "%20");
        
    }
}
