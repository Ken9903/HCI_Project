using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FireBase_Ver1 : MonoBehaviour
{

    public UIControl_Ver1 uiController;

    private FirebaseAuth auth;
    private FirebaseUser user;

    public ScenarioManager scenarioManager;
    public RealTime_Event_Trigger realTime_Event_Trigger;

    public DataController dataController;

    public bool tasking = false;


    // Start is called before the first frame update
    void Awake()
    {

        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                Debug.Log(Firebase.DependencyStatus.Available + "파이어 베이스 어베일");
                Debug.Log(task.Result + "파이어 베이스 Result");


                FirebaseInit();
            }
            else
            {
                Debug.LogError("Version Check Failed");
            }
        });

    }

    private void SignInAnonymous()
    {
        Debug.Log("익명의 로그인으로 실행");

        Debug.Log("Try");
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result.User;
            if (user != null)
                Debug.Log("태스크 유저 싱크 어나니머스 " + user.UserId);
            else
                Debug.Log("User is null");

            if(task.IsCompleted)
            {
                Debug.Log("태스크 컴플리티트");
                if (user == null)
                {
                    Debug.Log("익명 로그인 NULL");
                }
                else
                {
                    realTime_Event_Trigger.main_start();
                    Debug.Log("로그인 끝 메인 스타트 시작");
                }
                Debug.Log("user ID is : " + user.UserId);

            }
            else
            {
                Debug.Log("태스크 Fail");
            }
        });

       
    }
    /* //기존 코드
    public Task SigninAnonymous()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(
            task => {

                if (task.IsFaulted)
                {
                    Debug.LogError("Sign in Failed");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Sign in Complete");
                }
            
            });
    }
    */

    public void SignOut()
    {
        auth.SignOut();
    }

    private void FirebaseInit()
    {
        Debug.Log("FirebaseInit시작");
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log(auth);
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
     
        Debug.Log("AuthStateChanged 실행");
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;

            if(user != null && user.IsValid())
            {
                Debug.Log("user ID is : " + user.UserId);
                realTime_Event_Trigger.main_start();
            }
            else
            {
                Debug.Log("user == null");
                SignInAnonymous();
            }
            
        }
        else
        {
            Debug.Log("senderAuth == null");
        }

    }

    // 여기서부터는 Vote를 Sending 하는 과정
    // string으로 받아오는 변수는 일어나는 투표 이벤트의 이름
    // 주의해야할 점**은 규칙에서 read write의 권한을 허용하지 않으면 데이터 변경이 불가능
    public void SendVote(string voteName, bool vote)
    {
        // 기본 데이터 베이스 불러오기
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);
        
        // 찬성과 반대에 따라 데이터를 입력할 데이터 베이스 위치 설정
        if(vote) { voteDB = voteDB.Child("Agree"); }
        else { voteDB = voteDB.Child("Disagree"); }
        // 푸시할 데이터를 구별시켜줄 고유한 Key 생성
        string key = voteDB.Push().Key;

        // Dictionary 형의 입력할 데이터 설정, 임시로 UID를 데이터로 설정
        // 설정 데이터에 따라 투표한 시간 등을 데이터로 넘겨줄 수도 있음.
        Dictionary<string, object> voteData = new Dictionary<string, object>();
        voteData.Add("username", user.UserId);
        voteData.Add("Question", 11);
        
        // 위에 데이터를 넣으면 데이터 두 개가 병렬로 들어가니까
        // 1개의 데이터만 추가하기 위해서 데이터를 병합시켜서 추가해줌
        Dictionary<string, object> updateVote = new Dictionary<string, object>();
        updateVote.Add(key, voteData);

        voteDB.UpdateChildrenAsync(updateVote).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Update Vote Complete");
                    //StartCoroutine(uiController.viewResult(voteName));
                }
                else
                {
                    Debug.Log("update vote fail");
                }
            });
    }


    // 입력된 Vote 데이터를 불러오는 과정
    public void CountVote(string voteName)
    {
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        voteDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Read Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // 쓰레드 안에서 변수를 return 해주는 게 불가능 해서 다른 함수로 넘겨줘야 함.
                    // 기타 사항으로 foreach문을 통해 각각의 데이터를 불러올 수도 있음.
                    // 찬성 유저수, 반대 유저수를 Debug로 찍어보기
                    Debug.Log("Agree Count : " + snapshot.Child("Agree").ChildrenCount);
                    uiController.ResultChange(snapshot.Child("Agree").ChildrenCount,
                        snapshot.Child("Disagree").ChildrenCount);
                }
            });
    }

    public void CountVote_makeWay_Scenario(string voteName)
    {
        tasking = true;
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        voteDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    CountVote_makeWay_Scenario(voteName);
                    Debug.LogError("Read Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // 쓰레드 안에서 변수를 return 해주는 게 불가능 해서 다른 함수로 넘겨줘야 함.
                    // 기타 사항으로 foreach문을 통해 각각의 데이터를 불러올 수도 있음.
                    // 찬성 유저수, 반대 유저수를 Debug로 찍어보기
                    Debug.Log("Agree Count : " + snapshot.Child("Agree").ChildrenCount);
                    if(voteName == "Vote1")
                    {
                        Debug.Log("vote1");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.first_turning_point = 1;
                            Debug.Log("vote1 -> 1");
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.first_turning_point = 0;
                            Debug.Log("vote1 -> 0");
                        }
                    }
                    else if(voteName == "Vote2")
                    {
                        Debug.Log("Vote2 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.second_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.second_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote3")
                    {
                        Debug.Log("Vote3 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.third_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.third_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote4")
                    {
                        Debug.Log("Vote4 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.four_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.four_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote5")
                    {
                        Debug.Log("Vote5 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.five_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.five_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote6")
                    {
                        Debug.Log("Vote6 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.six_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.six_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote7")
                    {
                        Debug.Log("Vote7 투표 집계");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.seven_turning_point = 1;
                        }
                        else //같은 경우 처리 필요
                        {
                            scenarioManager.seven_turning_point = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("VoteName Error");
                    }
                    //dataController.SaveGameData();
                    tasking = false;

                }
            });
    }

    public void SendVote_More3(string voteName, int SelectNum)
    {
        // 기본 데이터 베이스 불러오기
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        Debug.Log("변수는 : " + "Select" + SelectNum.ToString());

        // "Select + N" 변수로 선택된 
        voteDB = voteDB.Child("Select" + SelectNum.ToString());

        // 푸시할 데이터를 구별시켜줄 고유한 Key 생성
        string key = voteDB.Push().Key;


        Dictionary<string, object> voteData = new Dictionary<string, object>();
        voteData.Add("username", user.UserId);
        voteData.Add("Question", 11);

        Dictionary<string, object> updateVote = new Dictionary<string, object>();
        updateVote.Add(key, voteData);

        voteDB.UpdateChildrenAsync(updateVote).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("투표 완료");
                }
            });
    }

    public void CountVote_More3(string voteName)
    {
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        voteDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Read Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    uiController.ResultChange_More3(snapshot);
                }
            });
    }
    public void CountVote_makeWay_Scenario_3(string voteName)
    {
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        voteDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    CountVote_makeWay_Scenario_3(voteName);
                    Debug.LogError("Read Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.Child("Select1").ChildrenCount);
                    Debug.Log(snapshot.Child("Select2").ChildrenCount);
                    Debug.Log(snapshot.Child("Select3").ChildrenCount);
                    // 쓰레드 안에서 변수를 return 해주는 게 불가능 해서 다른 함수로 넘겨줘야 함.
                    // 기타 사항으로 foreach문을 통해 각각의 데이터를 불러올 수도 있음.
                    // 찬성 유저수, 반대 유저수를 Debug로 찍어보기
                    if (snapshot.Child("Select1").ChildrenCount >= snapshot.Child("Select2").ChildrenCount && snapshot.Child("Select1").ChildrenCount >= snapshot.Child("Select3").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 1;
                        Debug.Log("1번 당선");
                    }
                  else if(snapshot.Child("Select2").ChildrenCount >= snapshot.Child("Select1").ChildrenCount && snapshot.Child("Select2").ChildrenCount >= snapshot.Child("Select3").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 2;
                        Debug.Log("2번 당선");
                    }
                  else if(snapshot.Child("Select3").ChildrenCount >= snapshot.Child("Select1").ChildrenCount && snapshot.Child("Select3").ChildrenCount >= snapshot.Child("Select2").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 3;
                        Debug.Log("3번 당선");
                    }
                  else
                    {
                        Debug.Log("3개 투표 결과 에러");
                    }

                    //dataController.SaveGameData();

                }
            });
    }

}
