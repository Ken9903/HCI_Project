using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class ScenarioManager : MonoBehaviour
{

    public int scenario_Main_Num = 0;  // ä�� ���� �ó����� �ѹ��� ���, RealTImeƮ������ �ð��� ���� //�б�� ���� X �ð��� ū���
    public int first_turning_point = 0;
    public int second_turning_point = 0;
    public int third_turning_point = 0;
    public int scenario_count = 20; //�� �ó����� ����
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
