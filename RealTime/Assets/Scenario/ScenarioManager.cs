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
            //���� ������
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
