using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;

public class BgScript : MonoBehaviour
{
     
    public Sprite WinBackground;
    public Sprite Warnging;
    public Sprite UltimateBackground;
    private GameObject face;
    private GameObject effect;
    private GameObject winBackground;
    private bool isPlaying;

    void Start()
    {
        face = transform.FindChild("Face").gameObject;
        effect = transform.FindChild("Effect").gameObject;

        winBackground = new GameObject();
        winBackground.transform.parent = gameObject.transform;
        winBackground.transform.position = new Vector3(0, 0.5f, 0); 
        winBackground.AddComponent<SpriteRenderer>();
        winBackground.GetComponent<SpriteRenderer>().sprite = WinBackground;
        winBackground.GetComponent<SpriteRenderer>().sortingLayerName = "Bg";
        winBackground.GetComponent<SpriteRenderer>().sortingOrder = 1;
        winBackground.transform.localScale = new Vector2(2.5f,1.6f);
        
        winBackground.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    { 
    }

    public void setSprite(Sprite sprite)
    {  
        GetComponent<SpriteRenderer>().sprite = sprite; 
    }

    public void setFace(Sprite sprite)
    {
        face.transform.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void showEffect()
    {
        if (isPlaying)
            return; 
        isPlaying = true;
        face.SetActive(true);
        effect.SetActive(true);

        face.transform.position = new Vector2(8,face.transform.position.y);
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true);
        mySequence.Append(face.transform.DOMove(new Vector2(0, face.transform.position.y), 0.7f));
        mySequence.AppendInterval(0.3f);
        mySequence.Append(face.transform.DOMove(new Vector2(-8, face.transform.position.y), 0.7f));
        mySequence.AppendCallback(() => { isPlaying = false; });
      
    }

    public void stopEffect()
    {
        face.SetActive(false);
        effect.SetActive(false);
    }

    public void showWinEffect()
    {
        print("BG : Show Win Effect");
        winBackground.SetActive(true);
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(3);
        mySequence.Append(winBackground.transform.GetComponent<SpriteRenderer>().DOFade(0, 3)); 
        mySequence.AppendCallback(() =>
        {
            winBackground.SetActive(false);
        });
    }

    public void showLoseEffect()
    {
        
    }

}
