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

    // Vote UI 가 켜져 있는지 체크하는 bool 값
    public bool VoteUIOn = false;

    void Update()
    {
        // 임시로 투표 등장 트리거 만들기
        if (Input.GetKeyDown(KeyCode.E))
        {
            AppearVote("Vote2");
            Debug.Log("Vote Appear");
        }
        
        
    }

    // Vote UI를 생성하면서 각 버튼에 voteName(이벤트 이름)이 담긴 함수를 넘겨줌.
    public void AppearVote(string voteName)
    {
        VoteUIOn = true;
        VoteUI.SetActive(true);
        AgreeBtn.onClick.AddListener(delegate { Agree(voteName); });
        DisagreeBtn.onClick.AddListener(delegate { Disagree(voteName); });

        // timer
        voteSlider.SetEndTime(5);
    }

    // Agree 버튼에 할당될 함수
    public void Agree(string voteName)
    {
        if(VoteUIOn == true)
        {
            firebase.SendVote(voteName, true);
            VoteUIOn = false;
        }
    }

    // Disagree 버튼에 할당될 함수
    public void Disagree(string voteName)
    {
        if(VoteUI == true)
        {
            firebase.SendVote(voteName, false);
            VoteUIOn = false;
        }
    }

    // 5초간 Result 화면을 보여주고 꺼지는 IEnumerator 함수
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

        Result_Txt.text = (Percent_A*100).ToString("F1") + " %의 사람들이 찬성을" + "\n" +
            (Percent_D*100).ToString("F1") + "%의 사람들이 반대를" + "\n" +
            "선택했습니다.";

        ResultUI.SetActive(true);

        StartCoroutine(ChartEffect(Percent_A));
    }
    

    public IEnumerator ChartEffect(float percent)
    {
        float time = 0f;
        // 애니메이션 재생 시간
        float effectTime = 1f;

        while (PieChart.fillAmount < percent)
        {
            time += Time.deltaTime / effectTime;
            PieChart.fillAmount = Mathf.Lerp(0, 1, time);
            yield return null;
        }
    }

}
