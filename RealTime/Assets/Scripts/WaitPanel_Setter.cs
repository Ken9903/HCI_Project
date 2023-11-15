using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitPanel_Setter : MonoBehaviour
{
    public GameObject waitPanel;
    public GameObject profile_btn;
    public GameObject memo_btn;
    public ScenarioManager scenarioManager;
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "IdleScene")
        {
            waitPanel.SetActive(true);
        }
        else
        {
            waitPanel.SetActive(false);
        }
        GameObject off = GameObject.Find("OFF");
        if (off != null)
        {
            off.SetActive(false);
        }
        if(scenarioManager.profile_btn == true)
        {
            profile_btn.SetActive(true);
        }
        if(scenarioManager.memo_btn == true)
        {
            memo_btn.SetActive(true);
        }
    }
}
