using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DamageControl))]
[RequireComponent(typeof(Collider2D))]
public class PushControl : MonoBehaviour 
{
    DamageControl dmgCtrl;
    int direction = 1;
	 
	void Start () 
	{
        dmgCtrl = GetComponent<DamageControl>();
    }
	
    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target.tag == "Player")
        {
            bool isRockman = target.GetComponent<PlayerControl>().isRockMan();

            if (isRockman)
            { 
                if (target.GetComponent<PlayerControl>().Unbeatable == false)
                { 
                    target.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * direction, 0));
                    target.GetComponent<PlayerControl>().setKnok(true); 
                } 
            }
        }
    }

    public void setDir(int dir)
    {
        direction = dir;
    }
}