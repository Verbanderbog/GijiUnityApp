using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peppermint : Collectible, IPlayerTriggerable
{
    
    public void OnPlayerTriggered(PlayerController player)
    {
        collected = true;
        
    }
    private void Awake()
    {
        collectibleTypeName = "Peppermint";
        
    }
    private void SetPostitionAndSnapToTile()
    {
        var pos = transform.position;
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f;

        transform.position = pos;
    }
    private void SetUniqueKey(Dictionary<int, Collectible> collectibles)
    {
        while (collectibles.ContainsKey(collectID))
        {
            collectID++;
        }

    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        Dictionary<int, Collectible> verifyList = new();
        foreach (Collectible collectible in Object.FindObjectsOfType<Peppermint>())
        {
            if (collectible != this)
            {
                verifyList.Add(collectible.collectID, collectible);
            }
        }
        SetUniqueKey(verifyList);
        verifyList.Add(collectID, this);
        SetPostitionAndSnapToTile();
    }
#endif
}
