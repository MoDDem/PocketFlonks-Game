using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SessionScript : MonoBehaviour
{
    public static SessionScript instance;
    public static string destination;

    private void Awake() {
        destination = Path.Combine(Application.persistentDataPath, ".session");
        Debug.LogWarning(destination);

        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
    }

    public static void SaveSession(string sessionID) {
        byte[] sessionBytes = Convert.FromBase64String(sessionID);

        FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
        file.Write(sessionBytes, 0, sessionBytes.Length);
        file.Close();
    }

    public static string LoadSession() {
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else return null;

        byte[] sessionBytes = new byte[file.Length];
        file.Read(sessionBytes, 0, sessionBytes.Length);
        string sessionID = Convert.ToBase64String(sessionBytes);
        file.Close();

        return sessionID;
    }

    public static void DeleteSession() { if(File.Exists(destination)) File.Delete(destination); }
}
