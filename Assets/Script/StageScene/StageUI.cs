using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class StageUI : MonoBehaviour

{

    public Sprite Win;
    public Sprite Lose;
 //   public Sprite Hit;
    public Sprite Warning;
    public GameObject HitPlayer;
    public GameObject HitEnemy;

    private GameObject win;
    private GameObject lose;
    private GameObject warning;
 //   private GameObject playerHit;
 //   private GameObject bossHit;
 //   private GameObject playerHitText;
 //   private GameObject bossHitText;
   

    private int playerCombo;
    private int bossCombo;

    private SpriteText playerSpriteText;

    void Start ()
    {  
        win = new GameObject();
        Image winImage = win.AddComponent<Image>();
        win.GetComponent<RectTransform>().SetParent(gameObject.transform);
        winImage.sprite = Win;
        winImage.SetNativeSize();
        win.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        win.SetActive(false);

        lose = new GameObject();
        Image loseImage = lose.AddComponent<Image>();
        lose.GetComponent<RectTransform>().SetParent(gameObject.transform);
        loseImage.sprite = Lose;
        lose.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        loseImage.SetNativeSize();
        lose.SetActive(false);  

        warning = new GameObject();
        Image warningImage = warning.AddComponent<Image>();
        warning.GetComponent<RectTransform>().SetParent(gameObject.transform);
        warningImage.sprite = Warning;
        warning.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        warningImage.SetNativeSize();
        warning.transform.localScale = new Vector2(0.8f,0.8f);
        warning.SetActive(false);


        //playerHit = new GameObject();
        //Image hitImage = playerHit.AddComponent<Image>();
        //playerHit.GetComponent<RectTransform>().SetParent(gameObject.transform);
        //hitImage.sprite = Hit;
        //hitImage.SetNativeSize();
        //playerHit.SetActive(false);
        //playerHit.transform.position = new Vector2(80+ hitImage.rectTransform.rect.width/2, 300);

        //playerHitText = transform.FindChild("PlayerComboText").gameObject; 
        //playerHitText.SetActive(false);

        //bossHit = new GameObject();
        //hitImage = bossHit.AddComponent<Image>();
        //bossHit.GetComponent<RectTransform>().SetParent(gameObject.transform); 
        //hitImage.sprite = Hit;
        //hitImage.SetNativeSize();
        //bossHit.SetActive(false);
        //bossHit.transform.position = new Vector2(720 - hitImage.rectTransform.rect.width/2, 300);

        //bossHitText = transform.FindChild("BossComboText").gameObject; 
        //bossHitText.SetActive(false); 
        HitPlayer.SetActive(false);
        HitEnemy.SetActive(false);
    }

    public void ShowWarning()
    {
        warning.SetActive(true);
        warning.GetComponent<Image>().color = new Color(1,1,1,1);
        warning.GetComponent<Image>().DOFade(0, 0.7f).SetLoops(5, LoopType.Yoyo).OnComplete(
            () => {warning.SetActive(false); });
    }
	 
    public void ShowWin()
    {
        win.SetActive(true);

        win.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        win.transform.localScale = new Vector2(4,4);

        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true); 
        mySequence.Append(win.transform.DOScale(1, 0.5f).SetEase(Ease.InExpo));
        mySequence.Join(win.GetComponent<Image>().DOFade(1, 0.5f));
        mySequence.AppendInterval(3);
        mySequence.Append(win.transform.DOScale(3, 0.8f));
        mySequence.Join(win.GetComponent<Image>().DOFade(0, 0.8f));
        mySequence.AppendCallback(() =>
        {
            win.SetActive(false);
        });
    }

    public void ShowLose()
    {
        lose.SetActive(true);

        win.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        win.transform.localScale = new Vector2(4, 4);

        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true); 
        mySequence.Append(lose.transform.DOScale(1, 0.5f).SetEase(Ease.InExpo));
        mySequence.Join(lose.GetComponent<Image>().DOFade(1, 0.5f));
        mySequence.AppendInterval(3);
        mySequence.Append(lose.transform.DOScale(3, 0.8f));
        mySequence.Join(lose.GetComponent<Image>().DOFade(0, 0.8f));
        mySequence.AppendCallback(() =>
        {
            lose.SetActive(false);
        });
    }


    private void setPlayerHit(int value)
    {
        if (value > 0)
        {
            HitPlayer.SetActive(true);
            //     playerHitText.SetActive(true);

            //    Text text = playerHitText.GetComponent<Text>();
            //    text.text = "" + value;

            //   playerHitText.transform.position = new Vector2(playerHit.transform.position.x - text.preferredWidth, playerHit.transform.position.y);
            HitPlayer.GetComponent<HitText>().setText(""+value);
            DOTween.Kill("playerhit"); 
            Sequence mySequence = DOTween.Sequence();
            mySequence.id = "playerhit"; 
            mySequence.AppendInterval(2);
            mySequence.AppendCallback(() =>
            {
                HitPlayer.SetActive(false);
              //  playerHitText.SetActive(false);
            });
        }
        else
        {
            DOTween.Kill("playerhit");
            HitPlayer.SetActive(false);
            //playerHitText.SetActive(false);
        }
    }

    private void setBossHit(int value)
    {
        if (value > 0)
        {

            HitEnemy.SetActive(true);
            //bossHitText.SetActive(true);

            //Text text = bossHitText.GetComponent<Text>();
            //text.text = "" + value;

            //bossHitText.transform.position = new Vector2(bossHit.transform.position.x - text.preferredWidth, bossHit.transform.position.y);
            HitEnemy.GetComponent<HitText>().setText("" + value);
            DOTween.Kill("bosshit");
            Sequence mySequence = DOTween.Sequence();
            mySequence.id = "bosshit"; 
            mySequence.AppendInterval(2);
            mySequence.AppendCallback(() =>
            {
                HitEnemy.SetActive(false);
             //   bossHitText.SetActive(false);
            });
        }
        else
        {
            DOTween.Kill("bosshit");
            HitEnemy.SetActive(false);
       //     bossHitText.SetActive(false);
        }
     
    }


    public int PlayerCombo
    {
        set
        {
            playerCombo = value;
            setPlayerHit(value);
        }

        get
        {
            return playerCombo; 
        }
    }

    public int BossCombo
    {
        set
        {
            bossCombo = value;
            setBossHit(value);
        }

        get
        {
            return bossCombo; 
        }
    }
}
