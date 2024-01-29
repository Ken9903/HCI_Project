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
    
    // 타이머 진행 시간, 임시로 300f초 설정  
    public float timeover = 300f;
    
    // 현실 시간으로 종료 시간
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

        // Timer 작동
        if(uiController.VoteUIOn == true)
        {
            timer.value = LeftTime(delay);
            if(timer.value >= timeover)
            {
                Debug.Log("시간 제한이 다 되었습니다.");
                TimeOverEvent();
            }
        }
    }

    // 시간이 종료되었을 때 발생하는 이벤트
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
