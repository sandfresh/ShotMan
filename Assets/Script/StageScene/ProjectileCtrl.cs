using UnityEngine;
using System.Collections;

public class ProjectileCtrl : MonoBehaviour
{
    public float speed = 10;

    Vector2 mDirection = new Vector2(1, 0);
    DamageControl dmgCtrl;
     
    void Start()
    {
        dmgCtrl = GetComponent<DamageControl>(); 
    }

     
    void Update()
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

   //     Debug.Log("Target = " + target.name + ", owner = " + owner.name);

        if (target != owner && target.tag == "Player" && target.name!= "CollisionDamage")
        {
            target.GetComponent<HpControl>().modifyHp(-dmgCtrl.Damage);
            target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
            Destroy(gameObject);
        }
    }
}