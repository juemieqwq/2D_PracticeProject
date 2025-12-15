using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkeletonManager : MonoBehaviour
{
    public static SkeletonManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
