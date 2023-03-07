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
    public List<TileBase> Siblings = new();
    public TileType tileType;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Sibling = 3;
        public const int Self = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        return neighbor switch
        {
            Neighbor.This => tile == this || Siblings.Contains(tile),
            Neighbor.NotThis => tile != this && !Siblings.Contains(tile),
            Neighbor.Sibling => tile != this && Siblings.Contains(tile),
            Neighbor.Self => tile == this,
            _ => base.RuleMatch(neighbor, tile),
        };
    }
}