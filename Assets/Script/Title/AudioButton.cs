using UnityEngine;
using System.Collections;

public class AudioButton : MonoBehaviour {

	// Use this for initialization
    private bool isOn = true;
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void setOnOff()
    {
        isOn = !isOn;
        AudioMgr.Instance.setOn(isOn);
    }
}
