using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIControl_Ver1 : MonoBehaviour
{
    public Vote_Slider voteSlider;
    public FireBase_Ver1 firebase;
    public GameObject VoteUI;
    public Button AgreeBtn;
    public Button DisagreeBtn;
    public GameObject ResultUI;

    public Text Result_Txt;
    public Image PieChart;

    // Vote UI �� ���� �ִ��� üũ�ϴ� bool ��
    public bool VoteUIOn = false;

    void Update()
    {
        // �ӽ÷� ��ǥ ���� Ʈ���� �����
        if (Input.GetKeyDown(KeyCode.E))
        {
            AppearVote("Vote2");
            Debug.Log("Vote Appear");
        }
        
        
    }

    // Vote UI�� �����ϸ鼭 �� ��ư�� voteName(�̺�Ʈ �̸�)�� ��� �Լ��� �Ѱ���.
    public void AppearVote(string voteName)
    {
        VoteUIOn = true;
        VoteUI.SetActive(true);
        AgreeBtn.onClick.AddListener(delegate { Agree(voteName); });
        DisagreeBtn.onClick.AddListener(delegate { Disagree(voteName); });

        // timer
        voteSlider.SetEndTime(5);
    }

    // Agree ��ư�� �Ҵ�� �Լ�
    public void Agree(string voteName)
    {
        if(VoteUIOn == true)
        {
            firebase.SendVote(voteName, true);
            VoteUIOn = false;
        }
    }

    // Disagree ��ư�� �Ҵ�� �Լ�
    public void Disagree(string voteName)
    {
        if(VoteUI == true)
        {
            firebase.SendVote(voteName, false);
            VoteUIOn = false;
        }
    }

    // 5�ʰ� Result ȭ���� �����ְ� ������ IEnumerator �Լ�
    public IEnumerator viewResult(string voteName)
    {
        VoteUI.SetActive(false);
        firebase.CountVote(voteName);
        yield return new WaitForSeconds(5);
        ResultUI.SetActive(false);
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

}
