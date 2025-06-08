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
                Debug.Log(Firebase.DependencyStatus.Available + "���̾� ���̽� ���");
                Debug.Log(task.Result + "���̾� ���̽� Result");


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
        Debug.Log("�͸��� �α������� ����");

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
                Debug.Log("�½�ũ ���� ��ũ ��ϸӽ� " + user.UserId);
            else
                Debug.Log("User is null");

            if(task.IsCompleted)
            {
                Debug.Log("�½�ũ ���ø�ƼƮ");
                if (user == null)
                {
                    Debug.Log("�͸� �α��� NULL");
                }
                else
                {
                    realTime_Event_Trigger.main_start();
                    Debug.Log("�α��� �� ���� ��ŸƮ ����");
                }
                Debug.Log("user ID is : " + user.UserId);

            }
            else
            {
                Debug.Log("�½�ũ Fail");
            }
        });

       
    }
    /* //���� �ڵ�
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
        Debug.Log("FirebaseInit����");
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log(auth);
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
     
        Debug.Log("AuthStateChanged ����");
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
                    // ������ �ȿ��� ������ return ���ִ� �� �Ұ��� �ؼ� �ٸ� �Լ��� �Ѱ���� ��.
                    // ��Ÿ �������� foreach���� ���� ������ �����͸� �ҷ��� ���� ����.
                    // ���� ������, �ݴ� �������� Debug�� ����
                    Debug.Log("Agree Count : " + snapshot.Child("Agree").ChildrenCount);
                    if(voteName == "Vote1")
                    {
                        Debug.Log("vote1");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
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
                        Debug.Log("Vote2 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
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
                        Debug.Log("Vote3 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.third_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.third_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote4")
                    {
                        Debug.Log("Vote4 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.four_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.four_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote5")
                    {
                        Debug.Log("Vote5 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.five_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.five_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote6")
                    {
                        Debug.Log("Vote6 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.six_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
                        {
                            scenarioManager.six_turning_point = 0;
                        }
                    }
                    else if (voteName == "Vote7")
                    {
                        Debug.Log("Vote7 ��ǥ ����");
                        if (snapshot.Child("Agree").ChildrenCount >= snapshot.Child("Disagree").ChildrenCount)
                        {
                            scenarioManager.seven_turning_point = 1;
                        }
                        else //���� ��� ó�� �ʿ�
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
                    CountVote_makeWay_Scenario_3(voteName);
                    Debug.LogError("Read Error");
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

                    //dataController.SaveGameData();

                }
            });
    }

}
