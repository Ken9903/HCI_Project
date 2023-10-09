using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable] // Á÷·ÄÈ­
public class GameData
{
    //ScenarioManager
    public int scenario_Main_Num;
    public bool[] watch_scenario;
    public int notWatch;

    //RealTIme_Event_Trigger
    public bool init;
    //public DateTime startTime;
    public string year;
    public string month;
    public string day;
    public string hour;
    public string minute;
    public string second;


    //ChattingManager
    public int wait_next_chat_max;
    public int wait_next_chat_min;

}
