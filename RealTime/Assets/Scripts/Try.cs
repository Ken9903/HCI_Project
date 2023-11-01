using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class Try : MonoBehaviour
{
    public Button button;

    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWithOnMainThread(
            task => {

                if (task.Result == Firebase.DependencyStatus.Available)
                {
                    FirebaseInit();
                }
                else
                {
                    Debug.LogError("Version Check Failed");
                }
            
            });    
    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if (senderAuth != null)
        {
            user = senderAuth.CurrentUser;
            if (user != null)
            {
                Debug.Log("user ID is : " + user.UserId);
            }
        }
    }
}
