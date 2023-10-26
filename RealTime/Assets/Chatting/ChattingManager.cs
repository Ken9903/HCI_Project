using System; //Serializable �����ϱ� ���� ���.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class ChattingManager : MonoBehaviour //***�ó����� �ѹ��� �����̾ƴ� ������ ���� ����� ���� �پ缺 �ְ� ������ �ʿ�����
{
    //���� Ŭ����
    public ScenarioManager scenarioManager;
    public InputField player_chat_input;

    [Serializable]
    public class ChatDataList
    {
        public string[] chatData; // ���� ��ȭ ���� 
    }
    public ChatDataList[] chatList; //�ó����� �к� 
    public string[] nameList;
    public bool[] sex; //nameList�� �ε����� ���� 0:���� 1:����
    public GameObject[] currentChatList = new GameObject[7] { null, null, null, null, null, null, null }; //***5�� max_chat_num�̶� ����ȭ �ʿ�***
    public Transform[] chatPoint; //���� �迭�� ���� ���� ����


    //����
    public int wait_next_chat_max = 200; //���� ä���� �ö������� �ɸ��� �ð� -> ���������� ȭ���� ǥ�� ���� *0.01���� �ʿ�
    public int wait_next_chat_min = 50;
    public int max_chat_num = 7;
    private int current_chat_num = 0;
    public int max_chat_kind = 10; //���� �ó����� �ϳ��� �� �� �ִ� ��� ä�� ���� ����
    private bool player_chatting = false;


    // ���ҽ�
    public GameObject chatUi; //Ui���ø�
    public Sprite manImage;
    public Sprite womanImage;
    public GameObject donation_panel; //�����̼� ��ü Ui
    public GameObject donate_name_money;
    public GameObject donate_content;

    IEnumerator playChatting()
    {
        while (true)
        {
            if (player_chatting == false)
            {
                if (current_chat_num == max_chat_num) // ���� ä���� �ִ�ġ �� ��
                {
                    Destroy(currentChatList[max_chat_num - 1]); //���� �����ִ� ä�� ���� 
                    currentChatList[max_chat_num - 1] = null;
                    current_chat_num--;
                }


                int chatData_Random_Index = UnityEngine.Random.Range(0, 10); //ǥ���� ������ ����
                string target_Data = chatList[scenarioManager.scenario_Main_Num].chatData[chatData_Random_Index]; //ǥ���� ä��
                int Name_Sex_Random_Index = UnityEngine.Random.Range(0, 40); // �̸��� ���� ���� ����
                GameObject currentChatUi = Instantiate(chatUi, chatPoint[0]);

                currentChatUi.transform.GetChild(0).GetComponent<Text>().text = target_Data; //�ؽ�Ʈ ����
                currentChatUi.transform.GetChild(1).GetComponent<Text>().text = nameList[Name_Sex_Random_Index]; //�̸� ����
                if (!sex[Name_Sex_Random_Index]) //������ ���� �̹��� ����
                {
                    currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = manImage;
                }
                else
                {
                    currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = womanImage;
                }



                for (int i = max_chat_num - 1; i >= 0; i--) //�� ���������� �Ʒ��� ����
                {
                    if (currentChatList[i] != null)
                    {
                        currentChatList[i].transform.SetParent(GameObject.Find(String.Concat("ChatPoint", (i + 2).ToString())).transform);
                        currentChatList[i].GetComponent<RectTransform>().localPosition = Vector3.zero; //��ġ ������ �ʿ�
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
                yield return new WaitForSeconds(.1f); //***���� üũ �ʿ�
            }

        }

    }


    public void playerChat_onClick()
    {
        player_chatting = true; // ���ÿ� UI�����ϸ� ���� �߻������ϴ� ���� ���� ����
        string chat = player_chat_input.text;
        Debug.Log(chat);

        if (current_chat_num == max_chat_num) // ���� ä���� �ִ�ġ �� ��
        {
            Destroy(currentChatList[max_chat_num - 1]); //���� �����ִ� ä�� ���� 
            currentChatList[max_chat_num - 1] = null;
            current_chat_num--;
        }
        GameObject currentChatUi = Instantiate(chatUi, chatPoint[0]);

        currentChatUi.transform.GetChild(0).GetComponent<Text>().text = chat; //�ؽ�Ʈ ����
        currentChatUi.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
        currentChatUi.transform.GetChild(1).GetComponent<Text>().text = "Guest356"; //�̸� -> ���� �޾ƿ���
        currentChatUi.transform.GetChild(2).GetComponent<Image>().sprite = manImage; // ���� �޾ƿ���

        for (int i = max_chat_num - 1; i >= 0; i--) //�� ���������� �Ʒ��� ����
        {
            if (currentChatList[i] != null)
            {
                currentChatList[i].transform.SetParent(GameObject.Find(String.Concat("ChatPoint", (i + 2).ToString())).transform);
                currentChatList[i].GetComponent<RectTransform>().localPosition = Vector3.zero; //��ġ ������ �ʿ�
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
        donate_name_money.GetComponent<Text>().text = name + "����" + money + "���� �Ŀ��ϼ̽��ϴ�.";
        donate_content.GetComponent<Text>().text = content;
        yield return new WaitForSeconds(delay);
        donation_panel.SetActive(false);
    }

    public void playDonate(string name, System.Single money, string content, System.Single delay)
    {
        StartCoroutine(donate(name, money, content, delay));
    }





    void Start()
    {
        StartCoroutine(playChatting());
        //StartCoroutine(donate("guest", 1000, "���� �Ŀ��� �Ѵ�", 5));
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("playDonate", this, SymbolExtensions.GetMethodInfo(() => playDonate((string)"", (int)0, (string)"", (float)0)));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("playDonate");
    }
}
