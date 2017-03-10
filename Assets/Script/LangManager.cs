using UnityEngine;
using System.Collections;
using System.IO;
 

public class LangManager 
{ 

    private Lang LMan;
    static private LangManager instance;
     

    private LangManager()
    {  
        TextAsset t = (TextAsset)Resources.Load("Value/langs");
        LMan = new Lang(t.ToString(), "zh-TW", true);
    }

    static public LangManager Instance
    {
        get
        {
            if(instance == null)
                instance = new LangManager();
            return instance;
        }
    }

    public string getString(string key)
    {
        return LMan.getString(key);
    }
     
}
