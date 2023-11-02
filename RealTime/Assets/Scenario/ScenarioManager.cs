using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class ScenarioManager : MonoBehaviour
{

    public int scenario_Main_Num = 0;  // 채팅 메인 시나리오 넘버에 사용, RealTIme트리거의 시간과 연동 //분기와 연동 X 시간당 큰덩어리
    public int first_turning_point = 0;
    public int second_turning_point = 0;
    public int third_turning_point = 0;
    public int scenario_count = 20; //총 시나리오 갯수
    public bool[] watch_scenario;

    public int notWatch = 0;


    public int get_turning_point(string voteName)
    {
        if(voteName == "Vote1")
        {
            return first_turning_point;
        }
        else if(voteName == "Vote2")
        {
            return second_turning_point;
        }
        else if(voteName == "Vote3")
        {
            return third_turning_point;
        }
        else
        {
            //쭉쭉 나가기
            Debug.Log("get_turning_point Error");
            return 0;
        }
    }

    


    private void OnEnable()
    {
        Lua.RegisterFunction("get_turning_point", this, SymbolExtensions.GetMethodInfo(() => get_turning_point((string)"")));
    }
    private void OnDisable()
    {
       Lua.UnregisterFunction("get_turning_point");
    }

}
