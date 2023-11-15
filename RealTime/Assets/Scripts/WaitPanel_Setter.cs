using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitPanel_Setter : MonoBehaviour
{
    public GameObject waitPanel;
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
    }
}
