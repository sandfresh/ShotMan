using UnityEngine;
using System.Collections;

public class Boss1Control : PlayerControl
{
    public GameObject dashEffect;
    public GameObject dashAtkEffect;
    public GameObject pushAtkEffect;
    public KeyCode pushKey;
    public KeyCode dashKey;

    float mAttackWaitTime = 0;
    bool mIsDashing = false;
    bool mIsWalk = false;

    public float DashSpeed = 3;
    public float WalkSpeed = 1f;

    // Use this for initialization
    new void Start () 
	{
        base.Start();
        anim = GetComponent<Animator>();
        airSpeed = 5; 
    }
	
    void FixedUpdate()
    {
       
    }

    
   
    void Update()
    {
        actionCheck();
         

        if (grounded && Input.GetKeyDown(dashKey))
        {
            anim.SetTrigger("DashTrigger");
        }
        else if (mIsDashing && (Input.GetKeyUp(left) || Input.GetKeyUp(right)))
        {
            Invoke("stopDash", 0.1f);
            mRigidBody.velocity = new Vector2(mRigidBody.velocity.x / 3, mRigidBody.velocity.y);
        }

        if (!isAttacking)
        {
            if (Input.GetKeyDown(attack))
            {
                //anim.SetBool("Attacking", true);
                anim.SetTrigger("Knife");
                mAttackWaitTime = 0;

                if (mIsDashing)
                    Invoke("stopDash", 0.1f);
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
            mRigidBody.velocity = new Vector2(0, mRigidBody.velocity.y);
            mAttackWaitTime += Time.deltaTime;
            if (mAttackWaitTime > 0.2f)
            {
                anim.SetBool("Attacking", false);
            }
        }

        if (mIsWalk)
        {
            mRigidBody.velocity = facingRight ? new Vector2(WalkSpeed, 0) : new Vector2(-WalkSpeed, 0);
        }

        anim.SetFloat("Speed", Mathf.Abs(mRigidBody.velocity.x));
    }

    public void setWalk(bool value)
    {
        mIsWalk = value;
    }

    public void stopDash()
    {
        specilaMove = false; 
    }

    public void dash()
    {
        specilaMove = true;
        Vector2 force = facingRight? new Vector2(300, 0): new Vector2(-300, 0);
        mRigidBody.AddForce(force);
        mIsDashing = true; 

        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight ? 0.5f : -0.5f;
        GameObject atk = Instantiate(dashEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);
    }

    public void onAttackStart()
    {
        anim.SetBool("Attacking", true);
    }

    public void onAttackStop()
    {
        isAttacking = false;
        anim.ResetTrigger("Knife"); 
    }

    public void knifeAtk()
    {
        isAttacking = true;
        Vector2 pos = gameObject.transform.position;
        pos.x += facingRight  ? 0.5f : -0.5f;
        pos.y += 0.4f;
        GameObject atk = Instantiate(dashAtkEffect, pos, Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
    }

    public void pushAtk()
    {
        isAttacking = true;
        Vector2 pos = gameObject.transform.position;
        pos.y += 0.5f;
        GameObject atk = Instantiate(pushAtkEffect, new Vector2(pos.x - 0.8f, pos.y + 0.5f), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        atk.GetComponent<PushControl>().setDir(-1);
        atk.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);

        atk = Instantiate(pushAtkEffect, new Vector2(pos.x + 0.8f, pos.y + 0.5f), Quaternion.identity) as GameObject;
        atk.transform.parent = gameObject.transform;
        //atk.GetComponent<PushControl>().setDir(facingRight ? 1 : -1);
        atk.transform.localScale = new Vector2(facingRight ? -1 : 1, 1);// facingRight ? 1 : -1, 1);
        atk.GetComponent<DamageControl>().setOwner(gameObject);
    }

    public override void initial()
    {
        throw new System.NotImplementedException();
    }

    
}