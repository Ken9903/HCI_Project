using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //참조
    public ScenarioManager scenarioManager;


    public float courutine_wait_second = 3f; //최적화 변수

    public bool init = false;  //***데이터 세이브해서 앱이 재 기동되도 유지해줘야함.


    //시간 관련
    DateTime startTime = new DateTime(); //***데이터 세이브해서 앱이 재 기동되도 유지해줘야함.
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
            Debug.Log(passed_time_);

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(시나리오) -> 트리거 역할 -> 씬이동
                Debug.Log("시나리오 실행" + scenarioManager.scenario_Main_Num + "번");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //다음 시나리오로 이동
            }
            yield return new WaitForSeconds(courutine_wait_second);
        }

    }
    private void Awake()
    {
        //
        //init,startTime 세이브 불러오기
        //

        if(init == false)
        {
            startTime = DateTime.Now;
            init = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //첫 시작, 늦게 접속한 만큼 시나리오 넘버 올려주고 Notwatch 올리기
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //***테스트 아직 못해봄
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //정시가 지났는데 못봄
            {
                if(passed_time_ > waitTriggerTime[i]) //기다리는 한계선 까지 넘음
                {
                    //못 봤을 때의 반응
                    Debug.Log("못봤어...");
                    //다수를 못봤을 때에는 따로 처리 해야함
                    Debug.Log("시나리오" + scenarioManager.scenario_Main_Num + "번 못봄");
                    //***
                    scenarioManager.notWatch++;

                    //못봤으니 다음 메인 시나리오로 이동
                    scenarioManager.watch_scenario[i] = true;
                    scenarioManager.scenario_Main_Num++;
                }
            }
        }


        //메인 체크 시작
        StartCoroutine(checkTrigger());
    }

    private double passed_time()
    {
        currentTime = DateTime.Now;
        span = currentTime - startTime;

        return span.TotalSeconds;
    }



}
