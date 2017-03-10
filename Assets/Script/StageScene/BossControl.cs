using UnityEngine;
using System.Collections;

public abstract class BossControl : MonoBehaviour
{
    public KeyCode up;
    public KeyCode attack;
    public KeyCode left;
    public KeyCode right;
    public float maxSpeed = 3;
    public LayerMask whatIsGround;
    public float jumpForce = 550;
    public Transform groundCheck;


    protected GameObject stageMgr;

    protected bool facingRight = true;
    protected Rigidbody2D rigidBody;
    protected Animator animator;
    protected bool grounded = false;
    protected float groundRadius = 0.2f;
    protected Camera cam;
    protected float maxWidth;
    protected float airSpeed = 3;

    protected bool isRockman = false;

    protected bool specilaMove = false;
    protected bool damaged = false;
    protected Vector2 extraForce;

    protected bool isAttacking = false;
    protected bool IsReady = false;
    protected float move;

    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        cam = Camera.main;
        Vector2 upperCorner = new Vector2(Screen.width, Screen.height);
        Vector2 targetWidth = cam.ScreenToWorldPoint(upperCorner);
        float charWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - charWidth;

        stageMgr = GameObject.Find("StageManager");
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
            transform.localScale = new Vector2(1, 1);
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

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        animator.SetBool("Ground", grounded);

        animator.SetFloat("vSpeed", rigidBody.velocity.y);

        move = 0;
        if (Input.GetKey(left))
            move = -1;
        else if (Input.GetKey(right))
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

            if (grounded && Input.GetKeyDown(up))
            {
                animator.SetBool("Ground", false);
                rigidBody.AddForce(new Vector2(0, jumpForce));

            }
            else if (!grounded && Input.GetKeyUp(up))
            {
                Vector2 velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y / 3);
                rigidBody.velocity = velocity;
            }

            if (!grounded)
                tempSpeed = airSpeed;

            rigidBody.velocity = new Vector2(move * tempSpeed, rigidBody.velocity.y);
        }

        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
    }

    public void setDamage()
    {
        if (!isRockman) // boss 都有無敵
            return;

        if (damaged)
            return;

        damaged = true;
        rigidBody.velocity = new Vector2(0, 0);
        animator.SetTrigger("DamageTrigger");
        animator.SetBool("Damaged", true);
    }

    public bool getIsReady()
    {
        return IsReady;
    }

    public abstract void initial();

}