using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    Action action;

    private void Start()
    {
        action += MicroPhoneTest;
        Request(Permission.Microphone,action);
    }

    public void Request(string targetPermission, Action granrtedAction)
    {
        if (Permission.HasUserAuthorizedPermission(targetPermission))
        {
            granrtedAction();
        }
        else
        {
            var callback = new PermissionCallbacks();

            callback.PermissionGranted += _ => granrtedAction();

            Permission.RequestUserPermission(targetPermission, callback);
        }

    }

    public void MicroPhoneTest()
    {
        Debug.Log("마이크 권한 요청");
    }


}
