using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPlaying;
    public bool IsPlaying { get { return isPlaying; } set { isPlaying = value; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(Instance);
        }
    }


    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
         
#endif
    Application.Quit();
    
    }
     
}
