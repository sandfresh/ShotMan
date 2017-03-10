using UnityEngine;
using System.Collections;

public class LoadStage : MonoBehaviour {

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
}
