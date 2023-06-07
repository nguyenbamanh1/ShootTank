using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStarted : MonoBehaviour
{
    public Transform startPostion;
    public float angelRotation;
    public static bool loadOk;
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (Main.loadOk)
        {
            Main.loadOk = false;
            loadOk = true;
            try
            {
                PlayerPrefs.DeleteAll();
            }catch(Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
    
