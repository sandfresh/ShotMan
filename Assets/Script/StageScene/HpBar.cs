using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{

    public Image Mask;

    private int currentValue = 28;
    private int maxValue = 28; 

    private float maskMaxHeight;
    private float maskMaxWidth;  

    void Start ()
	{ 
        initial();
	} 
 
	void Update ()
	{ 
	}

    public void initial()
    {
        maskMaxHeight = Mask.rectTransform.rect.height;
        maskMaxWidth = Mask.rectTransform.rect.width;
        Mask.rectTransform.sizeDelta = new Vector2(maskMaxWidth, maskMaxHeight * (float)(maxValue - currentValue) / maxValue);
    }

    public void setValue(int value)
    {
        currentValue = value; 
        Mask.rectTransform.sizeDelta = new Vector2(maskMaxWidth, maskMaxHeight * (float)(maxValue - currentValue) / maxValue);  
    }

    public void setValueShow(bool value)
    { 
        gameObject.transform.Find("Hp").GetComponent<Image>().enabled = value;
    }
  
    public void playRestoreEffect(Action delegationAction)
    {
        //int tempValue = 0;
        //Sequence mySequence = DOTween.Sequence();
        //mySequence.AppendInterval(0.04f);
        //mySequence.InsertCallback(0, () =>
        //{ 
        //    if (tempValue == maxValue)
        //    {
        //        delegationAction(); 
        //    }
        //    else
        //    {
        //        Mask.rectTransform.sizeDelta = new Vector2(maskMaxWidth, maskMaxHeight * (float)(maxValue - ++tempValue) / maxValue);
        //    }
        //});
        //mySequence.SetLoops(maxValue+1); 
        StartCoroutine(restore(delegationAction));
    }

    public IEnumerator restore(Action delegationAction)
    {
        int tempValue = 0;
        while (tempValue < maxValue)
        {
            yield return new WaitForSeconds(0.035f);
            Mask.rectTransform.sizeDelta = new Vector2(maskMaxWidth, maskMaxHeight * (float)(maxValue - ++tempValue) / maxValue);
        }
        yield return new WaitForSeconds(0.035f);
        delegationAction();
    }
}
