using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoStart : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("InitScene");
    }

}
