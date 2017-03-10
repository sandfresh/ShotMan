using UnityEngine;
using System.Collections;

public class UpwardExplosionCtr : MonoBehaviour {

   

    DamageControl dmgCtrl;

    void Start ()
    { 
        dmgCtrl = GetComponent<DamageControl>();
    }
	
	 
	void Update ()
    {
	
	}


    public void OnTriggerEnter2D(Collider2D collision)
    { 
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;

        if (target.tag == "Player" && target.name != "CollisionDamage" && target != owner)  //CollisionDamage is the BoxCollider makes player be damaged. 
        {
            if (target.GetComponent<PlayerControl>().Unbeatable == false)
            {
                HpControl hpControl = target.GetComponent<HpControl>();
                hpControl.modifyHp(-dmgCtrl.Damage);
            } 
        }
    }
}
