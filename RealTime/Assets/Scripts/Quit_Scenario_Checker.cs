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
        Debug.Log("경고 : 이 오브젝트는 시나리오 중에만 존재 하여야 합니다.");
        scenarioManager.scenario_Main_Num++;
        scenarioManager.Chat_Num = 0;
        chattingManager.wait_next_chat_min = 150;
        chattingManager.wait_next_chat_max = 200;

        dataController.SaveGameData();
        Debug.Log("시나리오 중 종료로 인한 시나리오 넘버 + 1 -> 건너뛰기");
        Debug.Log("종료 초기화,저장 완료");
    }
}
