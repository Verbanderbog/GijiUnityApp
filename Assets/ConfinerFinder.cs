using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ConfinerFinder : MonoBehaviour
{
    private UnityAction<Scene, LoadSceneMode> findConfiner;
    public static ConfinerFinder i { get; private set; }
    public CinemachineConfiner2D cine;
    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        cine = GetComponent<CinemachineConfiner2D>();
        cine.InvalidateCache();
        cine.m_BoundingShape2D = FindObjectsOfType<Collider2D>().First(x=> x.name == "Confiner");
        findConfiner = (scene, mode) =>
        {
            cine.InvalidateCache();
            cine.m_BoundingShape2D = FindObjectsOfType<Collider2D>().First(x => x.name == "Confiner");
        };
        SceneManager.sceneLoaded += findConfiner;
    }
    public void UnsubSceneLoad()
    {
        SceneManager.sceneLoaded -= findConfiner;
    }

}
