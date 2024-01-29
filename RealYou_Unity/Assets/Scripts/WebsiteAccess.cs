using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebsiteAccess : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    public GameObject mailList;
    public GameObject Login_page;
    public GameObject failText;
    
    public void onClick()
    {
        if(id.text == "silverrain0401@gmail.com")
        {
            if (pw.text == "qkrdmsqlRj^^7")
            {
                failText.SetActive(false);
                mailList.SetActive(true);
                Login_page.SetActive(false);
            }
            else
            {
                failText.SetActive(true);
            }
        }
        else
        {
            failText.SetActive(true);
        }
    }
}
