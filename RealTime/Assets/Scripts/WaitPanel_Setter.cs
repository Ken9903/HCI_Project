using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitPanel_Setter : MonoBehaviour
{
    public GameObject waitPanel;
    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
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
