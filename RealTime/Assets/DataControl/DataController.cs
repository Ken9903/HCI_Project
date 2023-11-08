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


    // ---싱글톤으로 선언--- 
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

    // --- 게임 데이터 파일이름 설정 ---
    public string GameDataFileName = "real.json";

    // "원하는 이름(영문).json"
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            // 게임이 시작되면 자동으로 실행되도록
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
        //SaveGameData();
    }
    public void LoadGameData()
    {

        string filePath = Application.persistentDataPath + GameDataFileName;
        Debug.Log(filePath);
        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            print("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);

        }
        // 저장된 게임이 없다면 //초기값 *** 테스트 필요
        else
        {
            print("새로운 파일 생성");
            _gameData = new GameData();
            //ScenarioManager
            gameData.scenario_Main_Num = 0;
            bool[] temp_watch = { false, false, false }; //***나중에 시나리오 크기에 맞춰서 바꿔주기
            gameData.watch_scenario = temp_watch;
            gameData.notWatch = 0;
            gameData.first_turning_point = 0;
            gameData.second_turning_point = 0;
            gameData.third_turning_point = 0;

            //RealTIme_Event_Trigger
            gameData.init = false;

            gameData.year = System.DateTime.Now.Year.ToString();
            gameData.month = System.DateTime.Now.Month.ToString();
            gameData.day = System.DateTime.Now.Day.ToString();
            gameData.hour = System.DateTime.Now.Hour.ToString();
            gameData.minute = System.DateTime.Now.Minute.ToString();
            gameData.second = System.DateTime.Now.Second.ToString();


            //ChattingManager
            gameData.wait_next_chat_max = 50;
            gameData.wait_next_chat_min = 25;



        }
        //ScenarioManager
        scenarioManager.scenario_Main_Num = gameData.scenario_Main_Num;
        scenarioManager.watch_scenario = gameData.watch_scenario;
        scenarioManager.notWatch = gameData.notWatch;

        //RealTIme_Event_Trigger
        realTime_Event_Trigger.init = gameData.init;
        DateTime temp;
        DateTime.TryParse(gameData.year + '/' + gameData.month + '/' + gameData.day + " " + gameData.hour +':'+ gameData.minute + ':' + gameData.second, out temp);
        realTime_Event_Trigger.startTime = temp;

        Debug.Log("데이터 저장 시작값" + temp);

        //ChattingManager
        chattingManager.wait_next_chat_max = gameData.wait_next_chat_max;
        chattingManager.wait_next_chat_min = gameData.wait_next_chat_min;


    }
    // 게임 저장하기
    public void SaveGameData()
    {
        //ScenarioManager
        gameData.scenario_Main_Num = scenarioManager.scenario_Main_Num;
        gameData.watch_scenario = scenarioManager.watch_scenario;
        gameData.notWatch = scenarioManager.notWatch;
        gameData.first_turning_point = scenarioManager.first_turning_point;
        gameData.second_turning_point = scenarioManager.second_turning_point;
        gameData.third_turning_point = scenarioManager.third_turning_point;

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


        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);


        print("저장완료");

    }


}
