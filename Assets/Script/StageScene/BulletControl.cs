using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour 
{
    public float speed = 10;

    Vector2 mDirection = new Vector2(1, 0);
    DamageControl dmgCtrl; 
    // Use this for initialization
   
    void Start () 
	{
        dmgCtrl = GetComponent<DamageControl>();
        GlobalVariable.getInstance().ShotNumber = GlobalVariable.getInstance().ShotNumber + 1;
	}
	
	// Update is called once per frame
	void Update ()
	{   
        Vector2 moveDir = mDirection * Time.deltaTime * speed;
        transform.Translate(moveDir);

        if (Mathf.Abs(transform.position.x) > 5 || Mathf.Abs(transform.position.y) > 3)
            Destroy(gameObject);
	}

    public void setDirection(int dir)
    {
        mDirection.x = dir;
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

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public void OnTriggerEnter2D(Collider2D collision)
    { 
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;

        if (target.tag == "Player" && target.name!= "CollisionDamage" && owner != target)  //CollisionDamage is that Box Collider cause player damage. 
        {
            HpControl hpControl = target.GetComponent<HpControl>();
            if (target.GetComponent<PlayerControl>().Unbeatable == false)
            {
                target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                hpControl.modifyHp(-dmgCtrl.Damage); 
            }
        
 
            string prefabPath = "Prefab/StageScene/BossHitEffect";
            Instantiate(Resources.Load(prefabPath), gameObject.transform.position, Quaternion.identity);
 

            //atk.transform.parent = gameObject.transform;
            //atk.transform.localScale = new Vector2(1, 1);// facingRight ? 1 : -1, 1);
            //atk.GetComponent<DamageControl>().setOwner(gameObject);
            //atk.GetComponent<PushControl>().setDir(facingRight ? 1 : -1); 
            Destroy(gameObject);
        }
        else if(target.tag == "Bomb")
        {
            BombControl bombctr = target.transform.GetComponent<BombControl>();
            if (bombctr.IsReflect == false)
            {
                bombctr.HP = bombctr.HP - 1;
                Destroy(gameObject);
            } 
        }
    }
}