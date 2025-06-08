using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Rotation_Set_Left : MonoBehaviour
{
    IEnumerator left_rot(System.Single degree)
    {
        for (int i = 0; i < degree; i++)
        {
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(0, -0.1f, 0);
            yield return new WaitForSeconds(0.03f);
        }
    }
    IEnumerator right_rot(System.Single degree)
    {
        for (int i = 0; i < degree; i++)
        {
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(0, 0.1f, 0);
            yield return new WaitForSeconds(0.03f);
        }
    }
    IEnumerator backPos(System.Single degree)
    {
        for (int i = 0; i < degree ; i++)
        {
            Vector3 pos = this.transform.position;
            pos.x += 0.01f;
            this.transform.position = pos;
            yield return new WaitForSeconds(0.03f);
        }
    }
    public void left_rotation(System.Single degree)
    {
       StartCoroutine(left_rot(degree));
    }
    public void right_rotation(System.Single degree)
    {
        StartCoroutine(right_rot(degree));
    }
    public void back_position(System.Single degree) //코루틴 변경 필
    {
        StartCoroutine(backPos(degree));
    }


    private void OnEnable()
    {
        Lua.RegisterFunction("left_rotation", this, SymbolExtensions.GetMethodInfo(() => left_rotation((int)0)));
        Lua.RegisterFunction("right_rotation", this, SymbolExtensions.GetMethodInfo(() => right_rotation((int)0)));
        Lua.RegisterFunction("back_position", this, SymbolExtensions.GetMethodInfo(() => back_position((int)0)));

    }
    private void OnDisable()
    {
        Lua.UnregisterFunction("left_rotation");
        Lua.UnregisterFunction("right_rotation");
        Lua.UnregisterFunction("back_position");

    }
}
