using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Collectible : MonoBehaviour
{
    public int collectID;
    public bool collected;

    private void Update()
    {
        if (gameObject.activeSelf && collected)
        {
            gameObject.SetActive(false);
        }
    }
}