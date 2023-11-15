using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScenarioManager : MonoBehaviour
{
    public DataController dataController;

    public int scenario_Main_Num = 0;  //�ó����� �ѹ��� ���, RealTImeƮ������ �ð��� ���� //�б�� ���� X �ð��� ū���
    public int Chat_Num = 0; //������ �ʿ� ���� �ó����� ���۰� ���ÿ� �ش� ��ȣ�� �ʱ�ȸ -> ���ӽ� �������ϸ� Wait���� ���ϱ�.
    public int first_turning_point = 0;
    public int second_turning_point = 0;
    public int third_turning_point = 0;
    public int scenario_count = 20; //�� �ó����� ����
    public bool[] watch_scenario;

    public int notWatch = 0;

    public bool profile_btn = false;
    public bool memo_btn = false;


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

    public void sceneChange(string scenename)
    {
        dataController.SaveGameData();
        SceneManager.LoadScene(scenename);
    }

    public void set_3Choice_Pos()
    {
        RectTransform rect = GameObject.Find("Bottom Panel").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, 0);

    }

    public void set_profile_btn_active_data()
    {
        this.profile_btn = true;
        dataController.SaveGameData();
    }
    public void set_memo_btn_active_data()
    {
        this.memo_btn = true;
        dataController.SaveGameData();
    }



    private void OnEnable()
    {
        Lua.RegisterFunction("get_turning_point", this, SymbolExtensions.GetMethodInfo(() => get_turning_point((string)"")));
        Lua.RegisterFunction("sceneChange", this, SymbolExtensions.GetMethodInfo(() => sceneChange((string)"")));
        Lua.RegisterFunction("set_3Choice_Pos", this, SymbolExtensions.GetMethodInfo(() => set_3Choice_Pos()));
        Lua.RegisterFunction("set_profile_btn_active_data", this, SymbolExtensions.GetMethodInfo(() => set_profile_btn_active_data()));
        Lua.RegisterFunction("set_memo_btn_active_data", this, SymbolExtensions.GetMethodInfo(() => set_memo_btn_active_data()));
    }
    private void OnDisable()
    {
       Lua.UnregisterFunction("get_turning_point");
       Lua.UnregisterFunction("sceneChange");
        Lua.UnregisterFunction("set_3Choice_Pos");
        Lua.UnregisterFunction("set_profile_btn_active_data");
        Lua.UnregisterFunction("set_memo_btn_active_data");
    }

}
