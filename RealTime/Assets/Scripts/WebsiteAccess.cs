using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebsiteAccess : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    public GameObject mailList;
    
    public void onClick()
    {
        if(id.text == "silverrain0401@gmail.com")
        {
            if (pw.text == "qkrdmsqlRj^^7")
            {
                mailList.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
