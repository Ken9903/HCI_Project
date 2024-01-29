using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiExitBtn_Uni : MonoBehaviour
{
    public GameObject ui;
    public void onclick()
    {
        ui.SetActive(false);
    }
}
