using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGeneratorSO", menuName = "Scriptable Objects/MapGeneratorSO")]
public class MapGeneratorSO : ScriptableObject
{
    [Header("Comunes Random walk parameters")]
    public int walkLength;
    public int iterations;
    public bool startRandomlyEachIteration;

    [Header("Corridor Generator")]
    public int corridorLength;
    public int corridorCount;
    [Range(0.1f, 1f)] public float roomPercent;
    public CorridorGenerator.CorridorWideningMode corridorWideningMode;

    [Header("BSP Room Generator")]
    public int dungeonWidth;
    public int dungeonHeight;
    public int minRoomWidth;
    public int minRoomHeight;
    [Range(0, 10)] public int offset;
    public bool randomWalkRooms;
}
