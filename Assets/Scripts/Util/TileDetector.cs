using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;

public enum TileType { Empty, Grass, Ice, Snow }
public class TileDetector : MonoBehaviour
{
    Tilemap tilemap;

    public static TileDetector Instance { get; private set; }
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public TileType GetTileType(Vector3 tileLocation)
    {
        if (tilemap == null)
        {
            foreach (Tilemap t in FindObjectsOfType<Tilemap>())
            {
                if (t.name == "Background")
                {
                    tilemap = t;
                    break;
                }
            }
        }

        tileLocation.x -= 0.5f;
        tileLocation.y -= 0.8f;
        TerrainTile tile = tilemap.GetTile(Vector3Int.RoundToInt(tileLocation)) as TerrainTile;
        if (tile != null)
            return tile.tileType;
        else
            return TileType.Empty;

    }

    public void SetTilemap(SceneDetails sceneDetails)
    {
        Scene s = SceneManager.GetSceneByName(sceneDetails.name);
        GameObject[] go = s.GetRootGameObjects();
        tilemap = go[0].transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
    }

}
