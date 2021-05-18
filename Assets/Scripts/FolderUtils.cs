using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public static class FolderUtils
{
    public static void ShowDelegateMethods(Delegate @delegate)
    {
        Delegate[] events = @delegate.GetInvocationList();
        Debug.Log("Events for " + @delegate.Method.Name);
        for (int i = 0; i < events.Length; i++)
        {
            Debug.Log("Event: " + events[i].Method.Name);
        }
    }
}
