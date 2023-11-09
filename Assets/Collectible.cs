using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Collectible : MonoBehaviour
{
    public int collectID;
    public bool collected { 
        get {return GameController.i.GetFlag(collectibleTypeName + collectID + "Collected") != 0;} 
        set { GameController.i.SetFlag(collectibleTypeName + collectID + "Collected", value); } 
    }
    protected string collectibleTypeName;
    bool collectedLastUpdate;
    private new Collider2D collider;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        collectedLastUpdate = collected;
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        /*
        if (!string.IsNullOrEmpty(collectibleTypeName))
        {
            if (!GameController.i.playerController.collectibles.ContainsKey(collectibleTypeName))
            {
                var list = new Dictionary<int, Collectible>
            {
                { collectID, this }
            };
                GameController.i.playerController.collectibles.Add(collectibleTypeName, list);
            }
            else
            {
                var items = GameController.i.playerController.collectibles[collectibleTypeName];
                if (items.ContainsKey(collectID))
                {
                    collected = items[collectID].collected;
                    items[collectID] = this;
                }
            }
        }
        */
        
    }
    private void Update()
    {
        if (collected)
        {
            collider.enabled = false;
            spriteRenderer.enabled = false; 
        }else
        {
            collider.enabled = true;
            spriteRenderer.enabled = true;
        }
        if (collected && !collectedLastUpdate)
            GameController.i.AddToFlag(collectibleTypeName+"Total", 1);
        if (!collected && collectedLastUpdate)
            GameController.i.AddToFlag(collectibleTypeName + "Total", -1);
        collectedLastUpdate = collected;
    }
    
}