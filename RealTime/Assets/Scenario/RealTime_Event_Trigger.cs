using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //참조
    public ScenarioManager scenarioManager;
    public FireBase_Ver1 fireBase;

    public GameObject waitTimeObj;
    public Text waitTime;


    public float courutine_wait_second = 3f; //최적화 변수


    //시간 관련
    public DateTime startTime = new DateTime(2023,11,22,19,0,0); //***데이터 세이브해서 앱이 재 기동되도 유지해줘야함.
    DateTime currentTime = new DateTime();
    TimeSpan span;

    
    //TriggerTIme
    public int[] triggerTime; //초단위로 설정.(1시간 = 3600) -> 메인 시나리오 넘버랑 연동  , 누적합으로 적어야함
    public int[] waitTriggerTime; //트리거가 걸리고 기다리는 최대 시간


    //***조정 필
    DateTime scenario_1 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_2 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_3 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_4 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_5 = new DateTime(2023, 11, 22, 23, 0, 0);

    IEnumerator checkTrigger()
    {
        while(true) //***시나리오 중에는 체크 안해도 됨.
        {
            double passed_time_ = passed_time();
            Debug.Log(passed_time_);
            TimeSpan diff;
            if(waitTimeObj.activeSelf == true)
            {
                if(scenarioManager.scenario_Main_Num == 0)
                {
                    diff = scenario_1 - DateTime.Now;
                    waitTime.text = diff.ToString(@"hh\:mm\:ss");
                }
                if (scenarioManager.scenario_Main_Num == 1)
                {
                    diff = scenario_2 - DateTime.Now;
                    waitTime.text = diff.ToString(@"hh\:mm\:ss");
                }
                if (scenarioManager.scenario_Main_Num == 2)
                {
                    diff = scenario_3- DateTime.Now;
                    waitTime.text = diff.ToString(@"hh\:mm\:ss");
                }
                if (scenarioManager.scenario_Main_Num == 3)
                {
                    diff = scenario_4 - DateTime.Now;
                    waitTime.text = diff.ToString(@"hh\:mm\:ss");
                }
                if(scenarioManager.scenario_Main_Num == 4)
                {
                    waitTime.text = "모든 방송이 종료 되었습니다";
                }
            }

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(시나리오) -> 트리거 역할 -> 씬이동
                Debug.Log("시나리오 실행" + scenarioManager.scenario_Main_Num + "번");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //다음 시나리오로 이동

                break;
            }

            


            yield return new WaitForSeconds(courutine_wait_second);
        }
        scenarioManager.sceneChange("Scene" + (scenarioManager.scenario_Main_Num - 1));
        StartCoroutine(checkTrigger()); //씬 체인지 후 다시 코루틴 시작

    }
    private void Awake()
    {
     
            Debug.Log("시작 시간 : " + startTime);
            Debug.Log("스타트 타임은 직접 변경해야 합니다");
    }
    // Start is called before the first frame update
    void Start()
    {
        //첫 시작, 늦게 접속한 만큼 시나리오 넘버 올려주기
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //*** 테스트 필요 ***
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //정시가 지났는데 못봄
            {
                if(passed_time_ > waitTriggerTime[i]) //기다리는 한계선 까지 넘음
                {
                    //스타트 할 때 현재 까지 진행된 투표상황, 버튼 상황 초기화
                    if (scenarioManager.scenario_Main_Num == 0)
                    {
                        fireBase.CountVote_makeWay_Scenario("Vote1");
                    }
                    if (scenarioManager.scenario_Main_Num == 1)
                    {
                        fireBase.CountVote_makeWay_Scenario_3("Vote2");
                        scenarioManager.profile_btn = true;
                    }
                    if(scenarioManager.scenario_Main_Num == 2)
                    {
                        fireBase.CountVote_makeWay_Scenario_3("Vote3");
                        if(scenarioManager.third_turning_point == 0)
                        {
                            scenarioManager.memo_btn = true;
                        }                    
                    }
                    if(scenarioManager.scenario_Main_Num == 3)
                    {
                        //해당 시나리오 보트 결과 추가
                    }
                    if (scenarioManager.scenario_Main_Num == 4)
                    {
                        //해당 시나리오 보트 결과 추가
                    }



                    //못봤으니 다음 메인 시나리오로 이동
                    scenarioManager.watch_scenario[i] = true;
                    scenarioManager.scenario_Main_Num++;
                   
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
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
