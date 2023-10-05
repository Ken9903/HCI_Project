using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;

#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
using LocalNotification = UnityEngine.iOS.LocalNotification;
#endif

public class LocalNotification : MonoBehaviour
{

    string title;
    string content;
    void Start()
    {
        DeleteNotification();

        #if UNITY_IOS
        NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
        #endif

    }

    void DeleteNotification() //알람 초기화
    {
        #if UNITY_ANDROID
        NotificationManager.CancelAll();
        #elif UNITY_IOS
        NotificationServices.ClearLocalNotifications();
        NotificationServices.CancelAllLocalNotifications();
        #endif
    }
    public void AddLocalNotification(TimeSpan AlertTime) //알람 추가
    {
        DeleteNotification();

        title = "Debug_Title";
        content = "Debug_Contet";

        #if UNITY_ANDROID
        NotificationManager.SendWithAppIcon(AlertTime, title, content, Color.gray, NotificationIcon.Bell);
        Debug.Log("Set Android Notification");    

        #elif UNITY_IOS
        LocalNotification noti = new LocalNotification();
        noti.alertTitle = title;
        noti.alertBody = content;
        noti.soundName = LocalNotification.defaultSoundName;
        noti.applicationIconBadgeNumber = 1;
        noti.fireDate = DateTime.Now.AddSeconds(AlertTime.TotalSeconds);
        NotificationServices.ScheduleLocalNotification(noti);

        #endif
    }


}
