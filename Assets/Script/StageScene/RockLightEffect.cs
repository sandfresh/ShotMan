using UnityEngine;
using System.Collections;

public class RockLightEffect : MonoBehaviour {
     
	void Start ()
    {
	
	}
	 
	void Update ()
    {
	    gameObject.transform.Rotate(new Vector3(0,0,2));
	}
}
