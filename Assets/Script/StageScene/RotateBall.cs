using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotateBall : MonoBehaviour {

	// Use this for initialization

    public Transform Ball; 

	void Start ()
	{ 
	}
	 
	void Update ()
	{
	     transform.Rotate(new Vector3(0, 0, 0.5f));  
	}

    public void MoveIn(Action act = null)
    {
        for (int i = 0; i < 16; i++)
        {
            int temp = i;
            var ball = Instantiate(Ball);
            GameObject ballAni = ball.transform.Find("BallAnimation").gameObject; 
            ball.transform.parent = gameObject.transform;
            ball.transform.localPosition = new Vector3(0, 0, 0);
            ball.eulerAngles = new Vector3(0, 0, 22.5f * i);  
          
            Sequence mySequence = DOTween.Sequence(); 
            if( i % 2 == 0)
                mySequence.Append(ballAni.transform.DOLocalMove(new Vector3(4.5f, 0, 0), 1f).From());
            if (i % 2 == 1)
                mySequence.Append(ballAni.transform.DOLocalMove(new Vector3(3.5f, 0, 0), 1f).From());
            mySequence.AppendCallback(() =>
            {
                Destroy(ball.gameObject);
                if (temp == 15)
                {
                    act();
                    Destroy(gameObject);
                }
                   
            });
        } 
    }

    public void MoveOut(Action act = null)
    {
        for (int i = 0; i < 16; i++)
        {
            int temp = i;
            var ball = Instantiate(Ball);
            GameObject ballAni = ball.transform.Find("BallAnimation").gameObject;
            ball.transform.parent = gameObject.transform;
            ball.transform.localPosition = new Vector3(0, 0, 0);
            ball.eulerAngles = new Vector3(0, 0, 22.5f * i);

           
            Sequence mySequence = DOTween.Sequence();
            if (i % 2 == 0)
                ballAni.transform.DOLocalMove(new Vector3(4.5f, 0, 0), 6f);
            if (i % 2 == 1)
                ballAni.transform.DOLocalMove(new Vector3(3.5f, 0, 0), 6f);

            mySequence.Append(ballAni.GetComponent<SpriteRenderer>().DOFade(0,4));
            mySequence.AppendCallback(() =>
            {
                Destroy(ball.gameObject);
                if (temp == 15)
                {
                    act();
                    Destroy(gameObject);
                }
            });
        }
    }
}
