using UnityEngine;
using System.Collections;

public class RotataTestScript : MonoBehaviour {

	// Use this for initialization.
       
    public float speed = 1;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, speed));
    }
}
