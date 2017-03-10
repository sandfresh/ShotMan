using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour {

    // Use this for initialization

    public Button[] buttonArray;

    void Start ()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<SelectCharacter>().StageIndex = i;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}


}
