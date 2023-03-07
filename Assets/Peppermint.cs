using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peppermint : Collectible, IPlayerTriggerable
{
    
    public void OnPlayerTriggered(PlayerController player)
    {
        collected = true;
    }
    private void Start()
    {
        if (!GameController.i.playerController.collectibles.ContainsKey("Peppermint"))
        {
            var list = new Dictionary<int, Collectible>
            {
                { collectID, this }
            };
            GameController.i.playerController.collectibles.Add("Peppermint", list);
        }
        else
        {
            var peppermints = GameController.i.playerController.collectibles["Peppermint"];
            if (peppermints.ContainsKey(collectID))
            {
                collected = peppermints[collectID].collected;
                peppermints[collectID] = this;
            }
        }
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

    }
#endif
}
