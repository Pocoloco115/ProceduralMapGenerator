using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase wallSideRight;
    [SerializeField] private TileBase wallSideLeft;
    [SerializeField] private TileBase wallBottom;
    [SerializeField] private TileBase wallFull;
    [SerializeField] private TileBase wallInnerCornerDownLeft;
    [SerializeField] private TileBase wallInnerCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerDownLeft;
    [SerializeField] private TileBase wallDiagonalCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerUpLeft;
    [SerializeField] private TileBase wallDiagonalCornerUpRight;

    public void PaintMap(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTile, floorTilemap);
    }
    public void PaintSingleWall(Vector2Int position, string BinaryType)
    {
        int binaryValue = Convert.ToInt32(BinaryType, 2);
        TileBase tile = null;
        if (WallByteType.wallTop.Contains(binaryValue))
        {
            tile = wallTop;
        }
        else if (WallByteType.wallSideRight.Contains(binaryValue))
        {
            tile = wallSideRight;
        }
        else if (WallByteType.wallSideLeft.Contains(binaryValue))
        {
            tile = wallSideLeft;
        }
        else if (WallByteType.wallBottom.Contains(binaryValue))
        {
            tile = wallBottom;
        }
        else if (WallByteType.wallFull.Contains(binaryValue))
        {
            tile = wallFull;
        }
        if (tile != null)
        {
            PaintSingleTile(position, tile, wallTilemap);
        }
    }
    private void PaintTiles(IEnumerable<Vector2Int> positions, TileBase tile, Tilemap targetTilemap)
    {
        foreach (var pos in positions)
        {
            PaintSingleTile(pos, tile, targetTilemap);
        }
    }
    private void PaintSingleTile(Vector2Int position, TileBase tile, Tilemap targetTilemap)
    {
        Vector3Int tilePos = new Vector3Int(position.x, position.y, 0);
        targetTilemap.SetTile(tilePos, tile);
    }
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        if (wallTilemap != null)
        {
            wallTilemap.ClearAllTiles();
        }
    }

    public void PaintSingleCornerWall(Vector2Int pos, string neighborBinaryType)
    {
        int binaryValue = Convert.ToInt32(neighborBinaryType, 2);
        TileBase tile = null;

        if (WallByteType.wallInnerCornerDownLeft.Contains(binaryValue))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallByteType.wallInnerCornerDownRight.Contains(binaryValue))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallByteType.wallDiagonalCornerDownLeft.Contains(binaryValue))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallByteType.wallDiagonalCornerDownRight.Contains(binaryValue))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallByteType.wallDiagonalCornerUpLeft.Contains(binaryValue))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallByteType.wallDiagonalCornerUpRight.Contains(binaryValue))
        {
            tile = wallDiagonalCornerUpRight;
        }
        if (tile != null)
        {
            PaintSingleTile(pos, tile, wallTilemap);
        }
    }
}
