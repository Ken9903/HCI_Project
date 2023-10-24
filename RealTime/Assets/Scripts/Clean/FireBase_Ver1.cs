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

    // Debug �� Text
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

    // �͸��� ������ �α��� �ϴ� ����
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

    // Play �Կ� �־ �� ���� ��������, Debugging �������� �ʿ��� ���� �ƿ� ���
    private void SignOut()
    {
        auth.SignOut();
    }


    // ���⼭���ʹ� Vote�� Sending �ϴ� ����
    // string���� �޾ƿ��� ������ �Ͼ�� ��ǥ �̺�Ʈ�� �̸�
    // �����ؾ��� ��**�� ��Ģ���� read write�� ������ ������� ������ ������ ������ �Ұ���
    public void SendVote(string voteName, bool vote)
    {
        // �⺻ ������ ���̽� �ҷ�����
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);
        
        // ������ �ݴ뿡 ���� �����͸� �Է��� ������ ���̽� ��ġ ����
        if(vote) { voteDB = voteDB.Child("Agree"); }
        else { voteDB = voteDB.Child("Disagree"); }
        // Ǫ���� �����͸� ���������� ������ Key ����
        string key = voteDB.Push().Key;

        // Dictionary ���� �Է��� ������ ����, �ӽ÷� UID�� �����ͷ� ����
        // ���� �����Ϳ� ���� ��ǥ�� �ð� ���� �����ͷ� �Ѱ��� ���� ����.
        Dictionary<string, object> voteData = new Dictionary<string, object>();
        voteData.Add("username", user.UserId);
        voteData.Add("Question", 11);
        
        // ���� �����͸� ������ ������ �� ���� ���ķ� ���ϱ�
        // 1���� �����͸� �߰��ϱ� ���ؼ� �����͸� ���ս��Ѽ� �߰�����
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


    // �Էµ� Vote �����͸� �ҷ����� ����
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
                    // ������ �ȿ��� ������ return ���ִ� �� �Ұ��� �ؼ� �ٸ� �Լ��� �Ѱ���� ��.
                    // ��Ÿ �������� foreach���� ���� ������ �����͸� �ҷ��� ���� ����.
                    // ���� ������, �ݴ� �������� Debug�� ����
                    Debug.Log("Agree Count : " + snapshot.Child("Agree").ChildrenCount);
                    uiController.ResultChange(snapshot.Child("Agree").ChildrenCount,
                        snapshot.Child("Disagree").ChildrenCount);
                }
            });
    }

}
