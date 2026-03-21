using System.Collections.Generic;
using UnityEngine;

public static class AlgorithmsManager
{
    public static List<Vector2Int> Directions = new List<Vector2Int>()
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
    public static HashSet<Vector2Int> RandomWalk(Vector2Int start, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(start);
        Vector2Int previousPosition = start;
        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int newPosition = previousPosition + GetRandomDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int start, int walkLength)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int direction = GetRandomDirection();
        Vector2Int currPosition = start;
        path.Add(currPosition);

        for (int i = 0; i < walkLength; i++)
        {
            currPosition += direction;
            path.Add(currPosition);
        }
        return path;
    }
    public static Vector2Int GetRandomDirection()
    {
        return Directions[Random.Range(0, Directions.Count)];
    }
}