using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;


public class BossCtr02 : PlayerControl
{

    public GameObject BombPrefab;
    public GameObject BombCirclePrefab;
    public GameObject PushAtkEffect;
    public GameObject InvisibleAttack; 

    private bool isExcuteing; 
    private GameObject innerBombCircle;
    private GameObject outerBombCircle; 

    private GameObject rockMan;
    private GameObject hpBar;
    private GameObject collisionDamage;
    private Vector2 targetPoint;
    private float mAttackWaitTime = 0;
    private float mDashWaitTime = 0;
    private float mPrevVelocityY = 0;
    private SummonType summonObject;
    private List<GameObject> superBombList;
    private List<GameObject> bombList;
    private List<GameObject> warningList; 

    private enum SummonType
    {
        Shield,Bomb
    }

    void Start ()
    { 
        base.Start();
        setDirection(-1);
        mRigidBody = transform.GetComponent<Rigidbody2D>();
        airSpeed = 5;
        hpBar = GetComponent<HpControl>().getHpBar();
        hpBar.GetComponent<HpBar>().setValueShow(false);
        rockMan = GameObject.Find("Rockman");
        collisionDamage = transform.Find("CollisionDamage").gameObject;

        if (GlobalVariable.getInstance().CurrentLevel > 0)
        {
            onSummonInnerShield();
            onSummonOuterShield(); 
        }
        else
        {
            onSummonInnerShield(); 
        }

        superBombList = new List<GameObject>();
        warningList = new List<GameObject>();
        bombList = new List<GameObject>();
    }

    public override void initial()
    {
        Debug.Log("Inital");
        base.initial();
        hpBar.GetComponent<HpBar>().setValueShow(true);
        hpBar.GetComponent<HpBar>().setValue(0);
        hpBar.GetComponent<HpBar>().playRestoreEffect(() =>
        {
            IsReady = true; 
            Debug.Log("Ready");
        });
    }

   
	void Update ()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        move = 0;

        if (Input.GetKey("1")  )
        {
            GetComponent<HpControl>().modifyHp(-1);
        } 
    }
     

    public void onAttack()
    {
        Debug.Log("OnAttack");
        isAttacking = true;
        anim.SetBool("Attacking", true); 
    }

    public void onAttackFinish()
    {
        Debug.Log("OnAttackFinish");
        isAttacking = false;
        anim.SetBool("Attacking", false); 
    }

    public bool getIsAttacking()
    {
        return isAttacking;
    }
      
    public int getOuterShiledNumber()
    {
        if (outerBombCircle != null)
            return outerBombCircle.GetComponent<BombCircle>().getBombNumber();
        else
            return 0;
    }

    public int getInnerShieldNumber()
    {
        if (innerBombCircle != null)
            return innerBombCircle.GetComponent<BombCircle>().getBombNumber();
        else 
            return 0; 
    }

    public bool getIsExcuting()
    {
        return isExcuteing;
    }

    public int getSuperBombNumber()
    {
        return superBombList.Count;
    }

    void onPushAtk()
    {
        if (innerBombCircle != null)
        {  
            int direction = facingRight ? 1 : -1;
            float distance = Vector2.Distance(transform.position, rockMan.transform.position);
            float time= distance/1.3f;
            innerBombCircle.transform.DOPunchPosition(direction*new Vector3(distance, 0, 0), time,0,0);
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(3);
            mySequence.AppendCallback(() =>
            {
                onAttackFinish();
            }); 
        }
        else
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1);
            mySequence.AppendCallback(() =>
            {
                onAttackFinish();
            });
        }
    }

    void onKick()
    { 
        Vector2 pos = gameObject.transform.position;
        pos.y += 0.5f;
        GameObject atk =
            Instantiate(PushAtkEffect, new Vector2(pos.x - 0.8f, pos.y ), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.GetComponent<PushControl>().setDir(-1);
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1); // facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);

        atk = Instantiate(PushAtkEffect, new Vector2(pos.x + 0.8f, pos.y), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        //atk.GetComponent<PushControl>().setDir(facingRight ? 1 : -1);
        atk.transform.localScale = new Vector2(facingRight ? -1 : 1, 1); // facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
    }

    void onSummon()
    {
        switch (summonObject)
        {
            case SummonType.Shield: 
                 
                if (GlobalVariable.getInstance().CurrentLevel > 0)
                {
                    onSummonInnerShield();
                    onSummonOuterShield(); 
                }
                else
                {
                    onSummonInnerShield(); 
                }

                break;
            case SummonType.Bomb:
                //Sequence mySequence = DOTween.Sequence();
                //mySequence.SetLoops(UnityEngine.Random.Range(3, 8));
                //mySequence.AppendInterval(0.5f);
                //mySequence.PrependCallback(() =>
                //{
                //    onSummonBomb();
                //}); 
                int random = UnityEngine.Random.Range(4, 5);
                for (int i = 0; i < random; i++)
                {
                    onSummonBomb();
                }
                
                break;
        }
    }

    void onSummonInnerShield()
    {
        if (innerBombCircle != null)
        {
            Quaternion innerQuaternion = innerBombCircle.transform.rotation;
            Destroy(innerBombCircle);
            innerBombCircle = Instantiate(BombCirclePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y+0.5f), innerQuaternion) as GameObject;
        }
        else
        {
            innerBombCircle = Instantiate(BombCirclePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), Quaternion.identity) as GameObject;
        } 
        innerBombCircle.GetComponent<BombCircle>().setOwner(gameObject);
        innerBombCircle.GetComponent<BombCircle>().setIsReflect(true);
        innerBombCircle.GetComponent<BombCircle>().Damage = 3;

    }

    void onSummonOuterShield()
    {
        if (outerBombCircle != null)
        {
            Quaternion outerQuaternion = outerBombCircle.transform.rotation;
            Destroy(outerBombCircle);
            outerBombCircle = Instantiate(BombCirclePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), outerQuaternion) as GameObject;
        }
        else
        {
            outerBombCircle = Instantiate(BombCirclePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), Quaternion.identity) as GameObject;
        } 
        outerBombCircle.GetComponent<BombCircle>().setRadius(2);
        outerBombCircle.GetComponent<BombCircle>().Damage = 3;
        outerBombCircle.GetComponent<BombCircle>().setClockwise(true);
        outerBombCircle.GetComponent<BombCircle>().setOwner(gameObject);
        outerBombCircle.GetComponent<BombCircle>().setIsReflect(true); 
    }

    void onSummonBomb()
    { 
        var bomb = Instantiate(BombPrefab, new Vector2(4, UnityEngine.Random.Range(-0.5f,0.4f)), Quaternion.identity) as GameObject; 
        bombList.Add(bomb);
        bomb.transform.position = new Vector2(bomb.transform.position.x+6*(bombList.Count-1),bomb.transform.position.y);

        bomb.GetComponent<BombControl>().setDirection(new Vector2(facingRight ? 1 : -1 , 0));
        bomb.GetComponent<DamageControl>().setOwner(gameObject); 
        bomb.GetComponent<BombControl>().setDestoryAction(() => { bombList.Remove(bomb); });
        bomb.GetComponent<BombControl>().HP = 1;
    }

    public void onCreateInvisibleAttack()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.3f : -0.3f;
        //pos.y += 0.3f;
        GameObject atk = Instantiate(InvisibleAttack, pos, Quaternion.identity) as GameObject;
        //atk.transform.parent = gameObject.transform;
        atk.GetComponent<DamageControl>().setOwner(gameObject);
    }


    void onSuper()
    {
        Time.timeScale = 0;
        GameObject.Find("Background").GetComponent<BgScript>().showEffect();
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true);
        mySequence.AppendCallback(() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(
            Resources.Load("Sound/super") as AudioClip);
        });
        
        mySequence.AppendInterval(1.7f);
        mySequence.AppendCallback(() =>
        {
            GameObject.Find("Background").GetComponent<BgScript>().stopEffect(); 
            Time.timeScale = 1;  
            
        });
    }

    void onStopSuper()
    {
        summonSuperBomb();
    }

    void summonSuperBomb()
    {  
        foreach(GameObject obj in superBombList)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in warningList)
        {
            Destroy(obj);
        }

        for (int i = 0; i < 8; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            superBombList.Add(bomb);
            bomb.GetComponent<DamageControl>().setOwner(gameObject);
            bomb.GetComponent<DamageControl>().Damage = 5;
            bomb.transform.position = new Vector2(-3.5f+1*i,2.4f-0.77f/2);
            Vector2 targetPoint = new Vector2(UnityEngine.Random.Range(-3.7f, -1), -1.295f+0.77f/2);

            GameObject warning = Instantiate(Resources.Load("Prefab/StageScene/WarningEffect")) as GameObject;
            warningList.Add(warning);
            warning.SetActive(false);
            warning.transform.position = targetPoint;

            bomb.GetComponent<BombControl>().setDestoryAction(() =>
            {
                superBombList.Remove(bomb);
                warningList.Remove(warning);
                Destroy(warning);
            });

          
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1*i);
            mySequence.AppendCallback(() =>
            {
                warning.SetActive(true);
                warning.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                warning.GetComponent<SpriteRenderer>().DOFade(1, 1); 
            });
            mySequence.AppendInterval(1);
            mySequence.Append(bomb.transform.DOMove(targetPoint, 0.2f)
                .SetEase(Ease.InSine));
            mySequence.AppendCallback(()=>
            { 
                bomb.GetComponent<BombControl>().Land(); 

            });
              
        }
    }

    public void setShield(bool value)
    {
        if (value)
        { 
            isExcuteing = true;
            summonObject = SummonType.Shield;
            anim.SetTrigger("SummonTrigger");

            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1);
            mySequence.AppendCallback(() => { isExcuteing = false; }); 
        }
        else
        {
            anim.ResetTrigger("SummonTrigger");
        } 
    }

    public void setBomb()
    {
        isExcuteing = true;
        summonObject = SummonType.Bomb;
        anim.SetTrigger("SummonTrigger");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(1);
        mySequence.AppendCallback(() => { isExcuteing = false; });
    }

    public void setEvade()
    {
        isExcuteing = true;
        anim.SetTrigger("EvadeTrigger");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(1);
        mySequence.AppendCallback(() => { isExcuteing = false; });
    }

    public void setPunch()
    {
        isExcuteing = true;
        anim.SetTrigger("PunchTrigger"); 

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(1);
        mySequence.AppendCallback(() => { isExcuteing = false; });
    }

    public void setKick()
    {
        isExcuteing = true;
        anim.SetTrigger("KickTrigger");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(1);
        mySequence.AppendCallback(() => { isExcuteing = false; });
    }

    public void setSuper()
    {
        anim.SetTrigger("SuperTrigger");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(1);
        mySequence.AppendCallback(() => { isExcuteing = false; });
    }
}
