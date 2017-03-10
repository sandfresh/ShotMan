using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombCircle : MonoBehaviour
{


    public Transform BombPrefab; 
    public int BombNumber;
    public float Radius;
    public float Speed;
    public int Damage;

    private List<Transform> BombList; 
    private Action BombAction;
    private bool counterclockWise;

    void Awake()
    { 
        BombList = new List<Transform>(); 
        
        for (int i = 0; i < BombNumber; i++)
        {
            var bomb = Instantiate(BombPrefab);
            BombList.Add(bomb);
            var go = new GameObject();
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);

            bomb.transform.parent = go.transform;
            bomb.transform.localPosition = new Vector3(0, Radius, 0);
            bomb.GetComponent<BombControl>().setDestoryAction(() =>
            {
                BombList.Remove(bomb); 
                if (BombList.Count == 0)
                { 
                    Destroy(gameObject);
                }

                if (BombAction != null)
                    BombAction();
            });
            bomb.GetComponent<DamageControl>().Damage = Damage;
            go.transform.eulerAngles = new Vector3(0, 0, (360.0f / BombNumber) * i);
        }
    }

	void Start ()
    {
        
    }
	
	 
	void Update ()
	{
	    if (Time.timeScale == 0)
	        return;
	    gameObject.transform.Rotate(new Vector3(0,0, Speed * (counterclockWise? 1:-1) * Time.deltaTime));
	} 

    void OnDestroy()
    {  
    }

    public void setOwner(GameObject owner)
    {
        foreach(Transform bomb in BombList)
        {
            bomb.GetComponent<DamageControl>().setOwner(owner);
        }
    }

    public void setIsReflect(bool value)
    {
        foreach (Transform bomb in BombList)
        {
            bomb.GetComponent<BombControl>().IsReflect = value;
        }
    }

    public void setClockwise(bool value)
    {
        counterclockWise = value;
    }

    public void setRadius(float value)
    {
        Radius = value;

        foreach(Transform bomb in BombList)
        {
            bomb.transform.localPosition = new Vector3(0, Radius, 0);
        }
    }
     
    public void setBombDestroyAction(Action action)
    {
        BombAction = action;
    }

    public int getBombNumber()
    {
        return BombList.Count;
    }
}
