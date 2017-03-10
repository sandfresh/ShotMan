using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class HitText : MonoBehaviour
{

    public GameObject SpriteTextPrefab;

    private SpriteText spriteText ;
    private GameObject spriteTextObj;

    void Awake()
    {
        print("HitText");
        spriteTextObj = Instantiate(SpriteTextPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        spriteTextObj.transform.parent = gameObject.transform;
        spriteText = spriteTextObj.GetComponent<SpriteText>();

        setText("");
    }

	void Start()
    {
     

    }
	 
	void Update()
    { 
    }

    public void setText(string text)
    {
        spriteText.setText(text);
        spriteText.setSize(60); 
        float hitwidth = GetComponent<Image>().sprite.rect.width; 
        DOTween.Complete("spritetextscale");
        Vector2 originscale = spriteTextObj.transform.localScale; 
        spriteTextObj.transform.localScale = originscale * 1.5f;
        spriteTextObj.transform.DOScale(originscale, 0.3f).id = "spritetextscale";
        spriteTextObj.transform.localPosition = new Vector2(-((text.Length -1)*50 + hitwidth/2), 30);
    }
}
