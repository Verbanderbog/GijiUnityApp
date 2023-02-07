using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFitObject : MonoBehaviour
{
    BoxCollider2D c;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<BoxCollider2D>();
        rect = GetComponent<RectTransform>();
        Fit();
    }

    private void OnValidate()
    {
        c = GetComponent<BoxCollider2D>();
        rect = GetComponent<RectTransform>();
        Fit();
    }

    private void Fit()
    {
        c.size = new Vector2(rect.rect.width,rect.rect.height);
    }
}
