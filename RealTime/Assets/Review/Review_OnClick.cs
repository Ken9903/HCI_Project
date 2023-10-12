using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Review_OnClick : MonoBehaviour
{
    int ui_scenario_num; //시나리오 진행하면 이 것 넣어주고 푸쉬
    
    public void onClick()
    {
        Debug.Log(ui_scenario_num);
        /*
         * if(ui_scena_num == 1)
         * {
         *      처리
         * }
         * else if -> 2 : 처리
         * ...쭉쭉 처리
         */
    }
}
