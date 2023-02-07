using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(fileName = "New Terrain Tile", menuName = "Tiles/TerrainTile")]
#endif
public class TerrainTile : RuleTile<TerrainTile.Neighbor>
{
    public List<TileBase> Siblings = new List<TileBase>();
    public TileType tileType;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {

    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.This: return tile == this || Siblings.Contains(tile);

        }
        return base.RuleMatch(neighbor, tile);
    }
}