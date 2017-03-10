using UnityEngine;
using System.Collections;

public class AniEffectCtrl : MonoBehaviour 
{
    public void onAnimatoinFinished()
    {
        Destroy(gameObject);
    }
}