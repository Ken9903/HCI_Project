using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebsiteExit : MonoBehaviour
{
    public GameObject ui;

    public ScenarioManager scenarioManager;
    public void onclick()
    {
        ui.SetActive(false);
        scenarioManager.sceneChange("IdleScene");
    }
}
