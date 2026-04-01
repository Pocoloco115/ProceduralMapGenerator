using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorGenerator : MapGenerator
{
    public enum CorridorWideningMode
    {
        None,
        IncreaseByOne,
        Include3x3
    }
    public override void StartProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);
        HashSet<Vector2Int> roomPostions = GenerateRooms(potentialRoomPositions);
        List<Vector2Int> deadEnds = FindDeadEs(floorPositions);
        AvoidDeadEnds(roomPostions, deadEnds);
        floorPositions.UnionWith(roomPostions);

        for(int i = 0; i < corridors.Count; i++)
        {
            if(parameters.corridorWideningMode == CorridorWideningMode.None) continue;
            if(parameters.corridorWideningMode == CorridorWideningMode.IncreaseByOne) corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            if(parameters.corridorWideningMode == CorridorWideningMode.Include3x3) corridors[i] = IncreaseCorridorsBrush3x3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }
        mapRenderer.PaintMap(floorPositions);
        WallManager.GenerateWalls(floorPositions, mapRenderer);

    }

    private List<Vector2Int> IncreaseCorridorsBrush3x3(List<Vector2Int> vector2Ints)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for(int i = 1; i < vector2Ints.Count; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                for(int k = -1; k < 2; k++)
                {
                    newCorridor.Add(vector2Ints[i - 1] + new Vector2Int(j, k));
                }
            }
        }
        return newCorridor;
    }

    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDir = Vector2Int.zero;

        for(int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if(previousDir != Vector2Int.zero && directionFromCell != previousDir)
            {
                for(int j = -1; j < 2; j++)
                {
                    for(int k = -1; k < 2; k++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(j, k));
                    }
                }
                previousDir = directionFromCell;
            }
            else
            {

                Vector2Int newCorridorTileOffSet = GetDirectionFromCell(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffSet);
            }
            previousDir = directionFromCell;
        }
        return newCorridor;
    }
    private Vector2Int GetDirectionFromCell(Vector2Int dir)
    {
        if(dir == Vector2Int.up) return Vector2Int.right;
        if(dir == Vector2Int.down) return Vector2Int.left;
        if(dir == Vector2Int.left) return Vector2Int.up;
        if(dir == Vector2Int.right) return Vector2Int.down;
        return Vector2Int.zero;
    }

    private void AvoidDeadEnds(HashSet<Vector2Int> floorPositions, List<Vector2Int> deadEnds)
    {
        foreach(var deadEnd in deadEnds)
        {
            if(!floorPositions.Contains(deadEnd))
            {
                var roomFloor = RunRandomWalk(parameters, deadEnd);
                floorPositions.UnionWith(roomFloor);
            }
        }
    }
    private List<Vector2Int> FindDeadEs(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var pos in floorPositions)
        {
            int neighbourCount = 0;
            foreach(var direction in AlgorithmsManager.Directions)
            {
                if (floorPositions.Contains(pos + direction))
                {
                    neighbourCount++;
                }
            }
            if (neighbourCount == 1)
            {
                deadEnds.Add(pos);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> GenerateRooms(HashSet<Vector2Int> roomPositions)
    {
        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(roomPositions.Count * parameters.roomPercent);
        List<Vector2Int> roomsToCreate = roomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        
        foreach(var roomPos in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(parameters, roomPos);
            rooms.UnionWith(roomFloor);
        }
        return rooms;
    }
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> roomPositions)
    {
        Vector2Int currentPos = startPos;
        roomPositions.Add(currentPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < parameters.corridorCount; i++)
        {
            List<Vector2Int> path = AlgorithmsManager.RandomWalkCorridor(currentPos, parameters.corridorLength);
            corridors.Add(path);
            currentPos = path[path.Count - 1];
            roomPositions.Add(currentPos);
            floorPositions.UnionWith(path);
        }
        return corridors;
    }
}