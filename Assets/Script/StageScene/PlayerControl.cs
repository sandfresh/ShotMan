using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class PlayerControl : MonoBehaviour 
{
    public KeyCode up;
    public KeyCode attack;
    public KeyCode left;
    public KeyCode right; 
    public LayerMask whatIsGround; 
    public Transform groundCheck;
    

    public float maxSpeed = 3;
    public float jumpForce = 550;

    protected Rigidbody2D mRigidBody;
    protected Animator anim;
    protected Vector2 extraForce;
    protected Camera cam;
    protected StageManager stageMgr;
    protected MoveControl moveCtrl; 

    protected float groundRadius = 0.2f; 
    protected float maxWidth;
    protected float airSpeed = 3;
    protected float move;

    protected bool facingRight = true;
    protected bool grounded = false;
    protected bool isRockman = false; 
    protected bool specilaMove = false;
    protected bool damaged = false; 
    protected bool isAttacking = false;
    protected bool IsReady = false;
    protected bool isDown = false;
    protected bool isUnbeatable = false;
    protected bool isKnock = false;
    protected bool isExcuting = false;
    protected AudioClip hpUpSE; 

    public bool Unbeatable
    {
        get { return isUnbeatable; }
    }


    public bool Down
    {
        get
        {
            return isDown;
        }

        set
        {
            isDown = value;
        }
    }


    protected void Start () 
	{
        mRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveCtrl = GetComponent<MoveControl>(); 
        cam = Camera.main;
        Vector2 upperCorner = new Vector2(Screen.width, Screen.height);
        Vector2 targetWidth = cam.ScreenToWorldPoint(upperCorner);
        float charWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - charWidth;

        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();

        hpUpSE = Resources.Load("Sound/lifeUp") as AudioClip; 
    }
      
    public bool isRockMan()
    {
        return isRockman;
    }

    protected void Flip()
    {
        if (damaged)
            return;
        facingRight = !facingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void setDirection(int direciton)
    { 
        if (direciton > 0)
        {
            transform.localScale = new Vector2(1,1);
            facingRight = true;
        }
        else 
        {
            transform.localScale = new Vector2(-1, 1);
            facingRight = false;
        }
    }

    protected void actionCheck()
    {  
        float tempSpeed = maxSpeed;

        bool isgrounded  = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        
        if (isRockman)
        {
            if(isgrounded == true && grounded == false)
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Land") as AudioClip);  
        } 
        grounded = isgrounded;

        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", mRigidBody.velocity.y);
         
        move = 0;
        if (Input.GetKey(left) || moveCtrl.LeftPressed)
            move = -1;
        else if (Input.GetKey(right) || moveCtrl.RightPressed)
            move = 1;
   
        //move = Input.GetAxis("Horizontal"); 

        if (move < 0)
            move = -1;
        else if (move > 0)
            move = 1;


        if (!damaged && !specilaMove && !isAttacking)
        {
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();

            if (grounded && (Input.GetKeyDown(up)))
            {
                anim.SetBool("Ground", false);
                Vector2 fore = new Vector2(0, jumpForce);
                //mRigidBody.AddForce(fore);
                mRigidBody.AddForce(fore*Time.fixedDeltaTime,ForceMode2D.Impulse);

            }
            else if (!grounded && (Input.GetKeyUp(up)))
            { 
                Vector2 velocity = new Vector2(mRigidBody.velocity.x, mRigidBody.velocity.y / 3);
                mRigidBody.velocity = velocity;
            }

            if (!grounded)
                tempSpeed = airSpeed;

            mRigidBody.velocity = new Vector2(move * tempSpeed, mRigidBody.velocity.y);
        }

        anim.SetFloat("Speed", Mathf.Abs(mRigidBody.velocity.x));
    }

    public void jump()
    {
        StartCoroutine(rundJumpDealy());
    }

    private bool isJumping = false;
    private IEnumerator rundJumpDealy()
    {
        if (!damaged && !specilaMove && !isAttacking)
        {
            if (grounded && isJumping == false)
            {
                anim.SetBool("Ground", false);
                mRigidBody.AddForce(new Vector2(0, jumpForce));
                isJumping = true;
                yield return new WaitForSeconds(0.3f);
                isJumping = false;
            }
            else if (!grounded)
            {
                Vector2 velocity = new Vector2(mRigidBody.velocity.x, mRigidBody.velocity.y / 3);
                mRigidBody.velocity = velocity;
            }
        }
      
    }


    public void setDamage()
    {

        if (!isRockman) // boss 都有無敵
            return;

        if (damaged)
            return;

        damaged = true;
       
        mRigidBody.velocity = new Vector2(0, 0);

        mRigidBody.gravityScale = 0;
        DOTween.Kill("damageGravity");
        Sequence mySequence = DOTween.Sequence();
        mySequence.id = "damageGravity";
        mySequence.AppendInterval(0.4f);
        mySequence.AppendCallback(() =>
        {
            mRigidBody.gravityScale = 1;
        });

        anim.SetTrigger("DamageTrigger");
        anim.SetBool("Damaged", true);
    }

    public bool getIsReady()
    {
        return IsReady;
    }
     

    public void setUnbeateable(GameObject collider)
    {
        GameObject collisionObject = collider.gameObject;

        if (gameObject.name != "Rockman")
        {
            if (collisionObject.tag == "Bullet")
            {
                if (isUnbeatable == false)
                {
                    isUnbeatable = true;
                    float unbeatabletime = 0.6f;

                    DOTween.Kill(Unbeatable);
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.id = "unbeatable";
                    mySequence.AppendInterval(unbeatabletime);
                    mySequence.AppendCallback(() => { isUnbeatable = false; });
                } 
            }
        }
        else
        {
            if (isUnbeatable == false)
            {
                isUnbeatable = true;
                float unbeatabletime = 0.6f;

                DOTween.Kill(Unbeatable);
                Sequence mySequence = DOTween.Sequence();
                mySequence.id = "unbeatable";
                mySequence.AppendInterval(unbeatabletime);
                mySequence.AppendCallback(() => { isUnbeatable = false; });
            }

        }
    } 

    public virtual void setDown()
    {
        print("Player down");
        anim.SetTrigger("Down"); 
    }

    public virtual void setWin()
    {
        anim.SetTrigger("Win"); 
    }
     

    public virtual void onDownEvent()
    { 
        print("OnDownEvent");
        if (gameObject.name != "Rockman")
        {
            Time.timeScale = 0.2f;
            mRigidBody.velocity = Vector2.zero;
            if (facingRight)
                mRigidBody.AddForce(new Vector2(-200, 200)*Time.fixedDeltaTime,ForceMode2D.Impulse);
            else
                mRigidBody.AddForce(new Vector2(200 , 200)*Time.fixedDeltaTime, ForceMode2D.Impulse);

            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/BossOver") as AudioClip);
        } 
    }

    public virtual void onStopDownEvent()
    { 
        isDown = true;
        Time.timeScale = 1f; 
        stageMgr.showResult();
    }

    public virtual void onWinEvent()
    {
     
    }

    public virtual void onStopWinEvent()
    {
        
    }

    public virtual void initial()
    {
        AudioMgr.Instance.Audio.PlayOneShot(hpUpSE);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if(gameObject.name == "Rockman")
        //print("collisionTrigger:"+collision.name); 
    }


    public void setKnok(bool value)
    { 
        isKnock = value;
        DOTween.Kill("knock");
        Sequence knockSequence = DOTween.Sequence();
        knockSequence.id = "knock";
        knockSequence.AppendInterval(2);
        knockSequence.AppendCallback(() =>
        {
            isKnock = false;
        });
    }
     
}