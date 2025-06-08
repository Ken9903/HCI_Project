using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiExitBtn : MonoBehaviour
{
    public Button btn;
    public GameObject ui;
    public void onclick()
    {
        GameObject profile_btn = GameObject.Find("Profile_btn");
        if(profile_btn.activeSelf == true)
        {
            profile_btn.GetComponent<Profile_bth>().profileOn = false;
        }
        ui.SetActive(false);
    }
    
}
