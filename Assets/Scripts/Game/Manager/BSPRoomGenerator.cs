using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BSPRoomGenerator : MapGenerator
{
    public override void StartProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var rooms = AlgorithmsManager.BinarySpacePartition(
            new BoundsInt((Vector3Int)startPos,
            new Vector3Int(parameters.dungeonWidth, parameters.dungeonHeight, 0)),
            parameters.minRoomWidth,
            parameters.minRoomHeight
            );
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogWarning(
                $"[BSPRoomGenerator] BinarySpacePartition no generó rooms. " +
                $"Dungeon=({parameters.dungeonWidth}x{parameters.dungeonHeight}), " +
                $"MinRoom=({parameters.minRoomWidth}x{parameters.minRoomHeight})"
            );
            return;
        }
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        if(parameters.randomWalkRooms)
        {
            floor = CreateRandomWalkRooms(rooms);
        }
        else
        {
            floor = CreateSimpleRooms(rooms);
        }
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in rooms)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        mapRenderer.PaintMap(floor);
        WallManager.GenerateWalls(floor, mapRenderer);
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestRoom(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> corridor = GenerateCorridor(currentRoomCenter, closest);
            corridors.UnionWith(corridor);
            currentRoomCenter = closest;
        }
        return corridors;
    }

    private HashSet<Vector2Int> GenerateCorridor(Vector2Int currentRoomCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != closest.y)
        {
            if (closest.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (closest.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != closest.x)
        {
            if (closest.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (closest.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestRoom(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var room in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(currentRoomCenter, room);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = room;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        foreach (var room in rooms)
        {
            for (int col = parameters.offset; col < room.size.x - parameters.offset; col++)
            {
                for (int row = parameters.offset; row < room.size.y - parameters.offset; row++)
                {
                    floorPositions.Add((Vector2Int)room.min + new Vector2Int(col, row));
                }
            }
        }
        return floorPositions;
    }
    private HashSet<Vector2Int> CreateRandomWalkRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var roomCenter = (Vector2Int)Vector3Int.RoundToInt(room.center);
            var roomFloor = RunRandomWalk(parameters, roomCenter);
            foreach(var pos in roomFloor)
            {
                if (pos.x > room.min.x + parameters.offset && pos.x < room.max.x - parameters.offset && pos.y > room.min.y + parameters.offset && pos.y < room.max.y - parameters.offset)
                {
                    floorPositions.Add(pos);
                }
            }
        }
        return floorPositions;
    }
}