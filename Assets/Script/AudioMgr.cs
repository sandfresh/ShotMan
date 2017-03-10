using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour
{

    private static AudioMgr instance;

    public static AudioMgr Instance
    {
        get
        {
            if (instance == null)
            {
                Instantiate(Resources.Load("Prefab/AudioMgr")); 
            }
            return instance;
        }
    }

    public AudioSource Audio
    {
        get { return audioSource; }
    }

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            gameObject.AddComponent<AudioSource>();
            audioSource = gameObject.GetComponent<AudioSource>(); 
            audioSource.volume = 0.5f;
        }

        DontDestroyOnLoad(this.gameObject);  
    }

    public void play(AudioClip clip,bool isLoop = true)
    {
        if (audioSource.clip != clip && clip != null)
        {
            audioSource.loop = isLoop;
            audioSource.clip = clip;
            audioSource.Play();
        }     
    }

    public void setOn(bool value)
    {
        audioSource.mute = !value;
    }
 

    void Start ()
    {
	
	}
	
	 
	void Update ()
    {
	
	}
}
