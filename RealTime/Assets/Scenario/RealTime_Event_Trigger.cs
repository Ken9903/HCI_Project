using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //����
    public ScenarioManager scenarioManager;
    public LocalNotification localNotification;
    public DataController dataController;
    public FireBase_Ver1 fireBase;


    public float courutine_wait_second = 3f; //����ȭ ����

    public bool init = false;  //***������ ���̺��ؼ� ���� �� �⵿�ǵ� �����������.


    //�ð� ����
    public DateTime startTime = new DateTime(); //***������ ���̺��ؼ� ���� �� �⵿�ǵ� �����������.
    DateTime currentTime = new DateTime();
    TimeSpan span;

    
    //TriggerTIme
    public int[] triggerTime; //�ʴ����� ����.(1�ð� = 3600) -> ���� �ó����� �ѹ��� ����  , ���������� �������
    public int[] waitTriggerTime; //Ʈ���Ű� �ɸ��� ��ٸ��� �ִ� �ð�

    IEnumerator checkTrigger()
    {
        while(true) //***�ó����� �߿��� üũ ���ص� ��.
        {
            double passed_time_ = passed_time();
            //Debug.Log(passed_time_);

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(�ó�����) -> Ʈ���� ���� -> ���̵�
                Debug.Log("�ó����� ����" + scenarioManager.scenario_Main_Num + "��");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //���� �ó������� �̵�

                //������ ���̺�
                dataController.SaveGameData();

                //�� �̵�
                scenarioManager.sceneChange("Scene" + (scenarioManager.scenario_Main_Num - 1));
            }


            //�˶� �ʱ�ȭ //SpanTime�� �Ű����� *** �׽�Ʈ �ʿ� *** �ѹ� �˶��߰� �Ⱥ��� ���� �˶� �ȶ� -> �ѹ��� ������ �˶� ����
            TimeSpan nextTimeSpan = new TimeSpan(0, 0, 0, triggerTime[scenarioManager.scenario_Main_Num]);
            localNotification.AddLocalNotification(nextTimeSpan - span); // ���������� �˶��� �ʱ�ȭ -> ���� -> �ʱ�ȸ -> ���� ����
            


            yield return new WaitForSeconds(courutine_wait_second);
        }

    }
    private void Awake()
    {
        // init üũ�� ������ �ε� ����
        dataController.LoadGameData();
        //

        if(init == false)
        {
            startTime = DateTime.Now;
            Debug.Log("���� �ð� �ʱ�ȭ �Ϸ�");
            init = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //ù ����, �ʰ� ������ ��ŭ �ó����� �ѹ� �÷��ְ� Notwatch �ø���
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //*** �׽�Ʈ �ʿ� ***
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //���ð� �����µ� ����
            {
                if(passed_time_ > waitTriggerTime[i]) //��ٸ��� �Ѱ輱 ���� ����
                {
                    //�� ���� ���� ����
                    Debug.Log("���þ�...");
                    //�ټ��� ������ ������ ���� ó�� �ؾ���
                    Debug.Log("�ó�����" + scenarioManager.scenario_Main_Num + "�� ����");
                    if(scenarioManager.scenario_Main_Num == 0)
                    {
                        fireBase.CountVote_makeWay_Scenario("Vote1");
                        Debug.Log("�۽�Ʈ �ʹ�����Ʈ ���� " + scenarioManager.first_turning_point + "��");
                    }
                    else if(scenarioManager.scenario_Main_Num == 1)
                    {
                        fireBase.CountVote_makeWay_Scenario_3("Vote2");
                        Debug.Log("������ �ʹ�����Ʈ ����" + scenarioManager.second_turning_point + "��");
                    }
                   else if(scenarioManager.scenario_Main_Num == 2)
                    {

                        fireBase.CountVote_makeWay_Scenario("Vote3");
                        Debug.Log("������ �ʹ�����Ʈ ����" + scenarioManager.second_turning_point + "��");
                    }
                    else if(scenarioManager.scenario_Main_Num == 3)
                    {
                        Debug.Log("�ۼ� ���");
                    }
                    else if(scenarioManager.scenario_Main_Num == 4)
                    {
                        Debug.Log("�ۼ� ���");
                    }
                    else
                    {
                        Debug.Log("Not watch Error");
                    }
                    //���� ������
                    //***
                    scenarioManager.notWatch++;

                    //�������� ���� ���� �ó������� �̵�
                    scenarioManager.watch_scenario[i] = true;
                    scenarioManager.scenario_Main_Num++;

                    dataController.SaveGameData();
                }
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
