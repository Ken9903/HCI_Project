using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DataController : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    public ChattingManager chattingManager;
    public RealTime_Event_Trigger realTime_Event_Trigger;


    // ---�̱������� ����--- 
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    // --- ���� ������ �����̸� ���� ---
    public string GameDataFileName = "real.json";

    // "���ϴ� �̸�(����).json"
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            // ������ ���۵Ǹ� �ڵ����� ����ǵ���
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }
    private void Awake()
    {
        LoadGameData();
        SaveGameData();
    }
    public void LoadGameData()
    {

        string filePath = Path.Combine(Application.persistentDataPath, GameDataFileName);
        Debug.Log(filePath);
        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            print("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);

        }
        // ����� ������ ���ٸ� //�ʱⰪ *** �׽�Ʈ �ʿ�
        else
        {
            print("���ο� ���� ����");
            _gameData = new GameData();
            //ScenarioManager
            gameData.scenario_Main_Num = 0;
            bool[] temp_watch = { false, false, false, false, false }; //***���߿� �ó����� ũ�⿡ ���缭 �ٲ��ֱ�
            gameData.watch_scenario = temp_watch;
            gameData.notWatch = 0;
            gameData.first_turning_point = 0;
            gameData.second_turning_point = 0;
            gameData.third_turning_point = 0;
            gameData.four_turning_point = 0;
            gameData.five_turning_point = 0;
            gameData.six_turning_point = 0;
            gameData.seven_turning_point = 0;

    //RealTIme_Event_Trigger
    gameData.init = false;

            gameData.year = System.DateTime.Now.Year.ToString();
            gameData.month = System.DateTime.Now.Month.ToString();
            gameData.day = System.DateTime.Now.Day.ToString();
            gameData.hour = System.DateTime.Now.Hour.ToString();
            gameData.minute = System.DateTime.Now.Minute.ToString();
            gameData.second = System.DateTime.Now.Second.ToString();


            //ChattingManager
            gameData.wait_next_chat_max = 350;
            gameData.wait_next_chat_min = 150;

            //Event
            gameData.profile_btn = false;
            gameData.memo_btn = false;

 


        }
        //ScenarioManager
        scenarioManager.scenario_Main_Num = gameData.scenario_Main_Num;
        scenarioManager.watch_scenario = gameData.watch_scenario;
        scenarioManager.notWatch = gameData.notWatch;
        scenarioManager.first_turning_point = gameData.first_turning_point;
        scenarioManager.second_turning_point = gameData.second_turning_point;
        scenarioManager.third_turning_point = gameData.third_turning_point;
        scenarioManager.four_turning_point = gameData.four_turning_point ;
        scenarioManager.five_turning_point = gameData.five_turning_point ;
        scenarioManager.six_turning_point = gameData.six_turning_point ;
        scenarioManager.seven_turning_point = gameData.seven_turning_point ;

        //RealTIme_Event_Trigger
        realTime_Event_Trigger.init = gameData.init;
        DateTime temp;
        DateTime.TryParse(gameData.year + '/' + gameData.month + '/' + gameData.day + " " + gameData.hour +':'+ gameData.minute + ':' + gameData.second, out temp);
        realTime_Event_Trigger.startTime = temp;

        Debug.Log("������ ���� ���۰�" + temp);

        //ChattingManager
        chattingManager.wait_next_chat_max = gameData.wait_next_chat_max;
        chattingManager.wait_next_chat_min = gameData.wait_next_chat_min;

        //Event
        scenarioManager.profile_btn = gameData.profile_btn;
        scenarioManager.memo_btn = gameData.memo_btn;



    }
    // ���� �����ϱ�
    public void SaveGameData()
    {
        Debug.Log("���� ����");
        //ScenarioManager
        gameData.scenario_Main_Num = scenarioManager.scenario_Main_Num;
        gameData.watch_scenario = scenarioManager.watch_scenario;
        gameData.notWatch = scenarioManager.notWatch;
        gameData.first_turning_point = scenarioManager.first_turning_point;
        gameData.second_turning_point = scenarioManager.second_turning_point;
        gameData.third_turning_point = scenarioManager.third_turning_point;
        gameData.four_turning_point = scenarioManager.four_turning_point;
        gameData.five_turning_point = scenarioManager.five_turning_point ;
        gameData.six_turning_point  = scenarioManager.six_turning_point;
        gameData.seven_turning_point = scenarioManager.seven_turning_point;

        //RealTIme_Event_Trigger
        gameData.init = realTime_Event_Trigger.init;

        gameData.year = realTime_Event_Trigger.startTime.Year.ToString();
        gameData.month = realTime_Event_Trigger.startTime.Month.ToString();
        gameData.day = realTime_Event_Trigger.startTime.Day.ToString();
        gameData.hour = realTime_Event_Trigger.startTime.Hour.ToString();
        gameData.minute = realTime_Event_Trigger.startTime.Minute.ToString();
        gameData.second = realTime_Event_Trigger.startTime.Second.ToString();


        //ChattingManager
        gameData.wait_next_chat_max = chattingManager.wait_next_chat_max;
        gameData.wait_next_chat_min = chattingManager.wait_next_chat_min;

        //Event
        gameData.profile_btn = scenarioManager.profile_btn;
        gameData.memo_btn = scenarioManager.memo_btn;



        Debug.Log("��� ���� ��");
        string ToJsonData = JsonUtility.ToJson(gameData);
        //string filePath = Application.persistentDataPath + GameDataFileName;
        string filePath = Path.Combine(Application.persistentDataPath, GameDataFileName);
        Debug.Log("��� ���� ��");
        Debug.Log(filePath + "���ϰ��");
        // �̹� ����� ������ �ִٸ� �����
        Debug.Log("��� ��Ʈ�� �߼� ��");
        File.WriteAllText(filePath, ToJsonData);


        print("����Ϸ�");

    }


}
