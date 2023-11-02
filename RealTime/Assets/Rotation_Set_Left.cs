using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Rotation_Set_Left : MonoBehaviour
{

    public void left_rotation(System.Single degree)
    {
        for (int i = 0; i < degree; i++)
        {
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(0, -0.1f, 0);
        }
    }
    public void right_rotation(System.Single degree)
    {
        for (int i = 0; i < degree; i++)
        {
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(0, 0.1f, 0);
        }
    }
    public void back_position(System.Single degree) //코루틴 변경 필
    {
        for (int i = 0; i < degree * 100; i++)
        {
            Vector3 pos = this.transform.position;
            pos.x += 0.0001f;
            this.transform.position = pos;
        }
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
