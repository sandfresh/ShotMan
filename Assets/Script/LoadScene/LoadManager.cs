using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour
{

    public Sprite[] CharacterArray;
    public Sprite[] NameArray;
    public Sprite[] LoadStageArray;
    public RuntimeAnimatorController[] ControllerArray;

    void Start ()
    { 
        GameObject nameobj = GameObject.Find("NameSprite");
        GameObject characterbigobj = GameObject.Find("CharacterBig");
        GameObject loadStage = GameObject.Find("LoadStage");
        GameObject characterAnim = GameObject.Find("CharacterAnimation");

        int stage = GlobalVariable.getInstance().getCurrentStage();

        nameobj.GetComponent<NameSprite>().setSprite(NameArray[stage]);
        characterbigobj.GetComponent<CharacterBig>().setSprite(CharacterArray[stage]);
        loadStage.GetComponent<LoadStage>().setSprite(LoadStageArray[stage]);

        characterbigobj.GetComponent<CharacterBig>().showMoveInEffect();
        nameobj.GetComponent<NameSprite>().showMoveInEffect();

        characterAnim.GetComponent<CharacterAnimation>().setAnimControlller(ControllerArray[stage]);
    }
	
	// Update is called once per frame
	void Update ()
    { 
	}


}
