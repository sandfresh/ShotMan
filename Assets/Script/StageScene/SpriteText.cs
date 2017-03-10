using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpriteText : MonoBehaviour
{ 
    
    public List<Sprite> SpriteList;
    public bool isUI = true;

    private string fontName;
    private string fontPath;
  
    private List<GameObject> spriteTextList;  
  
    private Vector2 originSize ; 
    private string currentText = "";
    private int currentSize = 30;

    public enum TextType
    {
        Image,Sprite
    }

    void Awake()
    { 
        spriteTextList = new List<GameObject>();
        gameObject.transform.localScale = Vector3.one; 
        originSize = new Vector2(SpriteList[0].rect.width, SpriteList[0].rect.height);


        setText("");
        setSize(30);
    }

    void Start()
    { 
    }


    void Update()
    { 
    }

    public void setText(string text)
    {
        currentText = text;
        if (isUI)
        {
            setImage(currentText);
        }
        else
        {
            setSprite(currentText);
        } 
    }

    public void setSize(int value)
    {
        currentSize = value;
       
        float scale = currentSize/originSize.y;
        gameObject.transform.localScale = Vector3.one * scale;
    }

    private void setSprite(string text)
    {

        for (int i = 0; i < spriteTextList.Count; i++)
        {
            spriteTextList[i].SetActive(false);
        }

        var charArray = text.ToCharArray();

        if (spriteTextList.Count < charArray.Length)
        {
            int diff = (charArray.Length - spriteTextList.Count);
            for (int i = 0; i < diff; i++)
            {
                GameObject spriteText = new GameObject();
                spriteText.transform.parent = gameObject.transform;
                spriteTextList.Add(spriteText);
                spriteText.transform.localScale = Vector3.one;
                float spacing = SpriteList[0].rect.width * (spriteTextList.Count - 1);
                spriteText.transform.localPosition = new Vector2(spacing, 0);
                spriteText.AddComponent<SpriteRenderer>();
            }
        }

        for (int i = 0; i < charArray.Length; i++)
        {
            var spriteText = spriteTextList[i];
            char character = charArray[i];
            spriteText.SetActive(true);
            spriteText.name = "" + character;
            var renderer = spriteText.GetComponent<SpriteRenderer>();

            foreach (var sprite in SpriteList)
            {
                if (sprite.name.Contains("" + character))
                {
                    renderer.sprite = sprite;
                    break;
                }
            }
        }
    }

    private void setImage(string text)
    {
        for (int i = spriteTextList.Count - 1; i >=0; i--)
        {
           spriteTextList[i].SetActive(false); 
        }

        var charArray = text.ToCharArray();

        if (spriteTextList.Count < charArray.Length)
        {
            int diff = (charArray.Length - spriteTextList.Count);
            for (int i = 0; i <diff; i++)
            {
                GameObject spriteText = new GameObject();
                spriteText.transform.parent = gameObject.transform;
                spriteTextList.Add(spriteText);
                spriteText.transform.localScale = Vector3.one;
                float spacing = SpriteList[0].rect.width * (spriteTextList.Count - 1);
                spriteText.transform.localPosition = new Vector2(spacing, 0);
                spriteText.AddComponent<Image>();
            }
        }

        for(int i = 0 ; i < charArray.Length ; i++)
        {
            var spriteText = spriteTextList[i];
            char character = charArray[i];
            spriteText.SetActive(true);
            spriteText.name = "" + character;
            var renderer = spriteText.GetComponent<Image>();

            foreach (var sprite in SpriteList)
            {
                if (sprite.name.Contains("" + character))
                { 
                    renderer.sprite = sprite;
                    renderer.SetNativeSize();
                    break;
                }
            }
        }
    }

    public float getWidth()
    {
        float value = currentText.Length * transform.localScale.x * originSize.x;  
        return value;
    }
}
