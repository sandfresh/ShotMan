using UnityEngine;
using System.Collections;
using System.Net;
using System.Runtime.Remoting.Messaging;

public class Boss3AI : MonoBehaviour
{

    // Use this for initialization
    private GameObject target;
    private Animator animator;
    private BossCtr03 controler;
    private Vector2 targetDirection;
    private Actions prevAction;
    private Actions nowAction;
    private HpControl hpControl;
     
    private float standByTime = 1;
    private float jumpTime = 0;
    private float walkTime = 0;

    private float targetDistance;
     
    private bool isForceIdle = false; 

    private bool isAction = false;
    private bool isIdle = false; 
    private bool isCrounch = false; 

    private bool isUnderAttack = false;

    private int level = 0;
    private int atkCounter = 0; 
    private float atkTime = 0;
    private float superTime = 0;
    private float waitTime;
    private float actionCounter = 0; 
    private float dashprepareCounter = 0;
    private float changeCounter = 0; 

    private GameObject stageMgr;

    void Start ()
    {
        animator = transform.GetComponent<Animator>();
        controler = transform.GetComponent<BossCtr03>();
        target = GameObject.Find("Rockman");
        stageMgr = GameObject.Find("StageManager");
        hpControl = GetComponent<HpControl>();
        level = GlobalVariable.getInstance().CurrentLevel;
    }
	
	 
	void Update ()
	{ 
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        targetDistance = Vector2.Distance(target.transform.position, transform.position);
   
        if (bullets.Length > 0 || targetDistance <= 1.5f)
        {
            isUnderAttack = true;
        }
        else
        {
            isUnderAttack = false;
        }

	    if (stageMgr.GetComponent<StageManager>().GameStart == false)
	    {
	        return;
	    }

        if (atkCounter > 0)
        {
            atkTime += Time.deltaTime;
            if (atkTime >= 2)
            {
                atkCounter--;
                atkTime = 0;
                print("AtkCounter:"+atkCounter);
            }
        }

        if (superTime > 0)
        {
            superTime -= Time.deltaTime;
        }

        if ((controler.getIsGround() == false))
	    { 
            return;
        } 

        if (controler.getIsExcuting())
        {
            if (isUnderAttack == true)
            {
                if (controler.getIsWalking() == true)
                { 
                    controler.setDirection(targetDirection.x > 0 ? 1 : -1);
                } 
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        if (info.IsName("WolfStandBy") || controler.getIsChanging() || controler.getIsSetChange())
        { 

            return;
        }

        Vector3 targetHeading = target.transform.position - transform.position;
        targetDirection = targetHeading.normalized;

       

        if (info.IsName("StandBy"))
        {
            //Face Target  
            if (targetDistance > 0.5f)
            { 
                controler.setDirection(targetDirection.x > 0 ? 1 : -1);
            }
                

            if (isForceIdle)
            {
                waitTime += Time.deltaTime;
                if (waitTime > standByTime)
                {
                    isForceIdle = false;
                    waitTime = 0; 
                    doAction();
                }
            }  
            else if (isUnderAttack)
            {
                waitTime = 0;
                doAction(); 
            }
            else
            {
                waitTime += Time.deltaTime;
                if (waitTime > standByTime)
                {
                    standByTime = Random.Range(1f, 1.5f);
                    waitTime = 0; 
                    doAction();
                }
            }
        } 
   
        /*
         * Walk , can be interrupted by attack 
         */
        else if (controler.getIsWalking())
        {
            //Face Target 
            if (targetDistance > 0.5f )
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
        else if (controler.getIsCrouching())
        {
            //Face Target

            if (targetDistance > 0.5f && controler.getIsAttacking() == false)
            {
                controler.setDirection(targetDirection.x > 0 ? 1 : -1);
            }
            doAction(); 
        }

       
    }

    private void doAction()
    { 
        float bulletDistance = getBulletDistance();

        Actions nextAction = Actions.Standby ;

        /*
            1.Player's distances is closer than bullet.
            2.No Bullets exist on the scene. 
        */
         
        if (targetDistance < bulletDistance)
        {
            /*
                 Player is very close.
                 Close Attack.
            */  
            if (targetDistance <= 1.5f)
            {
                if (isCrounch)
                {
                    int random = Random.Range(0, 11);

                    if (random < 6 || atkCounter > 3)
                        nextAction = Actions.Punch;
                    else
                    {
                        if (atkCounter < 3)
                        {
                            nextAction = Actions.Kick;
                            atkCounter++;
                        }
                        else
                        {
                            nextAction = Actions.Raise; 
                        }
                    }  
                }
                else
                {
                    nextAction = Actions.Crounch;
                }
            }
            /*
               Play is at a distance.
               Dash, Crounch kick , Jump , Jump Kick.
            */ 
            else if (targetDistance < 4)
            {
                if (isCrounch)
                {
                    int random = Random.Range(0, 11);

                    if (random < 8)
                    {
                        if (atkCounter < 3)
                        {
                            nextAction = Actions.Kick;
                            atkCounter++;
                        }
                        else
                        {
                            nextAction = Actions.Raise; 
                        }
                    }
                    else
                    {
                        nextAction = Actions.Raise; 
                    } 
                }
                else
                {
                    nextAction = Actions.Walk;
                }  
            }
            else
            {
                if (isCrounch)
                {
                    if (atkCounter < 3)
                    { 
                        nextAction = Actions.Kick;
                        atkCounter++; 
                    }
                    else
                    {
                        nextAction = Actions.Raise; 
                    } 
                }
                else
                {
                    int random = Random.Range(0, 21);
                    if (random < 5)
                        nextAction = Actions.Crounch;
                    else if (random < 10 && (bulletDistance > 3))
                    {
                        nextAction = Actions.Jump;
                    }
                    else
                    {
                        int random2 = Random.Range(0, 20);
                        if (random2 < 3)
                            nextAction = Actions.Dash;
                        else
                            nextAction = Actions.Walk; 
                    }
                }
            }
        }
        /*
            1.Bullets exist on the scene.
            2.Bullets' Quarters is close.
        */
        else
        { 
            /*
               Player is very close.  
               Close Attack.
            */
            if (targetDistance <= 1.5f)
            {
                if (isCrounch)
                {
                    int random = Random.Range(0, 11);

                    if (random < 7 || atkCounter > 3)
                        nextAction = Actions.Punch;
                    else
                    {
                        if (atkCounter < 3)
                        {
                            nextAction = Actions.Kick;
                            atkCounter++;
                        }
                        else
                        {
                            nextAction = Actions.Raise; 
                        }
                    } 
                }
                else
                {
                    int random = Random.Range(0, 11);
                    if (random < 5)
                        nextAction = Actions.Punch;
                    else
                    {
                        nextAction = Actions.Crounch; 
                    }
                }
                    
            }
            /*
                Player is far away.  
                Bounce bullet , Dash or Jump .  
            */
            else if (targetDistance < 3)
            {
                if (isCrounch)
                {
                    if (atkCounter < 3)
                    { 
                        nextAction = Actions.Kick;
                        atkCounter++;
                    }
                    else
                    {
                        nextAction = Actions.Raise; 
                    } 
                }
                else
                {
                    int random = Random.Range(0, 20);
                    if (random < 8)
                    {
                        int random2 = Random.Range(0, 2);
                        if (random2< 1)
                            nextAction = Actions.Punch;
                        else
                            nextAction = Actions.Kick;
                         
                    }
                    else if (random < 10)
                    {
                        nextAction = Actions.Jump;
                    }
                    else if (random < 15 || atkCounter >3)
                        nextAction = Actions.Dash;
                    else
                    {
                        nextAction = Actions.Crounch; 
                    }
           
                }  
            }
            else
            {
                if (isCrounch)
                {
                    if (atkCounter < 3)
                    { 
                        nextAction = Actions.Kick;
                        atkCounter++;
                    }
                    else
                    {
                        nextAction = Actions.Raise; 
                    } 
                }
                else
                {
                    int random = Random.Range(0, 21); 
                    if (random < 8)
                    {
                        int random2 = Random.Range(0, 11);
                        if (random2 < 5)
                        {
                        //    isAttacking = true;
                            int random3 = Random.Range(0, 2);
                            if(random3 < 1)
                                nextAction = Actions.Punch;
                            else 
                                nextAction = Actions.Kick; 
                        }
                        else 
                        {
                            if (atkCounter < 3)
                            {
                                nextAction = Actions.Kick;
                                atkCounter++;
                            }
                            else
                            {
                                nextAction = Actions.Raise; 
                            }
                        }
                         
                    }
                    else if(random < 10)
                    {
                        nextAction = Actions.Jump;
                    }
                    else if (random < 12 || atkCounter > 3)
                    {
                        int random2 = Random.Range(0, 3);
                        if (random2 < 1)
                            nextAction = Actions.Dash;
                        else
                            nextAction = Actions.Walk;
                    } 
                    else 
                    {
                        nextAction = Actions.Crounch; 
                    }
                } 
            } 
        }

        if (level == 0)
        {
            if(nextAction == Actions.Jump)
                nextAction = Actions.Dash; 
        }

        if (isUseSuper())
        {
            nextAction = Actions.Change;
        } 

        prevAction = nowAction;
        nowAction = nextAction;

        print("NextAction:"+nextAction);
        setAction(nowAction);
    }

    private void setAction(Actions action)
    { 
        switch (action)
        {
            case Actions.Standby: 
                setCrounch(false);
                setPunch(false);
                setKick(false);
                setWalk(false);
                break;
            case Actions.Walk: 
                setCrounch(false);
                setPunch(false);
                setKick(false);  
                setDashPrepare(false);
                setWalk(true);
                break;
            case Actions.Crounch:
                setWalk(false);
                setDashPrepare(false); 
                setCrounch(true);
                break;
            case Actions.Punch: 
                setWalk(false); 
                setDashPrepare(false); 
                setPunch(true);
                break; 
            case Actions.Dash:  
                setWalk(false);
                setPunch(false);
                setKick(false);
                setCrounch(false);
                setDashPrepare(true);
                break;
            case Actions.Kick: 
                setWalk(false);
                setDashPrepare(false); 
                setKick(true);
                break; 
            case Actions.Change: 
                setWalk(false);
                setCrounch(false);
                setPunch(false);
                setKick(false);
                setDashPrepare(false);
                setJump(false);
                setChange(true);
                break;
            case Actions.Raise:
                setCrounch(false);
                break;
            case Actions.Jump: 
                setCrounch(false);
                setWalk(false);
                setKick(false);
                setDashPrepare(false);
                setJump(true);  
                break; 
            case Actions.CrounchKick:
                setCrounch(true);
                setKick(true);
                break;
        }
    }
     

    private void setWalk(bool value)
    { 
        controler.setWalk(value);
        if (value)
        {
            walkTime = Random.Range(1.5f, 3.0f);
        } 
    }

    public void setCrounch(bool value)
    {
        controler.setCrounch(value);
        isCrounch = value;
    }

    public void setKick(bool value)
    {
        controler.setKick(value); 
    }

    public void setPunch(bool value)
    { 
        controler.setPunch(value); 
    }

    public void setDashPrepare(bool value)
    {
        controler.setDashPrepare(value);
    }

    public void setJump(bool value)
    { 
        controler.setJump(value);
    }

    public void setChange(bool value)
    { 
        controler.setChange(value); 
    }


    public float getBulletDistance()
    {
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");

        float minDistance = 1000;

        for (int i = 0; i < bullets.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, bullets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        float bulletDistance = minDistance;

        return bulletDistance; 
    }
      
    private bool isUseSuper()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        float hppercent = ((float)hpControl.hp / hpControl.getMaxHp())*100;

        float superProability = (int)((100 - hppercent) * 0.1f);
        int random = Random.Range(0, 101);

        bool returnValue = false;
    
        if (superTime <= 0 && (info.IsName("StandBy") || info.IsName("Walk")) && hppercent < 50 && level >= 2)
        {
            if (random < superProability)
            {
                returnValue = true;
                superTime = 10;
            } 
        } 
        return returnValue;
    }

}
