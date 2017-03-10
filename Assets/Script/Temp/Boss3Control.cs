using UnityEngine;
using System.Collections;

public class Boss3Control : PlayerControl
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

    float mAttackWaitTime = 0;

    GameObject rockMan;

	// Use this for initialization
	new void Start () 
	{
        base.Start();
        anim = GetComponent<Animator>();
        airSpeed = 5; 
        rockMan = GameObject.Find("Rockman");
    }
	
    void FixedUpdate()
    {
        
    }

    bool mIsDashing = false;
    bool mIsCrounch = false;
    bool mIsJump = false;
    bool mIsDashTrigger = false;
    // Update is called once per frame
    void Update()
    {
        float tempSpeed = maxSpeed;
        if (!anim.GetBool("Crounch"))
            mIsCrounch = false;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", mRigidBody.velocity.y);

        move = 0;
        if (Input.GetKey(left))
            move = -1;
        else if (Input.GetKey(right))
            move = 1;

        //move = Input.GetAxis("Horizontal");

        //anim.SetFloat("Speed", Mathf.Abs(move));

        if (move < 0)
            move = -1;
        else if (move > 0)
            move = 1;

        if (move != 0 && mIsDashTrigger)
            mIsDashTrigger = false;

        if (grounded)
        {
            mIsJump = false;
            if (mIsAirKick)
            {             
                // 飛踢著地
                mIsAirKick = false;
                mRigidBody.gravityScale = 1;
                mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 2, mRigidBody.velocity.y);
                onCreateAirKickEffect();
            }
        }

        if (!mIsAirKick && mIsDashing && (Input.GetKeyUp(left) || Input.GetKeyUp(right)))
        {
            Invoke("stopDash", 0.1f);
            mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 5, mRigidBody.velocity.y);
        }

        if (!damaged && !specilaMove && !isAttacking)
        {
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();

            if (Input.GetKeyDown(up))
            {
                if (mIsCrounch && grounded)
                {
                    anim.SetBool("Ground", false);
                    mRigidBody.AddForce(new Vector2(0, jumpForce));
                    mIsJump = true;
                    mIsCrounch = false;
                }
                else if (!mIsDashTrigger)
                {
                    mIsDashTrigger = true;
                    anim.SetTrigger("DashTrigger");
                    mRigidBody.velocity = new Vector2(0, 0);
                }
            }
            else if (Input.GetKeyUp(up))
            {
                if (!grounded)
                {
                    Vector2 velocity = new Vector2(mRigidBody.velocity.x, mRigidBody.velocity.y / 3);
                    mRigidBody.velocity = velocity;
                    mIsJump = false;
                }
            }

            if (!grounded)
                tempSpeed = airSpeed;

            Debug.Log("attack = " + isAttacking);
            if (!mIsCrounch || mIsJump)
                mRigidBody.velocity = new Vector2(move * tempSpeed, mRigidBody.velocity.y);
        }

        //Debug.Log("move = " + move + ", crouch = " + mIsCrounch + ", v = " + mRigidBody.velocity.y);

        if (!isAttacking)
        {
            if (Input.GetKeyDown(attack))
            {
                isAttacking = true;
                //Debug.Log("num = " + Random.Range(0, 200));
                anim.SetBool("Attacking", true);

                float dist = (rockMan.transform.position - gameObject.transform.position).magnitude;
                //Debug.Log("Dist = " + dist);
                if (dist < 1.5f)
                //if (Random.Range(0, 21) <= 10)
                {
                    anim.SetTrigger("PunchTrigger");
                    //Debug.Log("Punch");
                }
                else
                {
                    anim.SetTrigger("KickTrigger");
                    //Debug.Log("Kick");
                }

                mAttackWaitTime = 0;

                if (mIsDashing)
                    Invoke("stopDash", 0.1f);
            }
            else if (Input.GetKeyDown(downKey))
            {
                if (!mIsCrounch)
                {
                    if (!mIsDashTrigger)
                    {
                        //mIsCrounch = true;
                        anim.SetBool("Crounch", true);
                        mRigidBody.velocity = new Vector2(0, 0);
                    }
                }
                else
                {
                    //mIsCrounch = false;
                    anim.SetBool("Crounch", false);
                }
            }
            else if (Input.GetKeyDown(pushKey))
            {
                //anim.SetBool("Attacking", true);
                anim.SetTrigger("Push");
                mAttackWaitTime = 0;
            }
        }
        else
        {
            //mRigidBody.velocity = new Vector2(0, mRigidBody.velocity.y);
            mAttackWaitTime += Time.deltaTime;
            if (mAttackWaitTime > 0.2f && !mIsAirKick)
            {
                anim.SetBool("Attacking", false);
                isAttacking = false;
                Debug.Log("Atk finish2");
            }
        }

        anim.SetFloat("Speed", Mathf.Abs(mRigidBody.velocity.x));
        anim.SetFloat("y", mRigidBody.position.y);
    }

    public void stopDash()
    {
        specilaMove = false;
        mRigidBody.velocity = new Vector2(0, 0);
        //anim.SetBool("Dash", false);
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
        Debug.Log("Atk finish");
    }

    public void onCroFunch()
    {
        mIsCrounch = true;
        Debug.Log("Crouch = " + mIsCrounch);
    }

    public void onStandUp()
    {
        mIsCrounch = false;
    }

    public void onDash()
    {
        specilaMove = true;
        mIsDashing = true;
        mRigidBody.AddForce(new Vector2(move * 500, 0));
        anim.SetBool("Attacking", false); // 避免預先按下攻擊
        anim.ResetTrigger("KickTrigger"); // 避免預先按下攻擊
        anim.ResetTrigger("PunchTrigger"); // 避免預先按下攻擊

        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        GameObject atk = Instantiate(mDashEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);
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
    }

    public void onCreateInvisibleAttack()
    {
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.3f : -0.3f;
        //pos.y += 0.3f;
        GameObject atk = Instantiate(mInvisibleAttack, pos, Quaternion.identity) as GameObject;
        //atk.transform.parent = gameObject.transform;
        atk.GetComponent<DamageControl>().setOwner(gameObject);
    }

    public void onCreateAirKickEffect()
    {
        Vector2 pos = gameObject.transform.position;
        //pos.x += facingRight ? 0.9f : -0.9f;
        //pos.y += 0.2f;
        GameObject atk = Instantiate(mShockEffect, pos, Quaternion.identity) as GameObject;
        //atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
        atk.GetComponent<InvisibleControl>().DestoryTime = 0.4f;
    }

    public void onDropEnable()
    {
        mRigidBody.gravityScale = 1;
    }

    bool mIsAirKick = false;
    public void onAirKick()
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

        mIsAirKick = true;
        mRigidBody.gravityScale = 0;
        mRigidBody.velocity = new Vector2(0, 0);

        Invoke("startAirKick", 0.4f);
    }

    public void startAirKick()
    {
        Vector2 dir = rockMan.transform.position - mRigidBody.transform.position;

        float spd = dir.magnitude * 3.5f;
        if (spd < 15)
            spd = 15;
        mRigidBody.velocity = dir.normalized * spd;
            //AddForce(rockMan.transform.position.normalized * 500);
    }

    public override void initial()
    {
        
    }
}