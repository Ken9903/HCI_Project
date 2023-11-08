using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit_Scenario_Checker : MonoBehaviour
{
    public ChattingManager chattingManager;
    public ScenarioManager scenarioManager;
    public DataController dataController;
    void OnApplicationQuit()
    {
        Debug.Log("��� : �� ������Ʈ�� �ó����� �߿��� ���� �Ͽ��� �մϴ�.");
        scenarioManager.scenario_Main_Num++;
        scenarioManager.Chat_Num = 0;
        chattingManager.wait_next_chat_min = 150;
        chattingManager.wait_next_chat_max = 200;

        dataController.SaveGameData();
        Debug.Log("�ó����� �� ����� ���� �ó����� �ѹ� + 1 -> �ǳʶٱ�");
        Debug.Log("���� �ʱ�ȭ,���� �Ϸ�");
    }
}
