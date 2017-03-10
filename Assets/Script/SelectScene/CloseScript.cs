using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Button>().onClick.AddListener((() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
            Application.LoadLevel(YS.Title);
        }));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
