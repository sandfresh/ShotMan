using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BossCtr03  : PlayerControl
{

    public KeyCode downKey;
    public KeyCode pushKey;
    public KeyCode dashKey;

    public GameObject mDashEffect;
    public GameObject mKickEffect;
    public GameObject mClawEffect;
    public GameObject mShockEffect;
    public GameObject mWaveEffect;
    public GameObject mInvisibleAttack;
    public GameObject SupperEffect;

    public float WalkSpeed = 1f;

    bool mIsDashing = false;
    bool mIsCrounch = false;
    bool mIsJump = false;
    bool isChange = false;
    bool mIsDashTrigger = false; 

    private bool isWalk = false;
    private bool isJump = false;
    private bool isKick = false;
    private bool isDashPrepare = false;
    private bool isDash = false;
    private bool isCrounch = false;
    private bool isPunch = false;
    private bool isAirkick = false;
    private bool isSetChange = false;
    private bool isWolfAttack = false;
    private bool isExcuteing;
    private bool isSetAirKick = false;
    private bool isAirKick = false;

    private GameObject rockMan;
    private GameObject hpBar;
    private GameObject collisionDamage; 
    private float mAttackWaitTime = 0; 
    private float dashTime =  0;
    private float mPrevVelocityY = 0; 
    private int changeCounter = 0;
    private float dashMaxTime = 0;


    public override void initial()
    {
        print("Inital");
        base.initial();
        hpBar.GetComponent<HpBar>().setValueShow(true);
        hpBar.GetComponent<HpBar>().setValue(0);
        hpBar.GetComponent<HpBar>().playRestoreEffect(() =>
        {
            print("Ready");
            IsReady = true;
        });
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
    } 
	 
	void Update ()
	{ 
        if (Input.GetKey("1"))
        {
            GetComponent<HpControl>().modifyHp(-1);
        }

        float tempSpeed = maxSpeed;
	    if (!anim.GetBool("Crounch"))
	    {
            mIsCrounch = false;
        }
            
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        move = 0;

        if (isDash)
        {
            if (Vector2.Distance(rockMan.transform.position, (Vector2)transform.position) < 1)
            {
                setKick(true); 
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

        if (move != 0 && mIsDashTrigger)
            mIsDashTrigger = false;

        if (isWalk)
        {
            move = facingRight ? 1 : -1;
            tempSpeed = WalkSpeed;
        }

        if (!grounded)
        {
            move = facingRight ? 1 : -1;
            if (gameObject.transform.position.y > 0.5f && isAirKick == false && isSetAirKick == false)
            { 
                float distance = Vector2.Distance(rockMan.transform.position, gameObject.transform.position);
                if (distance > 2)
                {
                    isSetAirKick = true;
                    anim.SetTrigger("KickTrigger");
                    print("AirKick");
                }
            } 

            if ((Mathf.Abs(mPrevVelocityY) > 0.1f) && (Mathf.Abs(mRigidBody.velocity.y) <= 0.1f))
                isJump = false;
        }
        if (grounded)
        {
            mIsJump = false;
            if (isAirKick)
            {
                // 飛踢著地
                print("AirKick Landed");
                isAirKick = false;
                mRigidBody.gravityScale = 1;
                mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 2, mRigidBody.velocity.y);
                GameObject.FindWithTag("MainCamera").GetComponent<Camera>().DOShakePosition(0.5f, new Vector2(1f, 1f), 8);

                onCreateAirKickEffect();
                AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Wall") as AudioClip);
            }

            if (isWolfAttack)
            {
                isWolfAttack = false;
                mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 2, mRigidBody.velocity.y);
                GameObject.FindWithTag("MainCamera").GetComponent<Camera>().DOShakePosition(0.2f, new Vector2(1f, 1f), 8);

                onCreateAirKickEffect();
                AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Wall") as AudioClip);
            }
        }

        if (!isAirKick && mIsDashing && (Input.GetKeyUp(left) || Input.GetKeyUp(right)))
        {
            Invoke("stopDash", 0.1f);
            mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 5, mRigidBody.velocity.y);
        }

        if (!damaged && !specilaMove && !isAttacking)
        {
            //if (move > 0 && !facingRight)
            //    Flip();
            //else if (move < 0 && facingRight)
            //    Flip();

            //if (!grounded)
            //    tempSpeed = airSpeed; 

            //if (!mIsCrounch || mIsJump)
            //    mRigidBody.velocity = new Vector2(move * tempSpeed, mRigidBody.velocity.y);

            if (!isCrounch || isJump)
            {
                mRigidBody.velocity = new Vector2(move * tempSpeed, mRigidBody.velocity.y);
            }
                
            if (isAirKick)
            {
                print("AirKickSpeed:" + move * tempSpeed);
            }
        }


        mPrevVelocityY = mRigidBody.velocity.y; 
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", mRigidBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(mRigidBody.velocity.x));
        anim.SetFloat("y", mRigidBody.position.y);
        anim.SetInteger("Hp", GetComponent<HpControl>().hp);
    } 

    public void stopDash()
    {
        isDash = false;
        specilaMove = false;
        mRigidBody.velocity = new Vector2(0, 0); 
    }

    public void onAttack()
    { 
        isAttacking = true; 
        mAttackWaitTime = 0;
    }

    public void onAttackFinish()
    {
        isAttacking = false;
        anim.SetBool("Attacking", false);
        mRigidBody.velocity = Vector2.zero; 
    }

    public void onCrounch()
    {
        mIsCrounch = true;
        isCrounch = true; 
    }

    public void onStandUp()
    {
        mIsCrounch = false;
        isCrounch = false;
    }

    public void onDash()
    { 
        print("OnDash");
        isDash = true;
        specilaMove = true;
        mIsDashing = true;
        dashMaxTime = Random.Range(1.8f, 2.2f);
        anim.SetBool("Attacking", false); // 避免預先按下攻擊
        anim.ResetTrigger("KickTrigger"); // 避免預先按下攻擊
        anim.ResetTrigger("PunchTrigger"); // 避免預先按下攻擊
          

        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        GameObject atk = Instantiate(mDashEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);

        Vector2 force = new Vector2(500, 0);
        if (facingRight)
            mRigidBody.AddForce(force * Time.fixedDeltaTime , ForceMode2D.Impulse);
        else
            mRigidBody.AddForce(-force * Time.fixedDeltaTime , ForceMode2D.Impulse);

        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/dash") as AudioClip);
    }

    public void onCreateKickEffect()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        pos.y += 0.4f;
        GameObject atk = Instantiate(mKickEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<PushControl>().setDir(facingRight ? 1 : -1);

        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/missAtk") as AudioClip);
    }

    public void onCreateClawEffect()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.9f : -0.9f;
        pos.y += 0.2f;
        GameObject atk = Instantiate(mClawEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);

        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/missAtk") as AudioClip);
    }

    public void onCreateWaveEffect()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.3f : -0.3f;
        pos.y += 0.3f;
        GameObject atk = Instantiate(mWaveEffect, pos, Quaternion.identity) as GameObject;
        //atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<ProjectileCtrl>().setDirection(facingRight ? 1 : -1);

        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/missAtk") as AudioClip);
    }

    public void onCreateInvisibleAttack()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.3f : -0.3f;
        //pos.y += 0.3f;
        GameObject atk = Instantiate(mInvisibleAttack, pos, Quaternion.identity) as GameObject;
        //atk.transform.parent = gameObject.transform;
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<InvisibleControl>().DestoryTime = 0.3F;
    }

    public void onCreateAirKickEffect()
    {
        Vector2 pos = gameObject.transform.position;
        //pos.x += facingRight ? 0.9f : -0.9f;
        //pos.y += 0.2f;
        GameObject atk = Instantiate(mShockEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        
    }
   
    public void onAirKick()
    {
        print("OnAirKick");
        if (rockMan.transform.position.x - mRigidBody.transform.position.x > 0)
        {
            // right
            if (!facingRight)
                Flip();
        }
        else
        {
            // left
            if (facingRight)
                Flip();
        }

        isAirKick = true;
        mRigidBody.gravityScale = 0;
        mRigidBody.velocity = new Vector2(0, 0);

        Invoke("startAirKick", 0.4f);
    }

    public void startAirKick()
    {
        print("StartAirKick");
        Vector2 dir = new Vector3(rockMan.transform.position.x, -1.295f,0) - mRigidBody.transform.position;

        float spd = dir.magnitude * 3.5f;
        if (spd < 15)
            spd = 15;
        mRigidBody.velocity = dir.normalized * spd;
        //AddForce(rockMan.transform.position.normalized * 500);
    }
     
    public void onWolfAttack()
    { 
        isWolfAttack = true;
    }  

    public bool getIsAttacking()
    {
        return isAttacking;
    }

    public bool getIsExcuting()
    {
        return isExcuteing;
    }

    public bool getIsSetChange()
    {
        return isSetChange;
    } 
      
    public void setWalk(bool value)
    {
        isWalk = value;

        if (value)
        {
            Action action = () => { isWalk = false; };
            waitExcute(Random.Range(2,4),action);
        } 
    }


    public void setCrounch(bool value)
    {
        anim.SetBool("Crounch",value); 
        if (value)
        {
            waitExcute(0.5f);
        }
    }

    public void setPunch(bool value)
    {
        if (value)  
            anim.SetTrigger("PunchTrigger");  
        else
            anim.ResetTrigger("PunchTrigger"); 

        if (value)
        { 
            if (isSetChange)
            {
                if (rockMan.GetComponent<HpControl>().HP > 0)
                {
                    if (rockMan.transform.position.x - mRigidBody.transform.position.x > 0)
                    {
                        // right
                        if (!facingRight)
                            Flip();
                    }
                    else
                    {
                        // left
                        if (facingRight)
                            Flip();
                    }
                }
                else
                {
                    Flip();
                }
           
                float forceX = facingRight ? 550 : -550;
                mRigidBody.velocity = new Vector2(0, 0);
                mRigidBody.AddForce(new Vector2(forceX, 350), ForceMode2D.Force);
            }

           waitExcute(0.7f);
        }
    }

    public void setKick(bool value)
    {  
        if (value)
        {
            anim.SetTrigger("KickTrigger");
            waitExcute(0.4f);
        }
        else
        {
            anim.ResetTrigger("KickTrigger");
        }
    }

    public void setDashPrepare(bool value)
    {   
        if (value)
        {
            mRigidBody.velocity = Vector2.zero;
            if (isDashPrepare == false)
            { 
                isExcuteing = true;         //  wait until dashprepare
                isDashPrepare = true; 
                anim.SetBool("DashPrepare", true);
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
        mRigidBody.velocity = Vector2.zero;
        Action action = () =>
        {
            isDashPrepare = false;

            anim.SetBool("DashPrepare", false);
            anim.SetTrigger("DashTrigger"); 
        };
        waitExcute(Random.Range(0.3f, 0.5f), action); 
    }


    public void setChange(bool value)
    {
        if (value)
        { 
            Time.timeScale = 0;
            isExcuteing = true;
            isSetChange = true;
            GameObject.Find("Background").GetComponent<BgScript>().showEffect();
            Sequence mySequence = DOTween.Sequence();
            mySequence.SetUpdate(true);
            mySequence.AppendInterval(1.7f);
            mySequence.AppendCallback(() =>
            { 
                GameObject.Find("Background").GetComponent<BgScript>().stopEffect();

                Time.timeScale = 1;
                isExcuteing = false;
                specilaMove = true;
                
                anim.SetBool("Change", true); 
            });
        }
        else
        {
            specilaMove = false;
            isSetChange = false;
            anim.SetBool("Change", false);
        } 
    } 

    public void setJump(bool value)
    {
        isJump = value;
        
        if (value)
        {  
            print("Jump");
            int vectorX = facingRight ? 150 : -150; //Random.Range(150, 200);
            int vectorY = 550; 
            Vector2 force = new Vector2(vectorX, vectorY);
        //     mRigidBody.AddForce(force, ForceMode2D.Force);
            mRigidBody.AddForce(force * Time.fixedDeltaTime , ForceMode2D.Impulse); 

            isSetAirKick = false; 
            waitExcute(1.2f); 
        } 
    }
     
    public bool getIsGround()
    {
        return grounded;
    } 

    public bool getIsCrouching()
    {
        return isCrounch;
    }

    public bool getIsJumping()
    {
        return isJump;
    }

    public bool getIsWalking()
    {
        return isWalk; 
    }

    public bool getIsChanging()
    {
        return isChange;
    }

    public void OnChange()
    {
        Instantiate(SupperEffect, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f), Quaternion.identity);
    }

    public void OnSuper()
    { 
        isChange = true;
        GetComponent<DamageControl>().Damage = 5;
    }

    public void OnSuperStop()
    {
        isChange = false;
        GetComponent<DamageControl>().Damage = 1;
    }

    public void startSupperAttack()
    {
        changeCounter = 0; 
        StartCoroutine(SupperAttack()); 
    }

    public IEnumerator SupperAttack()
    {
        yield return new WaitForSeconds(0.5f);

        while (changeCounter < 4)
            {  
                {
                    float delaytime = 0.8f; 
                    setPunch(true);
                    yield return new WaitForSeconds(delaytime);
                    changeCounter++;
                } 
            }  

        if (changeCounter >= 4)
            setChange(false);
    }

    public override void setWin()
    {
        base.setWin();
        setCrounch(false);  
    }

    public void waitExcute(float time,Action action = null)
    {
        isExcuteing = true;
        DOTween.Complete("excute");
        Sequence mySequence = DOTween.Sequence();
        mySequence.id = "excute";
        mySequence.AppendInterval(time);
        mySequence.AppendCallback(() =>
        {
            isExcuteing = false;
            if(action != null)
                 action();
        });
    }
}

