using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SelectLevelScript : MonoBehaviour
{

	 
    static private bool isShow = false;
    static private bool isPlaying = false;
    private List<GameObject> buttonList ; 
    public int Level = 0;

    private void Awake()
    {
        var root = GameObject.Find("LevelButton");
        LevelBtnRootScript script = root.GetComponent<LevelBtnRootScript>();
        buttonList = script.getLevelBtnList();
    }

    void Start ()
    {  
        for (int i = 0; i < buttonList.Count; i++)
        { 
           buttonList[i].transform.Find("lockImage").gameObject.SetActive(false);
        }

        GetComponent<Button>().onClick.AddListener((() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
            if (isPlaying)
                return;
            if (isShow)
            {
                int currentStage = GlobalVariable.getInstance().getCurrentStage();
                GlobalVariable.getInstance().CurrentLevel = Level;
                this.transform.SetAsLastSibling();
            }
            showLevelList(!isShow);
        }));
         
        setLevel(GlobalVariable.getInstance().getStageProgress(0));
    }
	
	 
	void Update ()
    { 

    }
     
    public void showLevelList(bool isshow)
    {
        if (isPlaying)
            return;
        isShow = isshow;
        if (isShow)
        { 
            int progress = GlobalVariable.getInstance().getStageProgress(GlobalVariable.getInstance().getCurrentStage()); 

            for (int i = 0; i < buttonList.Count; i++)
            {
                GameObject btnobj = buttonList[i];
                Vector3 pos = btnobj.transform.position;
 
                if (i <= progress)
                {
                    btnobj.GetComponent<Button>().enabled = true;
                    buttonList[i].transform.Find("lockImage").gameObject.SetActive(false);
                }
                else
                {
                    btnobj.GetComponent<Button>().enabled = false;
                    buttonList[i].transform.Find("lockImage").gameObject.SetActive(true);
                }

                RectTransform rt = btnobj.GetComponent<RectTransform>();
                float height = rt.rect.height;

                GameObject canvanobj = GameObject.Find("SelectUI").gameObject;
                float value = canvanobj.GetComponent<Canvas>().scaleFactor;
                height *= value;

                btnobj.transform.DOMove(new Vector3(pos.x, pos.y + height * i, 0), 0.3f)
                    .OnStart(new TweenCallback(() =>
                    {
                        if (i == 0)
                            isPlaying = true;
                    })).OnComplete(new TweenCallback(() =>
                    {
                        if (i == 0)
                            isPlaying = false;
                    }));

            }
        }
        else
        {
            int currentstage = GlobalVariable.getInstance().getCurrentStage();
            for (int i = 0; i < buttonList.Count; i++)
            { 
                GameObject btnobj = buttonList[i];
                btnobj.GetComponent<Button>().enabled = false;
                btnobj.transform.DOMove(buttonList[0].transform.position, 0.3f).OnComplete(
                    new TweenCallback(() =>
                    { 
                        btnobj.transform.Find("lockImage").gameObject.SetActive(false);
                        if(GlobalVariable.getInstance().CurrentLevel == Level)
                            btnobj.GetComponent<Button>().enabled = true; 
                    }));
            }

        }
    }

    public void setLevel(int index)
    {  
        buttonList[index].transform.SetAsLastSibling();
        buttonList[index].GetComponent<Button>().enabled = true;
        int currentStageIndex = GlobalVariable.getInstance().getCurrentStage();
        int level = buttonList[index].GetComponent<SelectLevelScript>().Level;
        print("SetLevel:"+Level); 
        GlobalVariable.getInstance().CurrentLevel = level;
    } 
}
