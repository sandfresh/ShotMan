using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

	// Use this for initialization

    public AudioClip WallSE;
    AudioSource audio;

    void Start ()
    {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
     
}
