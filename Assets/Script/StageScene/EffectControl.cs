using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(DamageControl))]
[RequireComponent(typeof(Collider2D))]
public class EffectControl : MonoBehaviour
{
    DamageControl dmgCtrl;
    public bool destroyOnFinish = true;
    public bool destryOnContact = false;
     

    void Start()
    {
        dmgCtrl = GetComponent<DamageControl>();

        AudioClip effectSE = null;
        if (gameObject.tag == "Knife")
            effectSE = (Resources.Load("Sound/missAtk") as AudioClip);
        else if (gameObject.tag == "Push")
            effectSE = (Resources.Load("Sound/Wind") as AudioClip);
        else if (gameObject.tag == "Bomb")
        {
            print("PlayBombSound");
            effectSE = (Resources.Load("Sound/explosion") as AudioClip);
        }
       

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.1f);
        mySequence.AppendCallback(() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(effectSE);
        });
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        GameObject owner = dmgCtrl.Owner;
         
        if (target != owner && target.tag == "Player" && target.name != "CollisionDamage")
        {
            if (target.GetComponent<PlayerControl>().Unbeatable == false)
            {
                target.GetComponent<PlayerControl>().setUnbeateable(gameObject);
                target.GetComponent<HpControl>().modifyHp(-dmgCtrl.Damage); 
                if (destryOnContact)
                    Destroy(gameObject);


                if (gameObject.tag == "knife")
                {
                    AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/knife") as AudioClip);
                }
                if (gameObject.tag == "push")
                {
                    AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/explosion") as AudioClip);
                }

                var stageUI = GameObject.Find("StageUI").GetComponent<StageUI>();
                stageUI.BossCombo = stageUI.BossCombo + 1;
            } 
        } 
    }
     

    public  void onAnimatoinFinished()
    {
        if (destroyOnFinish)
            Destroy(gameObject); 
    }
}