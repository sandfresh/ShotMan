using UnityEngine;
using System.Collections;

public class DamageControl : MonoBehaviour 
{
    public int damage = 1;
    GameObject mOwner;

    public void setOwner(GameObject owner)
    {
        mOwner = owner;
    }

    public int Damage
    {
        get
        {
            if (mOwner.name == "Rockman")
            {  
                GlobalVariable.getInstance().HitNumber = GlobalVariable.getInstance().HitNumber + 1;
                GlobalVariable.getInstance().Combo = GlobalVariable.getInstance().Combo + 1;
                if (GlobalVariable.getInstance().Combo > GlobalVariable.getInstance().MaxCombo)
                    GlobalVariable.getInstance().MaxCombo = GlobalVariable.getInstance().Combo;

                var stageUI = GameObject.Find("StageUI").GetComponent<StageUI>();
                stageUI.PlayerCombo = stageUI.PlayerCombo + 1;
            }
            else
            { 
                var stageUI = GameObject.Find("StageUI").GetComponent<StageUI>();
                stageUI.BossCombo = stageUI.BossCombo + 1;
            }

            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public GameObject Owner
    {
        get
        {
            return mOwner;
        }
    }
}