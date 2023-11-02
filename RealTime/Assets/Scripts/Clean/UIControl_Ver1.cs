using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PixelCrushers.DialogueSystem;

public class UIControl_Ver1 : MonoBehaviour
{
    public Vote_Slider voteSlider;
    public FireBase_Ver1 firebase;
    public GameObject ResultUI;

    public Text Result_Txt;
    public Image PieChart;

    public Button Trigger;

    


    // Agree ��ư�� �Ҵ�� �Լ�
    // Vote2�� Debug�̸�
    public void Agree(string voteName)
    {
          firebase.SendVote(voteName, true);
    }

    // Disagree ��ư�� �Ҵ�� �Լ�
    public void Disagree(string voteName)
    {
          firebase.SendVote(voteName, false);
    }

    // 5�ʰ� Result ȭ���� �����ְ� ������ IEnumerator �Լ�
    public IEnumerator viewResult(string voteName)
    {
        firebase.CountVote(voteName);
        yield return new WaitForSeconds(5);
        ResultUI.SetActive(false);
    }
    public void viewResultStart(string voteName)
    {
        StartCoroutine(viewResult(voteName));
    }

    public void ResultChange(long agreeCount, long DisagreeCount)
    {
        float Percent_A = (((float)agreeCount) / ((float)(agreeCount + DisagreeCount)));
        float Percent_D = (((float)DisagreeCount) / ((float)(agreeCount + DisagreeCount)));

        PieChart.fillAmount = 0f;

        Result_Txt.text = (Percent_A*100).ToString("F1") + " %�� ������� ������" + "\n" +
            (Percent_D*100).ToString("F1") + "%�� ������� �ݴ븦" + "\n" +
            "�����߽��ϴ�.";

        ResultUI.SetActive(true);

        StartCoroutine(ChartEffect(Percent_A));
    }
    

    public IEnumerator ChartEffect(float percent)
    {
        float time = 0f;
        // �ִϸ��̼� ��� �ð�
        float effectTime = 1f;

        while (PieChart.fillAmount < percent)
        {
            time += Time.deltaTime / effectTime;
            PieChart.fillAmount = Mathf.Lerp(0, 1, time);
            yield return null;
        }
    }
    
    public void scenario_vote_count(string voteName)
    {
        firebase.CountVote_makeWay_Scenario(voteName);
    }


    private void OnEnable()
    {
        Lua.RegisterFunction("Agree", this, SymbolExtensions.GetMethodInfo(() => Agree((string)"")));
        Lua.RegisterFunction("Disagree", this, SymbolExtensions.GetMethodInfo(() => Disagree((string)"")));
        Lua.RegisterFunction("viewResultStart", this, SymbolExtensions.GetMethodInfo(() => viewResultStart((string)"")));
        Lua.RegisterFunction("scenario_vote_count", this, SymbolExtensions.GetMethodInfo(() => scenario_vote_count((string)"")));
    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("Agree");
        Lua.UnregisterFunction("Disagree");
        Lua.UnregisterFunction("viewResultStart");
        Lua.UnregisterFunction("scenario_vote_count");
    }

}
