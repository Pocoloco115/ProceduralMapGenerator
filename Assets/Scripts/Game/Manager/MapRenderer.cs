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
    [SerializeField] private TileBase wallTile;

    [Header("Generation Visuals")]
    [SerializeField] private bool animateGeneration = false;
    [SerializeField] private float stepDelay = 0.03f;

    public void PaintMap(IEnumerable<Vector2Int> floorPositions)
    {
        if (animateGeneration)
        {
            StartCoroutine(PaintTilesCoroutine(floorPositions, floorTile, floorTilemap));
        }
        else
        {
            PaintTilesImmediate(floorPositions, floorTile, floorTilemap);
        }
    }

    public void PaintWalls(IEnumerable<Vector2Int> wallPositions)
    {
        if (animateGeneration)
        {
            StartCoroutine(PaintTilesCoroutine(wallPositions, wallTile, wallTilemap));
        }
        else
        {
            PaintTilesImmediate(wallPositions, wallTile, wallTilemap);
        }
    }

    private void PaintTilesImmediate(IEnumerable<Vector2Int> positions, TileBase tile, Tilemap targetTilemap)
    {
        foreach (var pos in positions)
        {
            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
            targetTilemap.SetTile(tilePos, tile);
        }
    }

    private IEnumerator PaintTilesCoroutine(IEnumerable<Vector2Int> positions, TileBase tile, Tilemap targetTilemap)
    {
        foreach (var pos in positions)
        {
            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
            targetTilemap.SetTile(tilePos, tile);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        if (wallTilemap != null)
        {
            wallTilemap.ClearAllTiles();
        }
    }
}
