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


    


    private void OnEnable()
    {
        //Lua.RegisterFunction("playDonate", this, SymbolExtensions.GetMethodInfo(() => playDonate((string)"", (int)0, (string)"", (float)0)));
    }
    private void OnDisable()
    {
        //Lua.UnregisterFunction("playDonate");
    }

}
