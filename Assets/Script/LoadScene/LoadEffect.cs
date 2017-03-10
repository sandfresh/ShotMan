using UnityEngine;
using System.Collections;

public class LoadEffect : MonoBehaviour
{

	// Use this for initialization
    public GameObject LoadParticle;
    private float mTime ;
	void Start ()
    {
        
      
    }
	
	// Update is called once per frame
	void Update ()
	{
	    mTime += Time.deltaTime;

	    if (mTime >= 0.02f)
	    {
            GameObject obj = Instantiate(LoadParticle, new Vector3(4, Random.Range(-2.4F, 2.4F), 0), new Quaternion(0, 0, 0, 0)) as GameObject; 

            obj.transform.localScale = transform.localScale*Random.Range(0.2F, 1);

            mTime = 0;
	    }
	}
}
