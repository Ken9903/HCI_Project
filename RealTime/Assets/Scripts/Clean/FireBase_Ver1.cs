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


    // Start is called before the first frame update
    void Awake()
    {

        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseInit();
            }
            else
            {
                Debug.LogError("Version Check Failed");
            }
        });

    }

    public void SignIn()
    {
        SignInAnonymous();
    }

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

    public void SignOut()
    {
        auth.SignOut();
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
                Debug.Log("ID��οϷ�");
            }
            else
            {
                SigninAnonymous();
            }
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
                    //StartCoroutine(uiController.viewResult(voteName));
                }
                else
                {
                    Debug.Log("update vote fail");
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

    public void CountVote_makeWay_Scenario(string voteName)
    {
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        voteDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Read Error_2");
                    CountVote_makeWay_Scenario(voteName);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // ������ �ȿ��� ������ return ���ִ� �� �Ұ��� �ؼ� �ٸ� �Լ��� �Ѱ���� ��.
                    // ��Ÿ �������� foreach���� ���� ������ �����͸� �ҷ��� ���� ����.
                    // ���� ������, �ݴ� �������� Debug�� ����
                    Debug.Log("Agree Count : " + snapshot.Child("Agree").ChildrenCount);
                    if(voteName == "Vote1")
                    {
                        Debug.Log("vote1");
                        if (snapshot.Child("Agree").ChildrenCount > snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.first_turning_point = 1;
                            Debug.Log("vote1 -> 1");
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.first_turning_point = 0;
                            Debug.Log("vote1 -> 0");
                        }
                    }
                    else if(voteName == "Vote2")
                    {
                        if (snapshot.Child("Agree").ChildrenCount > snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.second_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.second_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote3")
                    {
                        if (snapshot.Child("Agree").ChildrenCount > snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.third_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.third_turning_point = 0;
                        }
                    }
                    else
                    {
                        //���� �߰�
                    }

                }
            });
    }

    public void SendVote_More3(string voteName, int SelectNum)
    {
        // �⺻ ������ ���̽� �ҷ�����
        DatabaseReference voteDB = FirebaseDatabase.DefaultInstance.GetReference(voteName);

        Debug.Log("������ : " + "Select" + SelectNum.ToString());

        // "Select + N" ������ ���õ� 
        voteDB = voteDB.Child("Select" + SelectNum.ToString());

        // Ǫ���� �����͸� ���������� ������ Key ����
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
                    Debug.Log("��ǥ �Ϸ�");
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
                    Debug.LogError("Read Error_3");
                    CountVote_makeWay_Scenario_3(voteName);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.Child("Select1").ChildrenCount);
                    Debug.Log(snapshot.Child("Select2").ChildrenCount);
                    Debug.Log(snapshot.Child("Select3").ChildrenCount);
                    // ������ �ȿ��� ������ return ���ִ� �� �Ұ��� �ؼ� �ٸ� �Լ��� �Ѱ���� ��.
                    // ��Ÿ �������� foreach���� ���� ������ �����͸� �ҷ��� ���� ����.
                    // ���� ������, �ݴ� �������� Debug�� ����
                    if (snapshot.Child("Select1").ChildrenCount >= snapshot.Child("Select2").ChildrenCount && snapshot.Child("Select1").ChildrenCount >= snapshot.Child("Select3").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 1;
                        Debug.Log("1�� �缱");
                    }
                  else if(snapshot.Child("Select2").ChildrenCount >= snapshot.Child("Select1").ChildrenCount && snapshot.Child("Select2").ChildrenCount >= snapshot.Child("Select3").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 2;
                        Debug.Log("2�� �缱");
                    }
                  else if(snapshot.Child("Select3").ChildrenCount >= snapshot.Child("Select1").ChildrenCount && snapshot.Child("Select3").ChildrenCount >= snapshot.Child("Select2").ChildrenCount)
                    {
                        scenarioManager.second_turning_point = 3;
                        Debug.Log("3�� �缱");
                    }
                  else
                    {
                        Debug.Log("3�� ��ǥ ��� ����");
                    }



                }
            });
    }

}
