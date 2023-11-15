using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //참조
    public ScenarioManager scenarioManager;
    public LocalNotification localNotification;
    public DataController dataController;
    public FireBase_Ver1 fireBase;


    public float courutine_wait_second = 3f; //최적화 변수

    public bool init = false;  //***데이터 세이브해서 앱이 재 기동되도 유지해줘야함.


    //시간 관련
    public DateTime startTime = new DateTime(); //***데이터 세이브해서 앱이 재 기동되도 유지해줘야함.
    DateTime currentTime = new DateTime();
    TimeSpan span;

    
    //TriggerTIme
    public int[] triggerTime; //초단위로 설정.(1시간 = 3600) -> 메인 시나리오 넘버랑 연동  , 누적합으로 적어야함
    public int[] waitTriggerTime; //트리거가 걸리고 기다리는 최대 시간

    IEnumerator checkTrigger()
    {
        while(true) //***시나리오 중에는 체크 안해도 됨.
        {
            double passed_time_ = passed_time();
            //Debug.Log(passed_time_);

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(시나리오) -> 트리거 역할 -> 씬이동
                Debug.Log("시나리오 실행" + scenarioManager.scenario_Main_Num + "번");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //다음 시나리오로 이동

                //데이터 세이브
                dataController.SaveGameData();

                //씬 이동
                scenarioManager.sceneChange("Scene" + (scenarioManager.scenario_Main_Num - 1));
            }


            //알람 초기화 //SpanTime이 매개변수 *** 테스트 필요 *** 한번 알람뜨고 안보면 다음 알람 안뜸 -> 한번에 여러개 알람 생성
            TimeSpan nextTimeSpan = new TimeSpan(0, 0, 0, triggerTime[scenarioManager.scenario_Main_Num]);
            localNotification.AddLocalNotification(nextTimeSpan - span); // 지속적으로 알람을 초기화 -> 생성 -> 초기회 -> 생성 해줌
            


            yield return new WaitForSeconds(courutine_wait_second);
        }

    }
    private void Awake()
    {
        // init 체크전 데이터 로드 보장
        dataController.LoadGameData();
        //

        if(init == false)
        {
            startTime = DateTime.Now;
            Debug.Log("시작 시간 초기화 완료");
            init = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //첫 시작, 늦게 접속한 만큼 시나리오 넘버 올려주고 Notwatch 올리기
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //*** 테스트 필요 ***
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //정시가 지났는데 못봄
            {
                if(passed_time_ > waitTriggerTime[i]) //기다리는 한계선 까지 넘음
                {
                    //못 봤을 때의 반응
                    Debug.Log("못봤어...");
                    //다수를 못봤을 때에는 따로 처리 해야함
                    Debug.Log("시나리오" + scenarioManager.scenario_Main_Num + "번 못봄");
                    if(scenarioManager.scenario_Main_Num == 0)
                    {
                        fireBase.CountVote_makeWay_Scenario("Vote1");
                        Debug.Log("퍼스트 터닝포인트 변경 " + scenarioManager.first_turning_point + "번");
                    }
                    else if(scenarioManager.scenario_Main_Num == 1)
                    {
                        fireBase.CountVote_makeWay_Scenario_3("Vote2");
                        Debug.Log("세컨드 터닝포인트 변경" + scenarioManager.second_turning_point + "번");
                    }
                   else if(scenarioManager.scenario_Main_Num == 2)
                    {

                        fireBase.CountVote_makeWay_Scenario("Vote3");
                        Debug.Log("세컨드 터닝포인트 변경" + scenarioManager.second_turning_point + "번");
                    }
                    else if(scenarioManager.scenario_Main_Num == 3)
                    {
                        Debug.Log("작성 요망");
                    }
                    else if(scenarioManager.scenario_Main_Num == 4)
                    {
                        Debug.Log("작성 요망");
                    }
                    else
                    {
                        Debug.Log("Not watch Error");
                    }
                    //쭉쭉 나가기
                    //***
                    scenarioManager.notWatch++;

                    //못봤으니 다음 메인 시나리오로 이동
                    scenarioManager.watch_scenario[i] = true;
                    scenarioManager.scenario_Main_Num++;

                    dataController.SaveGameData();
                }
            }
        }


        //메인 체크 시작 -> 모든 시나리오는 여기서 시작
        StartCoroutine(checkTrigger());
    }

    public double passed_time()
    {
        currentTime = DateTime.Now;
        span = currentTime - startTime;

        return span.TotalSeconds;
    }



}
