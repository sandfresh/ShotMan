using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class MoveControl : MonoBehaviour 
{
    public KeyCode mUp;
    public KeyCode mDown;
    public KeyCode mLeft;
    public KeyCode mRight;

    public KeyCode mA;
    public KeyCode mB;

    public GameObject GamePanel;

    bool mIsUpPressed;
    bool mIsDownPressed;
    bool mIsLeftPressed;
    bool mIsRightPressed;

    bool mIsAPressed;
    bool mIsBPressed;

    private bool isADown;
    private bool isBDown;
    private bool isAUP;
    private bool isBUP;
     
    private PanelControl panelControl;
    void Start()
    {
        panelControl = GamePanel.GetComponent<PanelControl>();
    }

    void Update () 
	{
        if (Input.GetKeyDown(mUp))
            mIsUpPressed = true;
        else if (Input.GetKeyUp(mUp))
            mIsUpPressed = false;

        if (panelControl.LeftPressed)
        {
            mIsLeftPressed = true;
            mIsRightPressed = false;
        } 
        else if (panelControl.RightPressed)
        {
            mIsRightPressed = true;
            mIsLeftPressed = false;
        }  
        else
        {
            mIsLeftPressed = false;
            mIsRightPressed = false;
        }
	} 

    public bool LeftPressed
    {
        set
        {
            mIsLeftPressed = value; 
        }

        get { return mIsLeftPressed; }
    }

    public bool RightPressed
    {
        set
        {
            mIsRightPressed = value; 
        }

        get { return mIsRightPressed; }
    }

    public bool APressed
    {
        set
        {
            mIsAPressed = value; 
        }

        get { return mIsAPressed;}
    }

    public bool BPressed
    {
        set
        {
            mIsBPressed = value; 
        }

        get { return mIsBPressed; }
    }

    public bool BDown
    {
        set
        { 
            isBDown = value;
            DOTween.Kill("bdown");
            Sequence mySequence = DOTween.Sequence();
            mySequence.id = "bdown";
            mySequence.AppendInterval(Time.fixedTime);
            mySequence.AppendCallback(() =>
            {
                isBDown = false; 
            });
            
        }

        get { return isBDown; }
    }

    public bool BUp
    {
        set
        {
            isBUP = value; 
            DOTween.Kill("bup");
            Sequence mySequence = DOTween.Sequence();
            mySequence.id = "bup";
            mySequence.AppendInterval(Time.fixedTime);
            mySequence.AppendCallback(() =>
            {
                isBUP = false; 
            });
        }
        get { return isBUP;}
    }

    public void leftButtonEnter()
    {
        if (isLeftWait == false)
        {
            mIsLeftPressed = true;
            mIsRightPressed = false; 
        } 
    }

    public void leftButtonExit()
    {
        if (isLeftWait)
            return;
        mIsLeftPressed = false; 

        isLeftWait = true;
        Sequence mySequence = DOTween.Sequence(); 
        mySequence.AppendInterval(0.05f);
        mySequence.AppendCallback(() =>
        {
            isLeftWait = false;
        });

    }

    public void rightButtonEnter()
    {
        if (isRightWait == false)
        {
            mIsRightPressed = true;
            mIsLeftPressed = false; 
        } 
    }

    public void rightButtonExit()
    {
        if (isRightWait)
            return;
        mIsRightPressed = false; 

        isRightWait = true;
        Sequence mySequence = DOTween.Sequence(); 
        mySequence.AppendInterval(0.05f);
        mySequence.AppendCallback(() =>
        {
            isRightWait = false;
        });
    }

    private bool isLeftWait;
    private bool isRightWait; 
}