using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectPage : MonoBehaviour
{

	// Use this for initialization
    public int Index = 0; 

	void Start ()
    {
        GetComponent<Button>().onClick.AddListener((() =>
        {
            AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/Click") as AudioClip);
            if (Index == 2)
            {
                Window.Show("警告","下載超過10000開放");
            }
            else
            {
                SetPage();
            } 
        }));
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetPage()
    {
        GlobalVariable.getInstance().setCurrentPage(Index);
    }

    public int GetPage()
    {
        return Index;
    }
}
