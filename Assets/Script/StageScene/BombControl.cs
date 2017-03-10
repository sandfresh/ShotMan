using System;
using DG.Tweening;
using UnityEngine; 

public class BombControl : MonoBehaviour
{

    public Transform ExplosionEffect; 
    public GameObject DustEffect;
    public float Speed = 3.5f;

    Vector2 mDirection = new Vector2(0, 0);
    DamageControl dmgCtrl; 

    private int hp = 7;
    private bool isReflect = false;
    private bool isGrounded = false;
    private bool isCounterdown = false;
    private Action DestroyAction;

    void Awake()
    {
        dmgCtrl = GetComponent<DamageControl>();
    }

    void Start()
    { 
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = mDirection * Time.deltaTime * Speed;
        transform.Translate(moveDir);

        if (Mathf.Abs(transform.position.x) > 60|| Mathf.Abs(transform.position.y) > 3)
            Destroy(gameObject);  
    }

    void OnDestroy()
    {
        if(DestroyAction != null)
            DestroyAction();
    }

    public void setDestoryAction(Action action)
    {
        DestroyAction = action;
    }

    public void setDirection(Vector2 vector)
    {
        mDirection = vector;
    }
     

    public Vector2 Direction
    {
        get
        {
            return mDirection;
        }
        set
        {
            mDirection = value;
        }
    }

    public bool IsReflect
    {
        get
        {
            return isReflect;
        }
        set
        {
            isReflect = value;
        }
    }

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;

            if (hp <= 0)
                Explode();
        }
    }

    void Explode()
    {
        Instantiate(ExplosionEffect, gameObject.transform.position, Quaternion.identity);// as GameObject; 
        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/explosion") as AudioClip);
        Destroy(gameObject);
    }

    public void Land()
    {
        Counterdown();
        GameObject obj = Instantiate(DustEffect, new Vector2(gameObject.transform.position.x , gameObject.transform.position.y - 0.2f), Quaternion.identity) as GameObject;
        GameObject.FindWithTag("MainCamera").GetComponent<Camera>().DOShakePosition(0.5f, new Vector2(0f, 1f), 8);
    }

    void Counterdown()
    {
        if (isCounterdown)
            return;
        isCounterdown = true;
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.3f);
        mySequence.AppendCallback(() =>
        {
            Explode();
        });
    }


    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;

        if (isReflect)
        { 
            if (target.tag == "Bullet" && target.GetComponent<DamageControl>().Owner != owner)
            {
                hp--;

                if (hp > 0)
                {
                    BulletControl ctrl = target.GetComponent<BulletControl>();
                    float dir = ctrl.Direction.x * -1;
                    target.transform.localScale = new Vector2(dir, 1);
                    dir *= UnityEngine.Random.Range(0.5f, 1f);
                    float y = UnityEngine.Random.Range(-0.3f, 0.3f);
                    target.GetComponent<BulletControl>().Direction = new Vector2(dir, y).normalized;
                    target.GetComponent<DamageControl>().setOwner(gameObject.GetComponent<DamageControl>().Owner);
                    string prefabPath = "Prefab/StageScene/BossHitEffect";
                    Instantiate(Resources.Load(prefabPath), gameObject.transform.position, Quaternion.identity);
                    AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rebound") as AudioClip);
                }
                else
                {
                    Explode(); 
                    Destroy(target); 
                }  
            }
            else if (target.tag == "Player" && target.name != "CollisionDamage" && target != owner)  //CollisionDamage is the BoxCollider makes player be damaged. 
            {
                //   if (target.GetComponent<PlayerControl>().Unbeatable)
                //        return;
                 
                if (target.GetComponent<PlayerControl>().Unbeatable == false)
                {
                    target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                    HpControl hpControl = target.GetComponent<HpControl>();
                    hpControl.modifyHp(-dmgCtrl.Damage);
                    target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                    Explode();
                }  
            }
        }
        else
        {
            if (target.tag == "Player" && target.name != "CollisionDamage" && target != owner)  //CollisionDamage is the BoxCollider makes player be damaged. 
            {
                if (target.GetComponent<PlayerControl>().Unbeatable == false)
                {
                    target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                    HpControl hpControl = target.GetComponent<HpControl>();
                    hpControl.modifyHp(-dmgCtrl.Damage);
                    target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                    Explode();
                }
            }
            else if (target.tag == "Bullet")
            {
                hp--;
                if (hp <= 0)
                {
                    Explode();
                }
                else
                {
                    string prefabPath = "Prefab/StageScene/BossHitEffect";
                    Instantiate(Resources.Load(prefabPath), gameObject.transform.position, Quaternion.identity);
                    AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rebound") as AudioClip);
                }
            }
        } 
    }
}
