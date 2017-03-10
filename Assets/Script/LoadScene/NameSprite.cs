using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NameSprite : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void setSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite; 
    }

    public void showMoveInEffect()
    {
        Vector3 charaPos = transform.position;
        transform.DOMove(new Vector3(charaPos.x - 6, charaPos.y, 0), 0.6f).From().SetEase(Ease.InSine); ;
    }
}
