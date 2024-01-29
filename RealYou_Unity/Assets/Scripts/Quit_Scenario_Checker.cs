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
        //scenarioManager.scenario_Main_Num++; //시작과 동시에 ++되기 때문에 더해줄 필요 없음
        scenarioManager.Chat_Num = 0;
        chattingManager.wait_next_chat_min = 150;
        chattingManager.wait_next_chat_max = 200;

        dataController.SaveGameData();
    }
}
