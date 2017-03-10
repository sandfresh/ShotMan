using System;
using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {

	// Use this for initialization

    private int count = 0;
    private float time = 0;
    private bool isTimerStart = false;

	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    { 
    }

    public void setAnimControlller(RuntimeAnimatorController controller)
    {
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = controller;
    }
     

    public void loadNextLevel()
    {
        Application.LoadLevel(YS.Stage);
    }
}
