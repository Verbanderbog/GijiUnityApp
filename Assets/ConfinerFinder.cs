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
        var cine = GetComponent<CinemachineConfiner2D>();
        cine.InvalidateCache();
        cine.m_BoundingShape2D = FindObjectsOfType<Collider2D>().First(x=> x.name == "Confiner");
        findConfiner = (scene, mode) =>
        {
            var cine = GetComponent<CinemachineConfiner2D>();
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
