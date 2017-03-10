using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerControl))]
public class HpControl : MonoBehaviour 
{
    public int hp = 28;
    public int maxHp = 28;
 //   public GameObject mDamageEffect;
    public GameObject DamgaeEffect;
    public GameObject HPBar;

    private PlayerControl playerCtrl;
    private StageManager stageMgr;
    private StageUI stageUI; 

    void Start()
    {
        playerCtrl = GetComponent<PlayerControl>();
        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageUI = GameObject.Find("StageUI").GetComponent<StageUI>(); 
    }

    public void modifyHp(int value)
    {
        if (stageMgr.GameStart == false)
            return;
        hp += value;
        if (hp <= 0)
            hp = 0;
        if (value < 0)
        {
            playerCtrl.setDamage();

            if (gameObject.name == "Rockman")
            {
                GlobalVariable.getInstance().Hp = HP;
                GlobalVariable.getInstance().Combo = 0;

                stageUI.PlayerCombo = 0;
            }
            else
            {
                stageUI.BossCombo = 0;
            }
        }
         
        if (hp <= 0)
        {
            playerCtrl.setDown();
            stageMgr.StopGame();

            if (gameObject.name == "Rockman")
                AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rockDie") as AudioClip);
            
        }
        else
        {
            if (gameObject.name == "Rockman")
                AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rockHurt") as AudioClip);
            else
                AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/BossHit") as AudioClip);
        }
  

        HPBar.GetComponent<HpBar>().setValue(hp);
    }

    public GameObject getDamageEffect()
    {
        return DamgaeEffect;
    }

    public GameObject getHpBar()
    {
        return HPBar;
    }

    public int getMaxHp()
    {
        return maxHp;
    }

    public int HP
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
}