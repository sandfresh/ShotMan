using UnityEngine;
using System.Collections;

public class RotationCtr : MonoBehaviour {

	// Use this for initialization
    public float Speed = 200f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    transform.Rotate(new Vector3(0,0,Speed * Time.deltaTime));
	}
}
