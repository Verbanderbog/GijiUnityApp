using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/*
#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(fileName = "New Terrain Tile", menuName = "Tiles/TerrainTile")]
*/
public class TerrainTile : Tile
{
    public TileType tileType;

/*
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Terrain Tile")]
    public static void CreateTerrainTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Terrain Tile","New Terrain Tile","Asset","Save Terrain Tile","Assets");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), path);
    }
#endif
    */
}