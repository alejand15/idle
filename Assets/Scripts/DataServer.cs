using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;

[Serializable]
public class dataToSave {
    public String userName;
    public int totalCoins;
    public int totalDamage;
    public int currentLevel;
}
public class DataServer : MonoBehaviour
{
    public dataToSave dts;
    public string userId;
    DatabaseReference dbRef;

    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveDataFn() { }
    public void LoadDataFn() { }
}
