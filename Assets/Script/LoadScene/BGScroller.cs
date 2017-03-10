using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BGScroller : MonoBehaviour
{ 

    public GameObject[] ScrollBgs; 
    
    // Use this for initialization  

    void Start()
    {
        GameObject bg = ScrollBgs[0];
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true);
        mySequence.Append(bg.transform.DOMove(new Vector2(-8, bg.transform.position.y), 1).SetEase(Ease.Linear).OnComplete(
            () => { bg.transform.position = new Vector2(8, bg.transform.position.y); }));

        mySequence.Append(bg.transform.DOMove(new Vector2(0, bg.transform.position.y), 1).SetEase(Ease.Linear));
        mySequence.SetLoops(-1);

        GameObject bg2 = ScrollBgs[1];
        Sequence mySequence2 = DOTween.Sequence();
        mySequence2.SetUpdate(true);
        mySequence2.Append(bg2.transform.DOMove(new Vector2(0, bg2.transform.position.y), 1).SetEase(Ease.Linear));
        mySequence2.Append(bg2.transform.DOMove(new Vector2(-8, bg2.transform.position.y), 1).SetEase(Ease.Linear).SetEase(Ease.Linear).OnComplete(
            () => { bg2.transform.position = new Vector2(8, bg2.transform.position.y); }));
        mySequence2.SetLoops(-1); 

    }
     

    void Update()
    { 
    }
}
