using System.Collections.Generic;
using UnityEngine;

public static class AlgorithmsManager
{
    public static List<Vector2Int> Directions = new List<Vector2Int>()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,

    };
    public static List<Vector2Int> DiagonalDirections = new List<Vector2Int>()
    {
        new Vector2Int(1, 1), 
        new Vector2Int(1, -1), 
        new Vector2Int(-1, -1), 
        new Vector2Int(-1, 1) 
    };
    public static List<Vector2Int> AllDirections = new List<Vector2Int>()
    {
        new Vector2Int(0, 1), 
        new Vector2Int(1, 1),
        new Vector2Int(1, 0), 
        new Vector2Int(1, -1),
        new Vector2Int(0, -1), 
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0), 
        new Vector2Int(-1, 1)
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
    public static List<BoundsInt> BinarySpacePartition(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        List<BoundsInt> rooms = new List<BoundsInt>();
        Queue<BoundsInt> queue = new Queue<BoundsInt>();
        queue.Enqueue(spaceToSplit);
        while (queue.Count > 0)
        {
            BoundsInt room = queue.Dequeue();
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, room, queue);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontaly(minHeight, room, queue);
                    }
                    else
                    {
                        rooms.Add(room);
                    }
                }
                else
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontaly(minHeight, room, queue);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, room, queue);
                    }
                    else
                    {
                        rooms.Add(room);
                    }
                }
            }
        }
        return rooms;
    }

    private static void SplitVertically(int minWidth, BoundsInt room, Queue<BoundsInt> queue)
    {
        var splitX = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(splitX, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + splitX, room.min.y, room.min.z), new Vector3Int(room.size.x - splitX, room.size.y, room.size.z));
        queue.Enqueue(room1);
        queue.Enqueue(room2);
    }

    private static void SplitHorizontaly(int minHeight, BoundsInt room, Queue<BoundsInt> queue)
    {
        var splitY = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, splitY, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + splitY, room.min.z), new Vector3Int(room.size.x, room.size.y - splitY, room.size.z));
        queue.Enqueue(room1);
        queue.Enqueue(room2);
    }

    public static Vector2Int GetRandomDirection()
    {
        return Directions[Random.Range(0, Directions.Count)];
    }
}