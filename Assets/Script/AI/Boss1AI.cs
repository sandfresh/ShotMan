using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Boss1AI : MonoBehaviour 
{   
    private GameObject target;
    private Animator selfAnim; 
    private BossCtr01 controler; 
    private HpControl hpControl;
    private Vector2 targetDirection;
    private Actions prevAction;
    private Actions nowAction;
    private StageManager stageMgr;
    private Animator animator;

    private int level = 0;
    private float pushTime = 0;
    private float standByTime = 1;
    private float jumpTime = 0;
    private float knifeTime = 0;
    private float knifeCounter = 0;
    private float superTime = 0;
    private float walkTime = 0;

    private float targetDistance;

    private bool isForceIdle = false;
    private bool IsPushable = true;
    private bool IsJumpable = false;

    private bool isAction = false;
    private bool isIdle = false; 
    private bool isKnife = false;
    
    private bool isUnderAttack = false;
    private float waitTime;

     
    void Start () 
	{ 
        selfAnim = transform.GetComponent<Animator>();
        controler = transform.GetComponent<BossCtr01>(); 
        target = GameObject.Find("Rockman");
        hpControl = GetComponent<HpControl>();
        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();
        animator = gameObject.GetComponent<Animator>();
        level = GlobalVariable.getInstance().CurrentLevel;
	} 

    void Update()
    { 
        AnimatorStateInfo info = selfAnim.GetCurrentAnimatorStateInfo(0);
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        targetDistance = Vector2.Distance(target.transform.position, transform.position);

        if (Time.timeScale == 0)
            return;

        if (controler.getIsGround() == false  || controler.getIsJumping() || controler.getIsDash()  
            ||controler.getIsSuper())
            return;

        if (stageMgr.GetComponent<StageManager>().GameStart == false) 
            return;


        if (bullets.Length > 0 || targetDistance <= 1.5f)
        {
            isUnderAttack = true;
        }
        else
        {
            isUnderAttack = false;
        }

        if (IsPushable == false)
        {
            pushTime += Time.deltaTime;
            if (pushTime > 2)
            {
                IsPushable = true;
                pushTime = 0;
            }
        }
        if (IsJumpable == false)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > 1f)
            {
                IsJumpable = true;
                jumpTime = 0;
            }
        }

        if (knifeCounter > 0)
        {
            knifeTime += Time.deltaTime;
            if (knifeTime >= 1)
            {
                knifeCounter--;
                knifeTime = 0;
            }
        }

        if (superTime > 0)
        {
            superTime -= Time.deltaTime;
        }

        if (controler.getIsExcuting())
        {
            if ( !info.IsName("StandBy") && !info.IsName("Walk"))
            {
                return;
            }
        }
        if (info.IsName("StandBy"))
        {
            //Face Target 
            Vector3 targetHeading = target.transform.position - transform.position;
            targetDirection = targetHeading.normalized;
            if (targetDistance > 0.5f)
                controler.setDirection(targetDirection.x > 0 ? 1 : -1);  
 
            if (isUnderAttack)
            {
                waitTime = 0;
                doAction();  
            }
            else
            {
                waitTime += Time.deltaTime; 

                if (waitTime > standByTime)
                {
                    standByTime = Random.Range(0.1f, 0.4f);
                    waitTime = 0;
                    doAction(); 
                }
            } 
        }  
        /*
         *    Walk , can be interrupted by attack 
         */
        else if (controler.getIsWalking())
        {
            //Face Target
            Vector3 targetHeading = target.transform.position - transform.position;
            targetDirection = targetHeading.normalized;
            if(targetDistance > 0.5f)
                controler.setDirection(targetDirection.x > 0 ? 1 : -1);

            if (isUnderAttack)
            {
                waitTime = 0;
                doAction(); 
            }
            else
            {
                waitTime += Time.deltaTime;
                
                if (waitTime >= walkTime)
                {
                    waitTime = 0;
                    doAction();  
                }
            }
        }

    }

    public void doAction()
    { 
        print("DoAction");
        var bullets = GameObject.FindGameObjectsWithTag("Bullet"); 
         
        float minDistance = 1000; 

        for (int i=0 ;i < bullets.Length ; i++)
        {
            float distance = Vector2.Distance(transform.position, bullets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            } 
        }
        float bulletDistance = minDistance;

        Actions nextAction = Actions.Standby;

        //According to the player distance to do next action.  
        if (targetDistance < bulletDistance) 
        {
            print("Player close");
            if (targetDistance <= 1.5f)
            {
                if (IsPushable)
                {
                    int random = Random.Range(0, 10);

                    if (random <= 7)
                        nextAction = Actions.Knife;
                    else
                    {
                        random = Random.Range(0, 20);
                        if(random < 12)
                        {
                            nextAction = Actions.Push;
                        }
                        else if(random < 16)
                        {
                            if (isJumpable())
                                nextAction = Actions.Jump;
                            else
                                nextAction = Actions.Dash; 
                        } 
                        else  
                        {
                            nextAction = Actions.Walk;
                        } 
                    }
                       
                }
                else
                {
                    nextAction = Actions.Knife;
                }
            } 
            else if (targetDistance < 3)
            {
                int random =  Random.Range(0, 10);

                if (random < 3)
                    nextAction = Actions.Walk;
                else if (random < 5)
                {
                    if (isJumpable())
                        nextAction = Actions.Jump;
                    else
                        nextAction = Actions.Dash;
                }
                else
                {
                    nextAction = Actions.Dash;
                }
            }
            else 
            {
                int random =  Random.Range(0, 10);
                if (random < 2)
                    nextAction = Actions.Walk;
                else if (random < 3)
                {
                    if (isJumpable())
                        nextAction = Actions.Jump;
                    else
                        nextAction = Actions.Dash;
                } 
                else  
                    nextAction = Actions.Dash; 
            }
        }
        else
        {
            if (bulletDistance <= 2.5f)
            {
                int random = Random.Range(0, 20);
                if (random < 6)
                    nextAction = Actions.Knife;
                else if (random < 10)
                {
                    nextAction = Actions.Push;
                }
                else if (random < 15)
                {
                    nextAction = Actions.Dash;
                }
                else
                {
                    nextAction = Actions.Jump;
                }

            }
        }

        if (level == 0)
        {
            if(nextAction == Actions.Jump)
                nextAction = Actions.Dash;
        }  
        
        if (nextAction == Actions.Knife)
            knifeCounter++; 
        
        if (knifeCounter >= 5)
        { 
            knifeCounter = 0;
            nextAction = Actions.Push; 
        }

        if (isUseSuper())
            nextAction = Actions.Super;
        print("NextAction:"+nextAction+" , "+Time.time);
        setAction(nextAction);
    }

    public void setAction(Actions action)
    { 
        prevAction = nowAction;
        nowAction = action;
         
        switch (action)
        {
            case Actions.Walk: 
                setKnife(false);
                setWalk(true);
                break;
            case Actions.Knife:
                setWalk(false);
                setKnife(true); 
                break;
            case Actions.Dash: 
                setWalk(false);
                setKnife(false);
                setDashPrepare(true);
                break;
            case Actions.Push: 
                setWalk(false);
                setKnife(false);
                setPush();
                break;
            case Actions.Jump: 
                setKnife(false);
                setWalk(false);
                setJump(true);
                break;
            case Actions.Standby: 
                setDashPrepare(false);
                setKnife(false);
                setWalk(false);
                setJump(false); 
                break;
            case Actions.Super:
                setKnife(false);
                setWalk(false);
                setJump(false);
                setSuper();
                break;
            default: 
                setWalk(false);
                setKnife(false);
                setDashPrepare(false);
                break;
        }
    }
      
    public void setWalk(bool value)
    { 
        controler.setWalk(value);
        if (value)
        {
            walkTime = Random.Range(1.5f, 3.0f);
            waitTime = 0;
        } 
    }

    public void setIdle(bool value = true)
    {
        isIdle = value;
    }

    public void setJump(bool value)
    {
        controler.setJump(value); 
    }

    public void setKnife(bool value)
    { 
        controler.setKnife(value);
    }

    public void setDashPrepare(bool value)
    { 
        if (value)
        { 
            controler.setDashPrepare(true);  
            controler.setTargetPoint(target.transform.position);
        } 
        else
        { 
            controler.setDashPrepare(false); 
        }
    } 

    public void setSuper()
    {
        controler.setSuper();
    }

    public void setPush()
    { 
        controler.setPush();
        IsPushable = false;
    } 

    private bool isJumpable()
    {
        bool retValue = false;
        if (IsJumpable)
        {
            if (prevAction == Actions.Walk || prevAction == Actions.Standby || prevAction == Actions.Dash)
                retValue = true; 
        }
        return retValue;
    }

    private bool isUseSuper()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float hppercent = ((float)hpControl.hp / hpControl.getMaxHp()) * 100;

        float superProability = (int)((100 - hppercent)); 
        int random = Random.Range(0, 101);

        bool returnValue = false;
        if (hppercent < 50 && superTime <= 0 && (info.IsName("StandBy")) && level > 1)
        {
            if ((random < superProability) & (controler.getSuperBombNumber() <= 0))
            {
                returnValue = true;
                superTime = 10;
            } 
        } 

        return returnValue;
    }
}