using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBtnRootScript : MonoBehaviour {

    // Use this for initialization

    public List<GameObject> ButtonList = new List<GameObject>();

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<GameObject> getLevelBtnList()
    {
        return ButtonList;
    }
}
