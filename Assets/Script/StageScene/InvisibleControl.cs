using UnityEngine;
using System.Collections;

public class InvisibleControl : MonoBehaviour 
{
    
    public bool selfDestruct = true;
    DamageControl dmgCtrl;
    public float time = 0;

    public float DestoryTime
    {
        set { destorytime = value; }
    }

    void Start()
    {
        dmgCtrl = GetComponent<DamageControl>();
    }

    float destorytime = 0.8f;
	// Update is called once per frame
	void Update () 
	{
        if (!selfDestruct)
            return;

        time += Time.deltaTime;
        if (time >= destorytime)
        {
            Destroy(gameObject);
        }
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;

    //    Debug.Log("Target = " + target + ", owner = " + owner);

        if (target != owner && target.tag == "Player" && target.name != "CollisionDamage")
        {
            target.GetComponent<HpControl>().modifyHp(-dmgCtrl.Damage);
            if (selfDestruct)
                Destroy(gameObject);
        }
    }
}