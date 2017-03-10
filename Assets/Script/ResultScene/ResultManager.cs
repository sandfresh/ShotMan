using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{

    public Sprite LoseBg;
    public Sprite WinBG;
    public Sprite ASprite;
    public Sprite BSprite;
    public Sprite CSprite;
    public Sprite SSprite; 

    public Text timeText;
    public Text hpText;
    public Text comboText;
    public Text hitText;
    public Text rankText;
    public Text commentText;
    public Image rankImage;
    public Image winnerImage;
    public Image resultBg;

    private bool isResultShow; 
   
    private List<Text> textList;

    private Text logText;
    void Start ()
	{
       // SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        //resultBg = gameObject.transform.FindChild("ResultBg").GetComponent<Image>();

        //timeText = transform.FindChild("TimeText").GetComponent<Text>();
        //hpText = transform.FindChild("RemainHpText").GetComponent<Text>();
        //comboText = transform.FindChild("ComboText").GetComponent<Text>();
        //hitText = transform.FindChild("HitText").GetComponent<Text>();
        //rankText = transform.FindChild("RankText").GetComponent<Text>();
        //commentText = transform.FindChild("CommentText").GetComponent<Text>();
        //winnerImage = transform.FindChild("WinnerImage").GetComponent<Image>();
        //rankImage = transform.FindChild("RankImage").GetComponent<Image>();
         
        rankImage.enabled = false;
        winnerImage.enabled = false;

        textList = new List<Text>(); 
        textList.Add(timeText);
        textList.Add(hpText);
        textList.Add(comboText);
        textList.Add(hitText);
        textList.Add(commentText);
        textList.Add(rankText);

        foreach (var text in textList)
        {
            text.enabled = false;
        }
        
        if (GlobalVariable.getInstance().Hp > 0)
        { 
            resultBg.sprite = WinBG;
            resultBg.color = Color.black;
            winnerImage.enabled = true;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(winnerImage.transform.DOScale(3, 1).From());
            mySequence.Join(winnerImage.transform.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0),1).From());
            mySequence.AppendCallback(() =>
            {
                 showResult();
            });
            mySequence.AppendInterval(2);
            mySequence.Append(winnerImage.transform.DOScale(3, 1));
            mySequence.Join(winnerImage.transform.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1)); 
        }
        else
        {
            resultBg.sprite = LoseBg;
            showResult();
        }
    }

    void Update ()
    { 
	} 

    public void showResult()
    {
        resultBg.color = Color.white;
 
        int remainhp = GlobalVariable.getInstance().Hp;
        int shotNumber = GlobalVariable.getInstance().ShotNumber;
        shotNumber = shotNumber > 0 ? shotNumber : 1;
        float accuracy = (((float)GlobalVariable.getInstance().HitNumber / shotNumber) * 100);

        timeText.text = LangManager.Instance.getString("time") + "    " + GlobalVariable.getInstance().GameTime.ToString("f1")+" sec";
        hpText.text = LangManager.Instance.getString("remainhp") + "    " + GlobalVariable.getInstance().Hp.ToString();
        comboText.text = LangManager.Instance.getString("combo") + "    " + GlobalVariable.getInstance().MaxCombo.ToString(); 
        hitText.text = LangManager.Instance.getString("accuracy") + "    " + accuracy.ToString("f1")+" %";

        string comment2 = "";
        if (remainhp > 0)
        {
            comment2 = LangManager.Instance.getString("wincomment"+ UnityEngine.Random.Range(1,3));
        }   
        else
        {
            comment2 = LangManager.Instance.getString("losecomment1");
        }
        commentText.text = comment2;

		foreach (var text in textList)
		{
			text.enabled = true;
			text.color = new Color(1, 1, 1, 0);
		 	text.DOColor(new Color(1, 1, 1, 1), 1);

            Sequence textSequence = DOTween.Sequence();
            textSequence.Append(text.transform.DOMoveX(800, 1).From());
            textSequence.Join(text.DOColor(new Color(1, 1, 1, 1), 1));
        }

        rankImage.enabled = true;
        if (GlobalVariable.getInstance().Hp > 0)
        {
            if (accuracy > 90 && remainhp > 20)
                rankImage.sprite = SSprite;
            else if (accuracy > 80 && remainhp > 10)
            {
                rankImage.sprite = ASprite;
            }
            else
            {
                rankImage.sprite = BSprite;
            }
        }
        else
        {
            rankImage.sprite = CSprite;
        }
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendCallback(() =>
        {
            rankImage.enabled = false;
        });
        mySequence.Append(rankImage.transform.DOScale(3, 0.1f));
        mySequence.AppendInterval(0.5f);
        mySequence.AppendCallback(() =>
        {
            rankImage.enabled = true;
        });
        mySequence.Append(rankImage.transform.DOScale(1, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Stamp") as AudioClip);
        }));
        mySequence.AppendCallback(() =>
        {
            isResultShow = true;
        });

        transform.FindChild("NextSceneBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            print("Click");
            
            if (isResultShow == true)
                Application.LoadLevel(YS.Title);

        }));
    } 

}
