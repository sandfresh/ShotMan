using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour 
{
    public void onAnimationFinished()
    {
        Destroy(gameObject);
    }
}