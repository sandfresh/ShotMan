using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class Boss2AI : MonoBehaviour
{

    private GameObject target;
    private Animator animator;
    private BossCtr02 controler;
    private Vector2 targetDirection;
    private Actions prevAction;
    private Actions nowAction;

    private int level = 0; 
    private float standByTime = 1;
    private float jumpTime = 0;
    private float knifeCounter = 0;

    private float targetDistance;

    private bool isForceIdle = false;

    private bool isAction = false;
    private bool isIdle = false;
    private bool isWalk = false;
    private bool isDashPrepare = false;
    private bool isCrounch = false;
    private bool isSetChange = false;

    private bool isUnderAttack = false;
    private bool isAttacking = false;
    private bool prevAttacking = false;

    private int atkCounter = 0;
    private float superTime = 0;
    private float idleCounter;
    private float actionCounter = 0; 
    private float dashprepareCounter = 0;
    private float changeCounter = 0;

    private GameObject stageMgr;
    private HpControl hpControl;

    void Start ()
    {
        animator = transform.GetComponent<Animator>();
        controler = transform.GetComponent<BossCtr02>();
        target = GameObject.Find("Rockman");
        stageMgr = GameObject.Find("StageManager");
        hpControl = gameObject.transform.GetComponent<HpControl>();
        level = GlobalVariable.getInstance().CurrentLevel;
    }
	
	 
	void Update ()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");

        targetDistance = Vector2.Distance(target.transform.position, transform.position);
        if (prevAttacking)
        {
            if (controler.getIsAttacking() == false)
            {
                isAttacking = false;
                prevAttacking = false;
                print("Attacking fasle");
            }
        }
        else
        {
            prevAttacking = controler.getIsAttacking();
        }


        if (isAttacking || stageMgr.GetComponent<StageManager>().GameStart == false 
            || controler.getIsExcuting())
            return;

	    if (info.IsName("StandBy"))
	    {
            //Face Target 
            Vector3 targetHeading = target.transform.position - transform.position;
            targetDirection = targetHeading.normalized;
            if (targetDistance > 0.5f)
                controler.setDirection(targetDirection.x > 0 ? 1 : -1);
            if (bullets.Length > 0 || targetDistance <= 1.5f)
            {
                isUnderAttack = true;
            }
            else
            {
                isUnderAttack = false;
            }

            if (isForceIdle)
            {
                idleCounter += Time.deltaTime;
                if (idleCounter > standByTime)
                {
                    isForceIdle = false;
                    doAction();
                }
            }
            else if (isUnderAttack)
            {
                doAction();
            }
            else
            {
                idleCounter += Time.deltaTime;
                if (idleCounter > standByTime)
                {
                    standByTime = Random.Range(1f, 1.5f);
                    idleCounter = 0;

                    doAction();
                }
            }
        }
    }

    void doAction()
    {
        float bulletDistance = getBulletDistance();
        int shieldBomb = controler.getInnerShieldNumber();
        Actions nextAction = Actions.Standby;

        if (targetDistance < 2.1f)
        {
            nextAction = Actions.Kick; 
        }
        else
        {
            if (bulletDistance < 2f)
            {
                if (IsSummonShield())
                {
                    nextAction = Actions.Shield;
                }
                else
                {
                    int random = Random.Range(0, 15);
                    if (random < 7)
                        nextAction = Actions.Bomb;
                    else if (random < 9)
                    {
                        if (shieldBomb > 2)
                            nextAction = Actions.Push;
                        else
                            nextAction = (( Random.Range(0,2) < 1 ) ? Actions.Bomb : Actions.Evade); 
                    }  
                    else if (random <13)
                        nextAction = Actions.Evade;
                    else 
                        nextAction = Actions.Standby; 
                }
            }
            else
            {
                if (IsSummonShield())
                {
                    nextAction = Actions.Shield;
                }
                else
                {
                    int random = Random.Range(0, 8);

                    if (random < 5)
                        nextAction = Actions.Bomb;
                    else if (random < 7)
                    {
                        if (shieldBomb > 2)
                            nextAction = Actions.Push;
                        else
                            nextAction = ((Random.Range(0, 2) < 1) ? Actions.Bomb : Actions.Evade);
                    }
                    else
                        nextAction = Actions.Standby;  
                    
                }
            }

            if (isUseSuper())
                nextAction = Actions.Super;

        }
         
        prevAction = nowAction;
        nowAction = nextAction;

        setAction(nowAction);
    }

    void setAction(Actions action)
    {
        switch (action)
        {
           case Actions.Standby:
                break;
           case Actions.Kick:
                setKick();
                break;
           case Actions.Push:
                setPunch();
                break;
           case Actions.Super:
                setSuper();
                break;
           case Actions.Evade:
                setEvade();
                break;
           case Actions.Bomb: 
                setBomb();
                break;
           case Actions.Shield:
                setShield();
                break;
        }
    }


    public void setShield()
    { 
        controler.setShield(true);
        isForceIdle = true;
        standByTime = 2;
    }

    public void setEvade()
    {
        controler.setEvade();
    }

    public void setPunch()
    {
        isAttacking = true;
        controler.setPunch();
    }

    public void setKick()
    {
        isAttacking = true;
        controler.setKick();
    }

    public void setBomb()
    {
        controler.setBomb();
    }

    public void setSuper()
    {
        controler.setSuper();
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

    private bool IsSummonShield()
    {
        int shieldBomb = controler.getOuterShiledNumber() + controler.getInnerShieldNumber();

        int summonProability = 0;

        if (GlobalVariable.getInstance().CurrentLevel > 0)
        {
            summonProability = (int)((1 - ((float)shieldBomb / 10)) * 100);
        }
        else
        {
            summonProability = (int)((1 - ((float)shieldBomb / 5)) * 100);
        }
         
        return  (Random.Range(0, 201) < summonProability)?  true : false; 
    }

    private bool isUseSuper()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float hppercent = ((float)hpControl.hp / hpControl.getMaxHp()) * 100;

        float superProability = (int)((100 - hppercent));
        int random = Random.Range(0, 101);

        bool returnValue = false;
        if (hppercent < 50 && superTime <= 0 && (info.IsName("StandBy")) && level  >= 2)
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
