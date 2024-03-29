using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScenarioManager : MonoBehaviour
{
    public DataController dataController;

    public int scenario_Main_Num = 0;  //시나리오 넘버에 사용, RealTIme트리거의 시간과 연동 //분기와 연동 X 시간당 큰덩어리
    public int Chat_Num = 0; //저장할 필요 없이 시나리오 시작과 동시에 해당 번호로 초기회 -> 접속시 어지간하면 Wait으로 가니까.
    public int first_turning_point = 0;
    public int second_turning_point = 0;
    public int third_turning_point = 0;
    public int four_turning_point = 0;
    public int five_turning_point = 0;
    public int six_turning_point = 0;
    public int seven_turning_point = 0;
    public int scenario_count = 5; //총 시나리오 갯수
    public bool[] watch_scenario;

    public int notWatch = 0;

    public bool profile_btn = false;
    public bool memo_btn = false;
    public Text TitleUi;

    // Loading 관련
    public GameObject LoadingUI;
    public CanvasGroup canvasGroup;
    public string loadSceneName;


    public int get_turning_point(string voteName)
    {
        if (voteName == "Vote1")
        {
            return first_turning_point;
        }
        else if (voteName == "Vote2")
        {
            return second_turning_point;
        }
        else if (voteName == "Vote3")
        {
            return third_turning_point;
        }
        else if (voteName == "Vote4")
        {
            return four_turning_point;
        }
        else if (voteName == "Vote5")
        {
            return five_turning_point;
        }
        else if (voteName == "Vote6")
        {
            return six_turning_point;
        }
        else if (voteName == "Vote7")
        {
            return seven_turning_point;
        }
        else
        {
            Debug.Log("get_turning_point Error");
            return 0;
        }
    }

    public void sceneChange(string scenename)
    {
        Debug.Log("씬체인지" + scenename);
        if(scenename == "IdleScene")
        {
            TitleUi.text = "방송 대기 중";
        }
        LoadingUI.SetActive(true);
        StartCoroutine(LoadSceneProcess(scenename));
        //SceneManager.LoadScene(scenename); 비동기 로딩으로 안가면 이쪽을 활성화
    }
    private IEnumerator LoadSceneProcess(string scenename)
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "InitScene")
            {
                //Init씬 이면 씬이동 계속 미룸
            }
            else
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                loadSceneName = scenename;
                break;
            }
            yield return new WaitForSeconds(1);
        }
        // Fade로 로딩 UI 등장
        yield return StartCoroutine("Fade", true);

        Debug.Log("씬 비동기 로딩 시작");
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        Debug.Log("비동기 로딩 while 진입 전");
        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.9f)
            {
                // 씬을 불러오기 전 대기하는 시간 설정
                yield return new WaitForSeconds(5f);

                // 씬 로드 가능하게 bool 값 변경
                op.allowSceneActivation = true;
                Debug.Log("씬 비동기 로딩 종료");
                yield break;
            }
        }
    }

    // 씬 로드 준비가 전부 완료된 끝 지점을 알려주는 콜백 함수
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // UI를 Fade로 자연스럽게 나타나게 하는 코루틴, bool값이 true면 나타나게, false면 사라지게
    private IEnumerator Fade(bool isFadeIn)
    {
        Debug.Log("Fade 시작");
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 1f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }
        Debug.Log("Fade 종료");
        if (!isFadeIn)
        {
            LoadingUI.SetActive(false);
        }
    }
    public void set_3Choice_Pos()
    {
        RectTransform rect = GameObject.Find("Bottom Panel").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, 0);

    }

    public void set_profile_btn_active_data()
    {
        this.profile_btn = true;
        dataController.SaveGameData();
    }
    public void set_memo_btn_active_data()
    {
        this.memo_btn = true;
        dataController.SaveGameData();
    }



    private void OnEnable()
    {
        Lua.RegisterFunction("get_turning_point", this, SymbolExtensions.GetMethodInfo(() => get_turning_point((string)"")));
        Lua.RegisterFunction("sceneChange", this, SymbolExtensions.GetMethodInfo(() => sceneChange((string)"")));
        Lua.RegisterFunction("set_3Choice_Pos", this, SymbolExtensions.GetMethodInfo(() => set_3Choice_Pos()));
        Lua.RegisterFunction("set_profile_btn_active_data", this, SymbolExtensions.GetMethodInfo(() => set_profile_btn_active_data()));
        Lua.RegisterFunction("set_memo_btn_active_data", this, SymbolExtensions.GetMethodInfo(() => set_memo_btn_active_data()));
    }
    private void OnDisable()
    {
       Lua.UnregisterFunction("get_turning_point");
       Lua.UnregisterFunction("sceneChange");
        Lua.UnregisterFunction("set_3Choice_Pos");
        Lua.UnregisterFunction("set_profile_btn_active_data");
        Lua.UnregisterFunction("set_memo_btn_active_data");
    }

}
