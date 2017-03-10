using UnityEngine;
using System.Collections;

public class WindowBtn : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void onClick()
    {
        print("OnClick");
        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
    }
}
