using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : AbstractMapGenerator
{
    [SerializeField] protected MapGeneratorSO mapGeneratorSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void StartProceduralGeneration()
    {
        mapRenderer.Clear();
        HashSet<Vector2Int> floorPositions = RunRandomWalk(mapGeneratorSO, startPos);
        mapRenderer.PaintMap(floorPositions);
        HashSet<Vector2Int> wallPositions = FindWallPositions(floorPositions);
        mapRenderer.PaintWalls(wallPositions);
    }
    public HashSet<Vector2Int> FindWallPositions(HashSet<Vector2Int> positions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var direction in AlgorithmsManager.Directions)
        {
            foreach (var pos in positions)
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
    protected HashSet<Vector2Int> RunRandomWalk(MapGeneratorSO parameters, Vector2Int pos)
    {
        var currentPos = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = AlgorithmsManager.RandomWalk(currentPos, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}