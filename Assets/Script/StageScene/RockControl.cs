using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RockControl : PlayerControl
{
    public GameObject bullet;
    public GameObject rotateBall;
    public GameObject rockLight;

    Transform shotBulletPos;

    private bool isShoot = false; 
    float mShootWaitTime = 0;

    private AudioClip shotSE;
    private AudioClip wallSE;
    private AudioClip showSE;


    new void Start () 
	{
        base.Start();
        anim = GetComponent<Animator>();
        anim.enabled = false;

        isRockman = true;
        airSpeed = maxSpeed;
        
        shotBulletPos = transform.FindChild("FirePos");
        shotSE = Resources.Load("Sound/Bullet") as AudioClip;
        wallSE = Resources.Load("Sound/Wall") as AudioClip;
        showSE = Resources.Load("Sound/rockEnter") as AudioClip;

        GetComponent<SpriteRenderer>().enabled = false;
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.1f);
        mySequence.AppendCallback(() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(showSE);
        });
        EnterStageAnimation();
	}
     

    public void onEnter()
    {
        anim.SetBool("Enter", true);
    }

    public void onStopEnter()
    {
        anim.SetBool("Enter", false); 
        IsReady = true;
    }

    public void FixedUpdate()
    {
        
    }
      
	void Update ()
    { 

        if (stageMgr.GetComponent<StageManager>().GameStart == false)
        { 
            return;
        }

	    if (Time.timeScale == 0)
	        return;

        if (isShoot)
        {
            mShootWaitTime += Time.deltaTime;
            if (mShootWaitTime > 0.3f)
            {
                anim.SetBool("Shoot", false);
                isShoot = false;
            }
        }

        actionCheck(); 
        
        if (!damaged)
        {
            if (Input.GetKeyDown(attack) || moveCtrl.APressed)
            { 
                moveCtrl.APressed = false;
                AudioMgr.Instance.Audio.PlayOneShot(shotSE);
                GameObject newBullet = Instantiate(bullet, shotBulletPos.position, Quaternion.identity) as GameObject;
                newBullet.transform.localScale = new Vector2(facingRight ? 1 : -1, 1);
                newBullet.GetComponent<BulletControl>().setDirection(facingRight ? 1 : -1);
                newBullet.GetComponent<DamageControl>().setOwner(gameObject);
                anim.SetBool("Shoot", true);
                isShoot = true;
                mShootWaitTime = 0;
            }
        } 
    }

    

    public void onDamageFinish()
    {
        anim.SetBool("Damaged", false);
        damaged = false;
        //Debug.Log("damageFinish");
    }

    public void EnterStageAnimation()
    {
        GameObject obj = Instantiate(rotateBall, gameObject.transform.position, Quaternion.identity) as GameObject;
        obj.GetComponent<RotateBall>().
            MoveIn(() =>
            {
                Debug.Log("Enter"); 
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Animator>().enabled = true;
            });
    }

    public void ExitStageAnimation()
    { 
        GameObject obj = Instantiate(rotateBall, gameObject.transform.position, Quaternion.identity) as GameObject;
        obj.GetComponent<RotateBall>().MoveOut(() =>
        {
         
        }); 
        GetComponent<SpriteRenderer>().enabled = false; 
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    } 

    public override void initial()
    {

    }
     

    public override void onStopWinEvent()
    {
        GameObject obj = Instantiate(rockLight, gameObject.transform.position, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = new Vector3(1,1,1);
        obj.transform.localPosition = facingRight ? new Vector2(0.2f, 0.5f) : new Vector2(0.2f, 0.5f);
        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rockLight") as AudioClip);

        Sequence mySequence = DOTween.Sequence(); 
        mySequence.AppendInterval(3);
        mySequence.AppendCallback(() =>
        {
            stageMgr.ShowScore();
        });
    } 

    public override void onStopDownEvent()
    {
        base.onStopDownEvent();
        print("onStopDownEvent");
        ExitStageAnimation();

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(6);
        mySequence.AppendCallback(() =>
        {
            stageMgr.ShowScore();
        });
      
    }
     
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.gameObject;

        if(target)

        if ((target.name == "Wall") && (isKnock == true))
        {
                print("IsKnock");
            AudioMgr.Instance.Audio.PlayOneShot(wallSE);
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().DOShakePosition(1, new Vector2(1, 1),10,0);
            isKnock = false;
        }
       
    }
}