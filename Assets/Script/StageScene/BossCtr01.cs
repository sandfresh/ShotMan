using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BossCtr01 : PlayerControl
{
    public GameObject DashEffect;
    public GameObject DashAtkEffect;
    public GameObject PushAtkEffect;
    public GameObject BombPrefab;
     
    public float WalkSpeed = 0.6f;

    private GameObject player; 
    private GameObject hpBar;
    private List<GameObject> superBombList;

    private float attackWaitTime = 0;
    private float dashTime = 0;
    private float dashMaxTime;
    private bool isExcuteing;
    private bool isDashPrepare = false;
    private bool isAttack = false;
    private bool isIdle = false;
    private bool isDash = false;
    private bool isWalk = false;
    private bool isJump = false;
    private bool isSuper = false; 
    private Vector2 targetPoint;

    public override void initial()
    {
        print("Inital");
        base.initial(); 
        hpBar.GetComponent<HpBar>().setValueShow(true);
        hpBar.GetComponent<HpBar>().setValue(0);
        hpBar.GetComponent<HpBar>().playRestoreEffect(() =>
        {
            IsReady = true;
            print("Ready");
        });
    }

    private void Start()
    {
        base.Start();
        setDirection(-1);
        airSpeed = 5;
        mRigidBody = transform.GetComponent<Rigidbody2D>();  
        hpBar = GetComponent<HpControl>().getHpBar();
        hpBar.GetComponent<HpBar>().setValueShow(false);
        player = GameObject.Find("Rockman");
        superBombList = new List<GameObject>();
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded); 
      

        if (isWalk)
        {
            mRigidBody.velocity = facingRight ? new Vector2(WalkSpeed, mRigidBody.velocity.y) : new Vector2(-WalkSpeed, mRigidBody.velocity.y);
        } 

        if (Input.GetKey("1"))
        {
            GetComponent<HpControl>().modifyHp(-1);
        }

        if (isDash)
        {  
            if (Vector2.Distance(player.transform.position, (Vector2)transform.position) < 1)
            {
                if(isJump)
                    print("Jumping Stop becuase dash not clear");
                setKnife(true);
                stopDash();
            }
            else if (Mathf.Abs(mRigidBody.velocity.x) < 0.1f)
            {
                stopDash();
            }
            else
            {
                dashTime += Time.deltaTime;
                if (dashTime > dashMaxTime)
                {
                    dashTime = 0;
                    stopDash();
                }
            }
        } 

        if (grounded)
        {
            if (isJump && !isExcuteing)
            {
                isJump = false;
            }
        }
        anim.SetInteger("Hp", GetComponent<HpControl>().hp);
        anim.SetFloat("Speed", Mathf.Abs(mRigidBody.velocity.x));
        anim.SetFloat("vSpeed", Mathf.Abs(mRigidBody.velocity.y));

    }

    public bool getIsGround()
    {
        return grounded;
    }

    public bool getIsWalking()
    {
        return isWalk;
    }

    public bool getIsExcuting()
    {
        return isExcuteing;
    } 

    public void setWalk(bool value)
    {
        isWalk = value; 

        if (value)
        {
            Action action = () => { isWalk = false; };
            waitExcute(Random.Range(2, 4), action);
        }
        else
        {
            mRigidBody.velocity = new Vector2(0,mRigidBody.velocity.y);
        }
    }

    public void setJump(bool value)
    {
        isJump = value; 

        if (value)
        {
            int vectorX = facingRight ? 150 : -150;  
            int vectorY = 550;  
            Vector2 force = new Vector2(vectorX, vectorY);  
            mRigidBody.AddForce(force*Time.fixedDeltaTime, ForceMode2D.Impulse);
            waitExcute(1.2f);
        } 
    }

    public void setKnife(bool value)
    {
        if (value)
        {
            anim.SetTrigger("Knife"); 
            DOTween.Kill("knife"); 
            waitExcute(0.6f);
        } 
    }

    public void setTargetPoint(Vector2 point)
    {
        targetPoint = point;
    }

    public void stopDash()
    {  
        print("Stop dash");
        isDash = false;
        specilaMove = false;
        mRigidBody.velocity = new Vector2(0, 0);
    }

    public void onAttack()
    {
        anim.SetBool("Attacking", true); 
    }

    public void onAttackFinish()
    { 
        isAttacking = false;
        anim.SetBool("Attacking", false); 
        mRigidBody.velocity = Vector2.zero;
    }  

    public void onDash()
    { 
        specilaMove = true; 
        isDash = true;
        attackWaitTime = 0;
        dashTime = 0;
        dashMaxTime = Random.Range(1.8f, 2.2f); 
      
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        GameObject atk = Instantiate(DashEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);

        Vector2 force = new Vector2(500, 0);
        if (facingRight)
            mRigidBody.AddForce(force * Time.fixedDeltaTime, ForceMode2D.Impulse);
        else
            mRigidBody.AddForce(-force * Time.fixedDeltaTime, ForceMode2D.Impulse);

        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/dash") as AudioClip);
    }

    public void knifeAtk()
    {
        isAttacking = true;
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        pos.y += 0.4f;
        GameObject atk = Instantiate(DashAtkEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2( 1, 1); 
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<DamageControl>().Damage = 1;

        waitExcute(0.6f);
    }

    public void dashAtk()
    {
        isAttacking = true;
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        pos.y += 0.4f;
        GameObject atk = Instantiate(DashAtkEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<DamageControl>().Damage = 3;

        waitExcute(0.6f);
    }

    public void pushAtk()
    {
        isAttacking = true;
        Vector2 pos = gameObject.transform.position;
        pos.y += 0.5f;
        GameObject atk =
            Instantiate(PushAtkEffect, new Vector2(pos.x - 0.8f, pos.y + 0.5f), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.GetComponent<PushControl>().setDir(-1);
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);  
        atk.GetComponent<DamageControl>().setOwner(gameObject);

        atk = Instantiate(PushAtkEffect, new Vector2(pos.x + 0.8f, pos.y + 0.5f), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform; 
        atk.transform.localScale = new Vector2(facingRight ? -1 : 1, 1);  
        atk.GetComponent<DamageControl>().setOwner(gameObject); 
    }

    public void setDashPrepare(bool value)
    {
        mRigidBody.velocity = Vector2.zero;
        if (value)
        {
            if (isDashPrepare == false)
            { 
                isExcuteing = true;
                isDashPrepare = true;
                anim.SetBool("DashPrepare",true); 
            } 
        }
        else
        {
            anim.SetBool("DashPrepare", false);
            isDashPrepare = false;
            isDash = false;
        }
    }

    public void onDashPrepare()
    { 
        isExcuteing = true;
        mRigidBody.velocity = Vector2.zero;
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(Random.Range(0.3f, 0.5f));
        mySequence.AppendCallback(() =>
        {
            isExcuteing = false;
            isDashPrepare = false;
            
            anim.SetTrigger("DashTrigger");
            anim.SetBool("DashPrepare", false);
        }); 
    }

    public bool getIsDashPrepare()
    {
        return isDashPrepare;
    }

    public bool getIsJumping()
    {
        return isJump;
    }

    public bool getIsDash()
    {
        return isDash;
    }

    public bool getIsSuper()
    {
        return isSuper;
    }

    public int getSuperBombNumber()
    {
        return superBombList.Count;
    }

    public void setPush()
    { 
        anim.SetTrigger("Push");

        waitExcute(2);
    }
 
    public override void setDown()
    { 
        mRigidBody.velocity = new Vector2(0,0);
        DOTween.Kill("dash");

        base.setDown();
    }

    public void setSuper()
    {
        anim.SetTrigger("SuperTrigger");
         
        isSuper = true;
        waitExcute(2);
    }

    public void onSuper()
    {
        Time.timeScale = 0; 
        GameObject.Find("Background").GetComponent<BgScript>().showEffect();
        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Super") as AudioClip);
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true);
        mySequence.AppendInterval(1.7f);
        mySequence.AppendCallback(() =>
        { 
            GameObject.Find("Background").GetComponent<BgScript>().stopEffect();
            Time.timeScale = 1; 
        });
    }

    public void onSuperFinish()
    {
        print("onSuperFinish");
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.1f);
        mySequence.AppendCallback(() =>
        {
            useSuper();
            isSuper = false;
        }); 
    }

    public void useSuper()
    {
        foreach (GameObject obj in superBombList)
        {
            Destroy(obj);
        }
          
        for (int i = 0; i < 10; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            superBombList.Add(bomb);
            bomb.GetComponent<NatrueBomb>().setOwner(gameObject);
            bomb.transform.position = new Vector2(-3.5f + 0.5f * i, 2.4f+0.77f/2);
            Vector2 targetPoint = new Vector2(UnityEngine.Random.Range(-3.5f, 3.5f), -1.295f + 0.77f / 2); 

            bomb.GetComponent<NatrueBomb>().setDestoryAction(() =>
            {
                superBombList.Remove(bomb);  
            }); 

            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(0.3f * (i+1));  
            mySequence.Append(bomb.transform.DOMove(targetPoint, 0.3f).SetEase(Ease.Linear));
            mySequence.AppendInterval(1);
            mySequence.AppendCallback(() =>
            {
                bomb.GetComponent<NatrueBomb>().setExplode(); 
            });

        }
    }

    public void waitExcute(float time,Action action = null)
    {
        isExcuteing = true;
        DOTween.Complete("excute");
        Sequence mySequence = DOTween.Sequence();
        mySequence.id = "excure";
        mySequence.AppendInterval(time);
        mySequence.AppendCallback(() =>
        {
            isExcuteing = false;
            if (action != null)
                action();
        });
    }
}