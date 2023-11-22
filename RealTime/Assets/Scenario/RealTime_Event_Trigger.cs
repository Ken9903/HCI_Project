using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //����
    public ScenarioManager scenarioManager;
    public FireBase_Ver1 fireBase;

    public GameObject waitTimeObj;
    public Text waitTime;


    public float courutine_wait_second = 3f; //����ȭ ����


    //�ð� ����
    public DateTime startTime = new DateTime(2023,11,22,19,0,0); //***������ ���̺��ؼ� ���� �� �⵿�ǵ� �����������.
    DateTime currentTime = new DateTime();
    TimeSpan span;

    
    //TriggerTIme
    public int[] triggerTime; //�ʴ����� ����.(1�ð� = 3600) -> ���� �ó����� �ѹ��� ����  , ���������� �������
    public int[] waitTriggerTime; //Ʈ���Ű� �ɸ��� ��ٸ��� �ִ� �ð�


    //***���� ��
    DateTime scenario_1 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_2 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_3 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_4 = new DateTime(2023, 11, 22, 23, 0, 0);
    DateTime scenario_5 = new DateTime(2023, 11, 22, 23, 0, 0);

    IEnumerator checkTrigger()
    {
        while(true) //***�ó����� �߿��� üũ ���ص� ��.
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
                    waitTime.text = "��� ����� ���� �Ǿ����ϴ�";
                }
            }

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(�ó�����) -> Ʈ���� ���� -> ���̵�
                Debug.Log("�ó����� ����" + scenarioManager.scenario_Main_Num + "��");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //���� �ó������� �̵�

                break;
            }

            


            yield return new WaitForSeconds(courutine_wait_second);
        }
        scenarioManager.sceneChange("Scene" + (scenarioManager.scenario_Main_Num - 1));
        StartCoroutine(checkTrigger()); //�� ü���� �� �ٽ� �ڷ�ƾ ����

    }
    private void Awake()
    {
     
            Debug.Log("���� �ð� : " + startTime);
            Debug.Log("��ŸƮ Ÿ���� ���� �����ؾ� �մϴ�");
    }
    // Start is called before the first frame update
    void Start()
    {
        //ù ����, �ʰ� ������ ��ŭ �ó����� �ѹ� �÷��ֱ�
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //*** �׽�Ʈ �ʿ� ***
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //���ð� �����µ� ����
            {
                if(passed_time_ > waitTriggerTime[i]) //��ٸ��� �Ѱ輱 ���� ����
                {
                    //��ŸƮ �� �� ���� ���� ����� ��ǥ��Ȳ, ��ư ��Ȳ �ʱ�ȭ
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
                        //�ش� �ó����� ��Ʈ ��� �߰�
                    }
                    if (scenarioManager.scenario_Main_Num == 4)
                    {
                        //�ش� �ó����� ��Ʈ ��� �߰�
                    }



                    //�������� ���� ���� �ó������� �̵�
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


        //���� üũ ���� -> ��� �ó������� ���⼭ ����
        StartCoroutine(checkTrigger());
    }

    public double passed_time()
    {
        currentTime = DateTime.Now;
        span = currentTime - startTime;

        return span.TotalSeconds;
    }



}
