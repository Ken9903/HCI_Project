using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime_Event_Trigger : MonoBehaviour
{
    //����
    public ScenarioManager scenarioManager;


    public float courutine_wait_second = 3f; //����ȭ ����

    public bool init = false;  //***������ ���̺��ؼ� ���� �� �⵿�ǵ� �����������.


    //�ð� ����
    DateTime startTime = new DateTime(); //***������ ���̺��ؼ� ���� �� �⵿�ǵ� �����������.
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
            Debug.Log(passed_time_);

            if (passed_time_ >= triggerTime[scenarioManager.scenario_Main_Num])
            {
                //play(�ó�����) -> Ʈ���� ���� -> ���̵�
                Debug.Log("�ó����� ����" + scenarioManager.scenario_Main_Num + "��");
                scenarioManager.watch_scenario[scenarioManager.scenario_Main_Num] = true;
                //
                scenarioManager.scenario_Main_Num++; //���� �ó������� �̵�
            }
            yield return new WaitForSeconds(courutine_wait_second);
        }

    }
    private void Awake()
    {
        //
        //init,startTime ���̺� �ҷ�����
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
        //ù ����, �ʰ� ������ ��ŭ �ó����� �ѹ� �÷��ְ� Notwatch �ø���
        double passed_time_ = passed_time();
        for(int i = 0; i < scenarioManager.scenario_count; i++) //***�׽�Ʈ ���� ���غ�
        {
            if(passed_time_ >= triggerTime[i] && scenarioManager.watch_scenario[i] == false) //���ð� �����µ� ����
            {
                if(passed_time_ > waitTriggerTime[i]) //��ٸ��� �Ѱ輱 ���� ����
                {
                    //�� ���� ���� ����
                    Debug.Log("���þ�...");
                    //�ټ��� ������ ������ ���� ó�� �ؾ���
                    Debug.Log("�ó�����" + scenarioManager.scenario_Main_Num + "�� ����");
                    //***
                    scenarioManager.notWatch++;

                    //�������� ���� ���� �ó������� �̵�
                    scenarioManager.watch_scenario[i] = true;
                    scenarioManager.scenario_Main_Num++;
                }
            }
        }


        //���� üũ ����
        StartCoroutine(checkTrigger());
    }

    private double passed_time()
    {
        currentTime = DateTime.Now;
        span = currentTime - startTime;

        return span.TotalSeconds;
    }



}
