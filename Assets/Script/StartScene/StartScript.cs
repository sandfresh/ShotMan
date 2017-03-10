using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {
    
	void Start ()
    { 
        GetComponent<Button>().onClick.AddListener((() =>
        {
            AudioClip clickSE = Resources.Load("Sound/Click") as AudioClip;
            AudioMgr.Instance.Audio.PlayOneShot(clickSE);
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(0.5f);
            mySequence.AppendCallback(() =>
            {
               Application.LoadLevel(YS.Select);
            });
           
        }));
    }
	
	void Update ()
    {
	
	}
}
