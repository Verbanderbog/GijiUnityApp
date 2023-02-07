using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinerController : MonoBehaviour
{
    public void SetConfiners(SceneDetails scene)
    {
        scene.AddChildCollidersTo(transform);
        foreach (Transform collider in transform)
        {
            if (!scene.ConnectedTo(collider.name))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
