using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NatrueBomb : MonoBehaviour
{

    public GameObject UpwardExploision;
    
    private Action DestroyAction;
    private GameObject owner;

    void Start ()
    {
        
    }
	
	
	void Update ()
    {
	
	}

    public void onAnimationStop()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.3f);
        mySequence.AppendCallback(() =>
        {
            Explode(); 
        }); 
    } 

    public void setExplode()
    {
        GetComponent<Animator>().SetTrigger("Explode");
    }

    void Explode()
    {
        GameObject exploision = Instantiate(UpwardExploision, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.4f), Quaternion.identity) as GameObject;
        exploision.GetComponent<DamageControl>().setOwner(owner);
        Destroy(gameObject);
    }

    public void setOwner(GameObject obj)
    {
        owner = obj;
    }


    void OnDestroy()
    {
        if (DestroyAction != null)
            DestroyAction();
    }

    public void setDestoryAction(Action action)
    {
        DestroyAction = action;
    }

 


}
