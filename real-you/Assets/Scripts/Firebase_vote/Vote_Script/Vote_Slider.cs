using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Vote_Slider : MonoBehaviour
{
    /*
    public Slider timer;
    public float delay = 5f;
    
    // Ÿ�̸� ���� �ð�, �ӽ÷� 300f�� ����  
    public float timeover = 300f;
    
    // ���� �ð����� ���� �ð�
    public float FinishTime;
    public UIControl_Ver1 uiController;

    void Update()
    {
        // Trigger
        if (Input.GetKeyDown(KeyCode.Q))
        {
            timer.value = 0f;
            SetEndTime(5);
        }

        // Timer �۵�
        if(uiController.VoteUIOn == true)
        {
            timer.value = LeftTime(delay);
            if(timer.value >= timeover)
            {
                Debug.Log("�ð� ������ �� �Ǿ����ϴ�.");
                TimeOverEvent();
            }
        }
    }

    // �ð��� ����Ǿ��� �� �߻��ϴ� �̺�Ʈ
    public void TimeOverEvent()
    {
        // vote UI disable
        uiController.VoteUI.SetActive(false);
    }

    // 
    public float LeftTime(float delay)
    {
        return (delay*60) - (PlayerPrefs.GetFloat("EndTime") - (float)DateTime.Now.TimeOfDay.TotalSeconds);
    }

    public void SetEndTime(double delay)
    {
        float End_time = (int)DateTime.Now.AddMinutes(delay).TimeOfDay.TotalSeconds;
        FinishTime = End_time;
        PlayerPrefs.SetFloat("EndTime", End_time);
    }
    */
}
