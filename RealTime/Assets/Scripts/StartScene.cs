using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject canvas_Logo;
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(Fade(true));
        yield return new WaitForSeconds(4);
        StartCoroutine(Fade(false));
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("IdleScene");


    }
    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            //canvas_Logo.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startDelay());
    }

   
}
