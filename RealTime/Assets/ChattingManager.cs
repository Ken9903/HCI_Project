using System; //Serializable �����ϱ� ���� ���.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingManager : MonoBehaviour
{
    //���� Ŭ����
    public ScenarioManager scenarioManager;

    [Serializable] 
    public class ChatDataList
    {
        public string[] chatData; // ���� ��ȭ ���� 
    }
    public ChatDataList[] chatList; //�ó����� �к� 
    public string[] nameList;
    public bool[] sex; //nameList�� �ε����� ���� 0:���� 1:����
    public GameObject[] currentChatList = new GameObject[5] { null, null, null, null, null }; //***5�� max_chat_num�̶� ����ȭ �ʿ�***
    public Transform[] chatPoint; //���� �迭�� ���� ���� ����


    //����
    public int wait_next_chat_max = 200; //���� ä���� �ö������� �ɸ��� �ð� -> ���������� ȭ���� ǥ�� ���� *0.01���� �ʿ�
    public int wait_next_chat_min = 50;
    public int max_chat_num = 5;
    private int current_chat_num = 0;
    public int max_chat_kind = 10; //���� �ó����� �ϳ��� �� �� �ִ� ��� ä�� ���� ����


    // ���ҽ�
    public GameObject chatUi; //Ui���ø�
    public Sprite manImage;
    public Sprite womanImage;
    
    IEnumerator playChatting()
    {
        while(true)
        {
            if (current_chat_num == max_chat_num) // ���� ä���� �ִ�ġ �� ��
            {
                Destroy(currentChatList[4]); //���� �����ִ� ä�� ����
                currentChatList[4] = null;
                current_chat_num--;
            }


            int chatData_Random_Index = UnityEngine.Random.Range(0, 10); //ǥ���� ������ ����
            string target_Data = chatList[scenarioManager.scenario_Main_Num].chatData[chatData_Random_Index]; //ǥ���� ä��
            int Name_Sex_Random_Index = UnityEngine.Random.Range(0, 40); // �̸��� ���� ���� ����
            GameObject currentChatUi = Instantiate(chatUi, chatPoint[0]);

            currentChatUi.transform.GetChild(0).GetComponent<Text>().text = target_Data; //�ؽ�Ʈ ����
            currentChatUi.transform.GetChild(1).GetComponent<Text>().text = nameList[Name_Sex_Random_Index]; //�̸� ����
            if(!sex[Name_Sex_Random_Index]) //������ ���� �̹��� ����
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
      
    }









    void Start()
    {
        StartCoroutine(playChatting());
    }
}
