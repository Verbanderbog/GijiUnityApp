using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { Empty, Grass, Ice, Snow }
public class TileDetector : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    public AudioClip[] footstepSounds = new AudioClip[Enum.GetValues(typeof(TileType)).Length];
    public static TileDetector Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public TileType GetTileType(Vector3 tileLocation)
    {
        tileLocation.x -= 0.5f;
        tileLocation.y -= 0.8f;
        TerrainTile tile = tilemap.GetTile(Vector3Int.RoundToInt(tileLocation)) as TerrainTile;
        if (tile != null)
            return tile.tileType;
        else
            return TileType.Empty;

    }

}
