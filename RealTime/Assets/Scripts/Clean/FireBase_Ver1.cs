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

    // Debug 용 Text
    public Text Debug_Text_user;

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Result == Firebase.DependencyStatus.Available)
            {
                Debug.Log("start");
                FirebaseInit();
            }
            else
            {
                Debug.LogError("Version Check Failed");
            }
        });
    }

    void Update()
    {
        
    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;
            if(user != null)
            {
                Debug.Log("user ID is : " + user.UserId);
                Debug_Text_user.text = "User ID : " + user.UserId;
            }
            else
            {
                Debug.Log("2");
            }
        }
        else
        {
            Debug.Log("1");
        }
    }

    // 익명의 유저로 로그인 하는 과정
    private Task SignInAnonymous()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) 
            { 
                Debug.LogError("Sigh In Failed"); 
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sign In Complete");
            }
        });
    }

    // Play 함에 있어서 쓸 일은 없겠지만, Debugging 과정에서 필요할 사인 아웃 기능
    private void SignOut()
    {
        auth.SignOut();
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
                    StartCoroutine(uiController.viewResult(voteName));
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

}
