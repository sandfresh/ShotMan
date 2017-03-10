using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour {

    // Use this for initialization

    public Image LeftImage;
    public Image RightImage ; 

    private bool isLeftPressed;
    private bool isRightPressed;

    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.touchCount > 0)
	    {
	        for (int i = 0; i < Input.touchCount; i++)
	        {
	            Touch touch = Input.GetTouch(i);

	            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
	            {
	                if ((touch.position.x) < 150 && touch.position.y < 100)
	                {
	                    LeftImage.GetComponent<RectTransform>().localScale = new Vector2(0.9f, 0.9f);
	                    RightImage.GetComponent<RectTransform>().localScale = new Vector2(-1, 1);
	                    isLeftPressed = true;
	                    isRightPressed = false;
	                    break;
	                }
	                else if ((touch.position.x) < 300 && touch.position.y < 100)
	                {
	                    RightImage.GetComponent<RectTransform>().localScale = new Vector2(-0.9f, 0.9f);
	                    LeftImage.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
	                    isLeftPressed = false;
	                    isRightPressed = true;
                        break;
                    }
	                else if ((touch.position.x) > 400 && (touch.position.x) < 800 && touch.position.y < 100)
	                {
	                    print("Area3 Touch");
	                }
	                else
	                {
	                    LeftImage.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
	                    RightImage.GetComponent<RectTransform>().localScale = new Vector2(-1, 1);
	                    isLeftPressed = false;
	                    isRightPressed = false;
	                }
                } 
                else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {

                    if ((touch.position.x) < 150 && touch.position.y < 100)
                    {
                        LeftImage.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
                        isLeftPressed = false;
                        break;
                    }
                    else if ((touch.position.x) < 300 && touch.position.y < 100)
                    {
                        RightImage.GetComponent<RectTransform>().localScale = new Vector2(-1, 1);
                        isRightPressed = false;
                        break;
                    }
                }
            }
        }
        
          
         
	   
    }

    public bool LeftPressed
    {
        set
        {
            isLeftPressed = value;
        }
        get
        {
            return isLeftPressed; ;
        }
    }

    public bool RightPressed
    {
        set
        {
            isRightPressed = value;
        }
        get
        {
            return isRightPressed; ;
        }
    }
}
