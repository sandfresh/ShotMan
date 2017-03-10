using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public Sprite[] Backgrounds;
    public Sprite[] Faces;
    public Sprite[] ResultText; 
    public AudioClip StageME;
    public AudioClip WarningSE;
    public Transform PauseWindow;
    public GameObject Player;
    public GameObject[] Bosses;
    private GameObject Boss;

    private bool isSetBossReady;
    private bool isGameStart;
    private bool isGamePause;
    private bool isGameOver; 

    public bool GameStart
    {
        get
        {
            return isGameStart;
        }
        set
        {
            isGameStart = value;
        }
    }

    public bool GameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
        }
    }

    void Awake()
    {
       
    }
	void Start ()
	{
	    int stage = GlobalVariable.getInstance().getCurrentStage(); 
	    Sprite sprite = Backgrounds[stage];
	    Sprite face = Faces[stage];
	    GameObject background = GameObject.Find("Background");
        background.GetComponent<BgScript>().setSprite(sprite);
        background.GetComponent<BgScript>().setFace(face);

        for (int i = 0; i < Bosses.Length; i++)
        {
            Bosses[i].SetActive(false);
        }
        Boss = Bosses[GlobalVariable.getInstance().getCurrentStage()];
        Boss.SetActive(true);

        GameObject.Find("StageUI").GetComponent<StageUI>().ShowWarning();

	    GlobalVariable.getInstance().HitNumber = 0;
	    GlobalVariable.getInstance().ShotNumber = 0;
	    GlobalVariable.getInstance().GameTime = 0;
	    GlobalVariable.getInstance().Combo = 0;
        GlobalVariable.getInstance().MaxCombo = 0;
        GlobalVariable.getInstance().Hp = Player.GetComponent<HpControl>().HP;
        
        StartGame();
        print("StageLevel:" + GlobalVariable.getInstance().CurrentLevel);
    }

    private void Update()
    {
        if (isGameStart)
        {
            GlobalVariable.getInstance().GameTime = GlobalVariable.getInstance().GameTime + Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    public void StartGame()
    {
        AudioMgr.Instance.Audio.Stop();
        AudioMgr.Instance.Audio.PlayOneShot(WarningSE);
        StartCoroutine(waitStart());
    }

    public void StopGame()
    {
        isGameStart = false;
        GameObject background = GameObject.Find("Background");
        GameObject stageUI = GameObject.Find("StageUI");
       
        if (Boss.GetComponent<HpControl>().hp <= 0)
        {  
            background.GetComponent<BgScript>().showWinEffect();
            stageUI.GetComponent<StageUI>().ShowWin();
            int currentStageIndex = GlobalVariable.getInstance().getCurrentStage();
            int curretnStageLevel = GlobalVariable.getInstance().CurrentLevel;
            int currentProgress = GlobalVariable.getInstance().getStageProgress(currentStageIndex);
            print("Stage "+currentStageIndex+" Clear "+ curretnStageLevel);
            if(curretnStageLevel == currentProgress)
             GlobalVariable.getInstance().modifyStageProgress(currentStageIndex, 1);
        }
        else
        { 
            background.GetComponent<BgScript>().showLoseEffect();
            stageUI.GetComponent<StageUI>().ShowLose();
        } 
    }

    public void ShowScore()
    { 
        print("Next Scene");
        Application.LoadLevel(YS.Result);
    }

    public void showResult()
    {
        StartCoroutine(waitGameOver());
    }

    public GameObject getBoss()
    {
        return Boss;
    }

    private IEnumerator waitGameOver()
    {
        while (isGameOver == false)
        {
            yield return new WaitForSeconds(0.1f);

            if (Boss.GetComponent<PlayerControl>().Down == true || Player.GetComponent<PlayerControl>().Down == true)
            {
                isGameOver = true;

                if (Boss.GetComponent<PlayerControl>().Down)
                {
                    
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.AppendInterval(2);
                    mySequence.AppendCallback(() => 
                    {
                        AudioMgr.Instance.Audio.PlayOneShot(Resources.Load("Sound/rockWin") as AudioClip);
                    });
                    mySequence.AppendInterval(3);
                    mySequence.AppendCallback(() =>
                    {
                        Player.GetComponent<RockControl>().setWin();
                    });
                   
                }
                else
                { 
                    Boss.GetComponent<PlayerControl>().setWin();
                }
            }    
        }
    }

    private IEnumerator waitStart()
    {
        while (isGameStart == false)
        {
            yield return new WaitForSeconds(0.1f); 

            if (Player.GetComponent<RockControl>().getIsReady() &&
                Boss.GetComponent<PlayerControl>().getIsReady())
            {
                isGameStart = true; 
                AudioMgr.Instance.play(StageME);
            }
            else
            {
                if (Player.GetComponent<RockControl>().getIsReady() && isSetBossReady == false)
                {
                    Boss.GetComponent<PlayerControl>().initial();
                    isSetBossReady = true;
                }
            } 
        } 
    }

    public void pauseGame()
    {
        if (isGameStart && Time.timeScale == 1 && isGamePause == false)
        {
            isGameStart = false;
            isGamePause = true;
            Time.timeScale = 0;

            PauseWindow.gameObject.SetActive(true);  
        }
    }

    public void resumeGame()
    {
        if (isGamePause)
        {
            isGameStart = true;
            isGamePause = false;
            Time.timeScale = 1;
            PauseWindow.gameObject.SetActive(false);
        }
    }

    public void exitGame()
    {
        Application.LoadLevel(YS.Title);
    }
     
}
