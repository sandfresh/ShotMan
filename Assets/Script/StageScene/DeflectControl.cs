using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class DeflectControl : MonoBehaviour 
{

    // Use this for initialization
   
    void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	} 

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target.tag == "Bullet")
        {
            DamageControl dmgCtr = GetComponent<DamageControl>();
            BulletControl ctrl = target.GetComponent<BulletControl>();
            target.GetComponent<DamageControl>().setOwner(dmgCtr.Owner);
            float dir = ctrl.Direction.x * -1;
            target.transform.localScale = new Vector2(dir, 1);  
            dir *= Random.Range(0.5f, 1f);
            float y = Random.Range(0f, 1f);
            target.GetComponent<BulletControl>().Direction = new Vector2(dir, y).normalized;

            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("/Sound/rebound") as AudioClip);
        }
    }
}