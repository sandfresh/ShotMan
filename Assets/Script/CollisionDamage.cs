using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {
     
    DamageControl dmgCtrl; 
 

    void Start ()
    {
        dmgCtrl = transform.parent.gameObject.GetComponent<DamageControl>();
        dmgCtrl.setOwner(gameObject);
    }
	 
	void Update ()
	{ 
	}  

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" )
            return;

        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;
         
        if (target != owner && (target.name == "Rockman"))
        {
            if (target.GetComponent<PlayerControl>().Unbeatable == false)
            {
                target.GetComponent<HpControl>().modifyHp(-dmgCtrl.Damage);
                target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                Object obj = target.GetComponent<HpControl>().getDamageEffect();
                Instantiate(obj, gameObject.transform.position, Quaternion.identity);
            } 
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    { 
        if (collision.gameObject.tag != "Player")
            return; 
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;
         
        if (target != owner && target.name == "Rockman")
        {
            if (target.GetComponent<PlayerControl>().Unbeatable == false)
            {
                target.GetComponent<HpControl>().modifyHp(-dmgCtrl.Damage);
                target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                Object obj = target.GetComponent<HpControl>().getDamageEffect();
                Instantiate(obj, gameObject.transform.position, Quaternion.identity);
            } 
        }
    }
}
