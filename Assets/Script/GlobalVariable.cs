using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GlobalVariable  
{

    public static GlobalVariable getInstance()
    {
        if (instance == null)
        {
            instance = new GlobalVariable();
        }
        return instance;
    }

    private static GlobalVariable instance;

    private const int TOTALSTAGE = 3;
    private int currentStage = 2;
    private int currentLevel = 2;
    private int currentPage = 0;

    private int hp = 28;
    private int hitNumber = 0;
    private int shotNumber = 0;
    private int combo = 0;
    private int maxcombo = 0;
    private float gameTime = 60;

    Dictionary<int, int> stageProgress ; 

    private SaveData saveData;


    private GlobalVariable()
    {
        stageProgress = new Dictionary<int, int>(); 
        saveData = SaveMgr.Instance.getSaveFille(Save.FileName.GameSave);
        string progressString = saveData.GetValue<string>(Save.GameSave.Progress);
        if (progressString == null)
        {
            stageProgress.Add(0, 0);
            stageProgress.Add(1, 0);
            stageProgress.Add(2, 0);
        }
        else
        {
            stageProgress = JsonConvert.DeserializeObject<Dictionary<int, int>>(progressString);
        }  
    }
     

    public int getTotalStage()
    {
        return TOTALSTAGE;
    }

    public int getStageProgress(int stage)
    {
        return stageProgress[stage];
    }

    public void modifyStageProgress(int stage,int value)
    {
        if((stageProgress[stage] + value) <= 2)
        {
          stageProgress[stage] = stageProgress[stage]+value;
          save();
        }
    }

    public void setCurrentStage(int value)
    {
        currentStage = value;
    }

    public int getCurrentStage()
    {
        return currentStage; 
    }

    public int getCurrentPage()
    {
        return currentPage;
    }

    public void setCurrentPage(int value)
    {
        currentPage = value;
    }
      

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }

        set
        {
            currentLevel = value;
        }
    }

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public int HitNumber
    {
        get
        {
            return hitNumber;
        }

        set
        {
            hitNumber = value;
        }
    }

    public int ShotNumber
    {
        get
        {
            return shotNumber;
        }

        set
        {
            shotNumber = value;
        }
    }

    public float GameTime
    {
        get
        {
            return gameTime;
        }

        set
        {
            gameTime = value; 
        }
    }

    public int Combo
    {
        get
        {
            return combo;
        }

        set
        {
            combo = value; 
        }
    }

    public int MaxCombo
    {
        get
        {
            return maxcombo;
        }

        set
        {
            maxcombo = value;
        }
    }

    public void save()
    {
       string converResult =  JsonConvert.SerializeObject(stageProgress);
       saveData[Save.GameSave.Progress] = converResult;
       saveData.Save();
    }
}
