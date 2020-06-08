using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string email;
    public string username;
    public string sessionID;
    public bool isLocalPlayer;
    public GameLocation location;

    public void SetValues(params ValueTuple<string, object>[] prop) {
        Type sourceType = GetType();

        foreach(var p in prop) {
            var targetObj = sourceType.GetField(p.Item1);
            if(targetObj == null)
                throw new Exception("Thats bad");

            targetObj.SetValue(this, p.Item2);
        }
    }
}
