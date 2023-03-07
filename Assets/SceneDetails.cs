using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    public bool IsLoaded { get; private set; }

    [SerializeField] List<SceneDetails> connectedScenes;
    public List<SceneDetails> ConnectedScenes { get => connectedScenes; }

    new Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            LoadScene(true);
            
            foreach (var scene in GameController.i.loadedScenes)
            {
                if (scene != this && !connectedScenes.Contains(scene))
                {
                    scene.UnloadScene();
                }
            }
            GameController.i.loadedScenes.RemoveWhere(scene => scene != this && !connectedScenes.Contains(scene));
            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }
            
        }
        if (collision.CompareTag("MainCamera"))
        {


        }
    }

    private void LoadScene(bool primaryScene = false)
    {
        if (!IsLoaded)
        {
            var load = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            if (primaryScene)
            {
                load.completed += e => {
                    TileDetector.Instance.SetTilemap(this);
                    var controller = ConfinerFinder.i.cine.m_BoundingShape2D.GetComponent<ConfinerController>();
                    if (controller != null)
                    {
                        controller.SetConfiners(this);
                    }
                };
            }
            IsLoaded = true;
            GameController.i.loadedScenes.Add(this);
        }
        else
        {
            if (primaryScene)
            {
                TileDetector.Instance.SetTilemap(this);
                var controller = ConfinerFinder.i.cine.m_BoundingShape2D.GetComponent<ConfinerController>();
                if (controller != null)
                {
                    controller.SetConfiners(this);
                }
            }
                
        }
    }

    private void UnloadScene()
    {
        if (IsLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
            
        }
    }

    public bool ConnectedTo(string name)
    {
        List<string> names = new()
        {
            this.name
        };
        foreach (var scene in connectedScenes)
        {
            names.Add(scene.name);
        }
        return names.Contains(name);
    }

    public void AddChildCollidersTo(Transform transform)
    {
        if (transform.Find(collider.name) == null)
            MakeColliderGameObject().transform.parent = transform;
        foreach (var scene in connectedScenes)
        {
            if (transform.Find(scene.collider.name) == null)
                scene.MakeColliderGameObject().transform.parent = transform;
        }
    }

    private GameObject MakeColliderGameObject()
    {
        var retGameObject = new GameObject(collider.name);
        Collider2D retCollider = (Collider2D)retGameObject.AddComponent(collider.GetType());

        if (retCollider is BoxCollider2D d)
        {
            d.size = ((BoxCollider2D)collider).size;
            d.offset = ((BoxCollider2D)collider).offset;
        }
        else if (retCollider is PolygonCollider2D d1)
        {
            d1.offset = ((PolygonCollider2D)collider).offset;
            d1.points = ((PolygonCollider2D)collider).points;
        }
        retCollider.transform.position = transform.position;
        retCollider.usedByComposite = true;
        
        return retGameObject;

    }
}
