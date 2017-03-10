using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
     
 
    public float PixelUnit = 100.0f; //This can be PixelsPerUnit, or you can change it during runtime to alter the camera.
    public int desireHeight = 480;
    public int desireWidth = 800; 

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
         
        Screen.SetResolution(800, 480, true);
 
           
        AudioClip bgm = null;
        if (Application.loadedLevelName == "Title")
        {
            string path = "Music/titleBgm"; 
            bgm = Resources.Load(path) as AudioClip;

            AudioMgr.Instance.play(bgm);
        }
        else if (Application.loadedLevelName == "Load")
        {
            string path = "Music/LoadingBgm";
            bgm = Resources.Load(path) as AudioClip;

            AudioMgr.Instance.play(bgm,false);
        }
        else if (Application.loadedLevelName == "Result")
        {
            string path = "Music/ResultBgm";
            bgm = Resources.Load(path) as AudioClip; 
            AudioMgr.Instance.play(bgm,false); 
        }


    }

    void Update()
    { 

    }

    void OnLevelWasLoaded(int level)
    {
        
        //print("Level"+level);
        //if (level == 0)
        //{
        //    AudioClip titleBg = Resources.Load("Music/titleBgm") as AudioClip;
        //    AudioMgr.Instance.Aduio.clip = titleBg;
        //    AudioMgr.Instance.Aduio.Play();
        //} 
    }
     
}

