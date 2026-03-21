using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorGenerator : MapGenerator
{
    [SerializeField] private int corridorLenght = 10;
    [SerializeField] private int corridorCount = 5;
    [SerializeField][Range(0.1f, 1f)] private float roomPercent = 0.4f;

    public override void StartProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPositions, roomPositions);
        floorPositions.UnionWith(GenerateRooms(roomPositions));
        mapRenderer.PaintMap(floorPositions);
        mapRenderer.PaintWalls(FindWallPositions(floorPositions));

    }
    private HashSet<Vector2Int> GenerateRooms(HashSet<Vector2Int> roomPositions)
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(roomPositions.Count * roomPercent);
        List<Vector2Int> roomsToCreate = roomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        
        foreach(var roomPos in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(mapGeneratorSO, roomPos);
            rooms.UnionWith(roomFloor);
        }
        return rooms;
    }
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> roomPositions)
    {
        Vector2Int currentPos = startPos;
        roomPositions.Add(currentPos);
        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> path = AlgorithmsManager.RandomWalkCorridor(currentPos, corridorLenght);
            currentPos = path[path.Count - 1];
            roomPositions.Add(currentPos);
            floorPositions.UnionWith(path);
        }
    }
}
