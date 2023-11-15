using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("IdleScene");

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startDelay());
    }

   
}
