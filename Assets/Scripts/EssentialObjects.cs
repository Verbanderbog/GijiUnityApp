using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EssentialObjects : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Quit()
    {
        
        ConfinerFinder.i.unsubSceneLoad();
        SceneManager.LoadScene(0);
        DestroyImmediate(gameObject);
    }
}
