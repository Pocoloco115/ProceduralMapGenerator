using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallManager 
{
    public static HashSet<Vector2Int> FindWallPositions(HashSet<Vector2Int> positions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var pos in positions)
        {
            foreach (var direction in directions)
            {
                Vector2Int newPos = pos + direction;
                if (!positions.Contains(newPos))
                {
                    wallPositions.Add(newPos);
                }
            }
        }
        return wallPositions;
    }
    public static void GenerateWalls(HashSet<Vector2Int> floorPositions, MapRenderer mapRenderer)
    {
        var wallPositions = FindWallPositions(floorPositions, AlgorithmsManager.Directions);
        var cornerPositions = FindWallPositions(floorPositions, AlgorithmsManager.DiagonalDirections);
        GenerateBasicWall(mapRenderer, wallPositions, floorPositions);
        GenerateCornerWall(mapRenderer, cornerPositions, floorPositions);
    }

    private static void GenerateCornerWall(MapRenderer mapRenderer, HashSet<Vector2Int> cornerPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach(var pos in cornerPositions)
        {
            string neighborBinaryType = "";
            foreach(var direction in AlgorithmsManager.AllDirections)
            {
                Vector2Int neighborPos = pos + direction;
                neighborBinaryType += floorPositions.Contains(neighborPos) ? "1" : "0";
            }
            mapRenderer.PaintSingleCornerWall(pos, neighborBinaryType);
        }
    }

    private static void GenerateBasicWall(MapRenderer mapRenderer, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var pos in wallPositions)
        {
            string neighborBinaryType = "";
            foreach(var direction in AlgorithmsManager.Directions)
            {
                Vector2Int neighborPos = pos + direction;
                neighborBinaryType += floorPositions.Contains(neighborPos) ? "1" : "0";
            }
            mapRenderer.PaintSingleWall(pos, neighborBinaryType);
        }
    }
}
