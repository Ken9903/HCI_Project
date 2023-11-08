using System; //Serializable 적용하기 위해 사용.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class ChattingManager : MonoBehaviour //***시나리오 넘버에 연동이아닌 독자적 변수 제어로 좀더 다양성 있게 가야할 필요있음
{
    //참조 클래스
    public ScenarioManager scenarioManager;
    public InputField player_chat_input;

    [Serializable]
    public class ChatDataList
    {
        public string[] chatData; // 실제 대화 내용 
    }
    public ChatDataList[] chatList; //시나리오 분별 
    public string[] nameList;
    public bool[] sex; //nameList랑 인덱스로 연동 0:남자 1:여자
    public GameObject[] currentChatList = new GameObject[7] { null, null, null, null, null, null, null }; //***5는 max_chat_num이랑 동기화 필요***
    public Transform[] chatPoint; //고정 배열로 개선 여지 있음


    //변수
    public int wait_next_chat_max = 200; //다음 채팅이 올라오기까지 걸리는 시간 -> 랜덤값으로 화제성 표현 해줌 *0.01연산 필요
    public int wait_next_chat_min = 50;
    public int max_chat_num = 7;
    private int current_chat_num = 0;
    public int max_chat_kind = 10; //메인 시나리오 하나에 들어갈 수 있는 모든 채팅 종류 갯수
    private bool player_chatting = false;


    // 리소스
    public GameObject chatUi; //Ui템플릿
    public Sprite manImage;
    public Sprite womanImage;
    public GameObject donation_panel; //도네이션 전체 Ui
    public GameObject donate_name_money;
    public GameObject donate_content;

    IEnumerator playChatting()
    {
        while (true)
        {
            if (player_chatting == false)
            {
                if (current_chat_num == max_chat_num) // 현재 채팅이 최대치 일 때
                {
                    Destroy(currentChatList[max_chat_num - 1]); //가장 위에있는 채팅 삭제 
                    currentChatList[max_chat_num - 1] = null;
                    current_chat_num--;
                }


                int chatData_Random_Index = UnityEngine.Random.Range(0, 10); //표시할 랜덤값 추출
                string target_Data = chatList[scenarioManager.Chat_Num].chatData[chatData_Random_Index]; //표시할 채팅
                int Name_Sex_Random_Index = UnityEngine.Random.Range(0, 40); // 이름과 성별 랜덤 추출
                GameObject currentChatUi = Instantiate(chatUi, chatPoint[0]);

                currentChatUi.transform.GetChild(0).GetComponent<Text>().text = target_Data; //텍스트 변경
                currentChatUi.transform.GetChild(1).GetComponent<Text>().text = nameList[Name_Sex_Random_Index]; //이름 변경
                if (!sex[Name_Sex_Random_Index]) //성별에 따른 이미지 변경
                {
                    currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = manImage;
                }
                else
                {
                    currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = womanImage;
                }



                for (int i = max_chat_num - 1; i >= 0; i--) //맨 위에서부터 아래로 조정
                {
                    if (currentChatList[i] != null)
                    {
                        currentChatList[i].transform.SetParent(GameObject.Find(String.Concat("ChatPoint", (i + 2).ToString())).transform);
                        currentChatList[i].GetComponent<RectTransform>().localPosition = Vector3.zero; //위치 재조정 필요
                        currentChatList[i + 1] = currentChatList[i];
                    }
                }

                currentChatList[0] = currentChatUi;
                current_chat_num++;

                float wait = UnityEngine.Random.Range(wait_next_chat_min, wait_next_chat_max) * 0.01f;
                yield return new WaitForSeconds(wait);
            }
            else
            {
                yield return new WaitForSeconds(.1f); //***버그 체크 필요
            }

        }

    }




    public void playerChat_onClick()
    {
        player_chatting = true; // 동시에 UI접근하면 버그 발생가능하니 기존 로직 정지
        string chat = player_chat_input.text;
        Debug.Log(chat);

        if (current_chat_num == max_chat_num) // 현재 채팅이 최대치 일 때
        {
            Destroy(currentChatList[max_chat_num - 1]); //가장 위에있는 채팅 삭제 
            currentChatList[max_chat_num - 1] = null;
            current_chat_num--;
        }
        GameObject currentChatUi = Instantiate(chatUi, chatPoint[0]);

        currentChatUi.transform.GetChild(0).GetComponent<Text>().text = chat; //텍스트 변경
        currentChatUi.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
        currentChatUi.transform.GetChild(1).GetComponent<Text>().text = "Guest356"; //이름 -> 따로 받아오기
        currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = manImage; // 성별 받아오기

        for (int i = max_chat_num - 1; i >= 0; i--) //맨 위에서부터 아래로 조정
        {
            if (currentChatList[i] != null)
            {
                currentChatList[i].transform.SetParent(GameObject.Find(String.Concat("ChatPoint", (i + 2).ToString())).transform);
                currentChatList[i].GetComponent<RectTransform>().localPosition = Vector3.zero; //위치 재조정 필요
                currentChatList[i + 1] = currentChatList[i];
            }
        }
        currentChatList[0] = currentChatUi;
        current_chat_num++;

        player_chat_input.text = "";
        player_chatting = false;
    }

    public IEnumerator donate(string name, System.Single money, string content, System.Single delay)
    {
        donation_panel.SetActive(true);
        donate_name_money.GetComponent<Text>().text = name + "님이" + money + "원을 후원하셨습니다.";
        donate_content.GetComponent<Text>().text = content;
        yield return new WaitForSeconds(delay);
        donation_panel.SetActive(false);
    }

    public void playDonate(string name, System.Single money, string content, System.Single delay)
    {
        StartCoroutine(donate(name, money, content, delay));
    }

    public void nextChat()
    {
        scenarioManager.Chat_Num++;
    }
    public void initChat()
    {
        scenarioManager.Chat_Num = 0;
    }
    public void scenarioChat_Setting(System.Single num)
    {
        scenarioManager.Chat_Num = (int)num;
    }

    public void chnage_chat_speed(System.Single min, System.Single max)
    {
        wait_next_chat_min = (int)min;
        wait_next_chat_max = (int)max;
    }
    public void init_chat_speed()
    {
        wait_next_chat_min = 150;
        wait_next_chat_max = 200;
    }

    public void set_chat_time_event()//시나리오 0번 시간알려주는 이벤트용 함수
    {
        int temp = 0; //*** 해당 번호 추가 요망
        chatList[temp].chatData[0] = DateTime.Now.ToString("HH") +"시간 이야";
        chatList[temp].chatData[1] = DateTime.Now.ToString("HH") + "시임";
        chatList[temp].chatData[2] = "지금" + DateTime.Now.ToString("HH") + "시";
        chatList[temp].chatData[3] = "ㅇㅇ" + DateTime.Now.ToString("HH") + "시";

    }




    void Start()
    {
        StartCoroutine(playChatting());
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("playDonate", this, SymbolExtensions.GetMethodInfo(() => playDonate((string)"", (int)0, (string)"", (float)0)));
        Lua.RegisterFunction("scenarioChat_Setting", this, SymbolExtensions.GetMethodInfo(() => scenarioChat_Setting((int)0)));
        Lua.RegisterFunction("nextChat", this, SymbolExtensions.GetMethodInfo(() => nextChat()));
        Lua.RegisterFunction("initChat", this, SymbolExtensions.GetMethodInfo(() => initChat()));
        Lua.RegisterFunction("chnage_chat_speed", this, SymbolExtensions.GetMethodInfo(() => chnage_chat_speed((int)0, (int)0)));
        Lua.RegisterFunction("init_chat_speed", this, SymbolExtensions.GetMethodInfo(() => init_chat_speed()));
        Lua.RegisterFunction("set_chat_time_event", this, SymbolExtensions.GetMethodInfo(() => set_chat_time_event()));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("playDonate");
        Lua.UnregisterFunction("scenarioChat_Setting");
        Lua.UnregisterFunction("nextChat");
        Lua.UnregisterFunction("initChat");
        Lua.UnregisterFunction("chnage_chat_speed");
        Lua.UnregisterFunction("init_chat_speed");
        Lua.UnregisterFunction("set_chat_time_event");
    }
}
