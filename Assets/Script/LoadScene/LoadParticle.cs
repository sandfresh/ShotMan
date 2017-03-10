using UnityEngine;
using System.Collections;

public class LoadParticle : MonoBehaviour
{
    private int mSpeed ;
	// Use this for initialization
	void Start ()
	{
	    mSpeed = Random.Range(10, 12);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector2.left * Time.deltaTime * mSpeed);

        if (transform.position.x < -7)
            Destroy(gameObject);
    }
}
