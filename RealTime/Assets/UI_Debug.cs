using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Debug : MonoBehaviour
{
    public Text scenario_text;
    public Text time_text;
    public Text notWatch_text;

    public ScenarioManager scenarioManager;
    public RealTime_Event_Trigger realTime_Event_Trigger;

    IEnumerator debug()
    {
        while(true)
        {
            scenario_text.text = scenarioManager.scenario_Main_Num.ToString();
            time_text.text = realTime_Event_Trigger.passed_time().ToString();
            notWatch_text.text = scenarioManager.notWatch.ToString();
            yield return new WaitForSeconds(1);
        }

    }

    private void Start()
    {
        StartCoroutine(debug());
    }



}
