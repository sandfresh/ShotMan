using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveMgr
{

    private static SaveMgr instance;


    public static SaveMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveMgr();
            }
            return instance;
        }
    }

    private Dictionary<string, SaveData> savedataLsit;

    private SaveMgr()
    {
        savedataLsit = new Dictionary<string, SaveData>();
    }

    public SaveData getSaveFille(string value)
    {
        if (!savedataLsit.ContainsKey(value))
        {
            SaveData data = null;  
            try
            {
                data = SaveData.LoadFromStreamingAssets(value+SaveData.extension);
            }
            catch (Exception)
            {
                data = new SaveData(value); 
            } 

            savedataLsit.Add(value, data);
        }

        return savedataLsit[value];
    }
}
