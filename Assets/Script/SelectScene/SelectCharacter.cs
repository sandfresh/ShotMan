using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SelectCharacter : MonoBehaviour
{

	// Use this for initialization
    public int StageIndex = 0;
    private int page;

    private float nameSpritePosX;
    private float characterSpritePosX;

    void Start ()
    { 
        characterSpritePosX = -200;
        nameSpritePosX = -284;
        GlobalVariable.getInstance().setCurrentStage(0);
        setContent(); 
	}


    /*
    *  When the page index have been changed,
    *  show/hide the name & character image.
    */

    void Update ()
    {
        if (GlobalVariable.getInstance().getCurrentPage() != page)
        {
            setContent();
        } 
    }
     

    public void setContent()
    { 
        GameObject lockobj = transform.Find("lock").gameObject;
        GameObject unlockobj = transform.Find("unlock").gameObject;

        page = GlobalVariable.getInstance().getCurrentPage();

        if (page * transform.parent.GetComponent<SelectButton>().buttonArray.Length + StageIndex < GlobalVariable.getInstance().getTotalStage())
        {
            lockobj.SetActive(false);
            unlockobj.SetActive(true);
              
            string path = "Image/Stage/";
            Image charaimg = unlockobj.transform.Find("charaImg").GetComponent<Image>();
            //  Sprite charaSprite = Resources.Load<Sprite>(path + "stageBtn" + StageIndex);
            Sprite charaSprite = Load(path + "stageSheet", "stageBtn" + StageIndex);
            charaimg.sprite = charaSprite;

            Image nameimg = unlockobj.transform.Find("nameImg").GetComponent<Image>();
         //   Sprite nameSprite = Resources.Load<Sprite>(path + "name" + StageIndex);
            Sprite nameSprite = Load(path + "stageSheet", "name" + StageIndex);
            nameimg.sprite = nameSprite; 
        }
        else
        {
             lockobj.SetActive(true);
             unlockobj.SetActive(false);

            lockobj.transform.FindChild("lockText").GetComponent<Text>().text = LangManager.Instance.getString("downloadover") +
                                                 (((page * 4 + StageIndex +1) - GlobalVariable.getInstance().getTotalStage()) *5000)
                                                 + LangManager.Instance.getString("open");
        } 
    }

    public static Sprite Load(string fileName, string spriteName)
    { 
        // Resoucesから対象のテクスチャから生成したスプライト一覧を取得
        Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
        // 対象のスプライトを取得
        return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
    }

    public void OnClick(GameObject sender)
    {
        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
        int page = GlobalVariable.getInstance().getCurrentPage();
        selectCharacter(StageIndex + page * transform.parent.GetComponent<SelectButton>().buttonArray.Length);  
    }

    public void selectCharacter(int number)
    {
        if (GlobalVariable.getInstance().getTotalStage() < (number+1))
            return;
        if (GlobalVariable.getInstance().getCurrentStage() == StageIndex)
            return;


        GameObject charaobj = GameObject.Find("characterImage");
        Image characterimage = charaobj.GetComponent<Image>();
        string path = "Image/Stage2/";
        //    Sprite newSprite = Resources.Load<Sprite>(path+"bossArt"+number);
        Sprite newSprite = Load(path, "bossArt" + number);
        GameObject imageobj = GameObject.Find("nameImage");
        Image nameimage = imageobj.GetComponent<Image>();
        string path2 = "Image/Stage/";
        //  Sprite nameImageNew = Resources.Load<Sprite>(path2 + "name" + number);
        Sprite nameImageNew = Load(path2, "name" + number);

        if (newSprite)
        {
            GlobalVariable.getInstance().setCurrentStage(number);

            GameObject.Find("levelBtn0").SendMessage("showLevelList",false);

            DOTween.Complete("characterSequence");
            DOTween.Complete("nammeSequence"); 

            Vector2 charaPos = charaobj.GetComponent<RectTransform>().anchoredPosition;  
            characterimage.sprite = newSprite;
            characterimage.color = new Color(1,1,1,0.5f);
            characterimage.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterSpritePosX, charaPos.y);
            Sequence characterSequence = DOTween.Sequence();
            characterSequence.id = "characterSequence";
            characterSequence.Append(characterimage.GetComponent<RectTransform>().DOMoveX(500, 0.3f).From()); 
            characterSequence.Join(characterimage.DOFade(1, 0.3f));
             

            Vector2 namePos = imageobj.GetComponent<RectTransform>().anchoredPosition;
            nameimage.sprite = nameImageNew;
            nameimage.color = new Color(1,1,1,0.5f);
            nameimage.GetComponent<RectTransform>().anchoredPosition = new Vector2(nameSpritePosX,namePos.y);
            Sequence nameSequence = DOTween.Sequence();
            nameSequence.id = "nameSequence";
            nameSequence.Append(nameimage.GetComponent<RectTransform>().DOMoveX(-300 , 0.3f).From()); 
            nameSequence.Join(nameimage.DOFade(1, 0.3f)); 
        }
        else
        {
            Debug.LogError("Sprite not found", this);
        }

        int level = GlobalVariable.getInstance().getStageProgress(StageIndex);
        GameObject root = GameObject.Find("LevelButton");
        LevelBtnRootScript script = root.GetComponent<LevelBtnRootScript>();
        List<GameObject> list = script.getLevelBtnList();
        list[0].GetComponent<SelectLevelScript>().setLevel(level); 
    }

    public int getStageIndex()
    {
        return StageIndex;
    }

}
