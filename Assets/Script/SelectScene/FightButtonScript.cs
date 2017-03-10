using System.Collections;
using UnityEngine; 
using DG.Tweening;
using UnityEngine.UI;

public class FightButtonScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    showEffect();

        GetComponent<Button>().onClick.AddListener((() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
            Application.LoadLevel(YS.Load);
        }));
    }

    public void showEffect()
    {
        GameObject imgobj = gameObject.transform.Find("startBtnImage").gameObject;

        imgobj.transform.localScale = new Vector3(1, 1, 1);
        Image image = imgobj.transform.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 1); 

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(imgobj.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f));
        mySequence.AppendInterval(0.2f);
        mySequence.SetLoops(-1);


        Sequence mySequence2 = DOTween.Sequence(); 
        mySequence2.Append(image.DOFade(0f, 0.5f));
        mySequence2.AppendInterval(0.2f);
        mySequence2.SetLoops(-1); 
    }
       

    void Update ()
    {
	
	}
     
}
