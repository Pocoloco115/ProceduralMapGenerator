using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Random Walk Settings")]
    [SerializeField] private Vector2Int startPos = Vector2Int.zero;
    [SerializeField] private MapGeneratorSO mapGeneratorOS;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private bool showGenerationBySteps = true;

    private static List<Vector2Int> Directions = new List<Vector2Int>()
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PaintMap(IEnumerable<Vector2Int> floorPositions) 
    {
        foreach(var pos in floorPositions)
        {
            PaintSingleTile(pos, floorTile);    
        }
    }
    private void CreateWalls(HashSet<Vector2Int> positions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach(var direction in Directions)
        {
            foreach(var pos in positions)
            {
                Vector2Int newPos = pos + direction;
                if (!positions.Contains(newPos))
                {
                    wallPositions.Add(newPos);
                }
            }
        }
        
        foreach(var wallPosition in wallPositions)
        {
            PaintSingleTile(wallPosition, wallTile);
        }
    }
    private void PaintSingleTile(Vector2Int pos, TileBase tile) 
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }
    public void GenerateMap()
    {
        ClearMap();
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        Vector2Int currentStartPos = startPos;
        for (int i = 0; i < mapGeneratorOS.walkLenght; i++)
        {
            HashSet<Vector2Int> path = RandomWalk(currentStartPos, mapGeneratorOS.walkLenght);
            floorPositions.UnionWith(path);
            if (mapGeneratorOS.startRandomlyEachIteration)
            {
                currentStartPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        if (showGenerationBySteps)
        {
            StartCoroutine(PaintMapBySteps(floorPositions));
            StartCoroutine(PaintWallsBySteps(floorPositions));
            return;
        }
        PaintMap(floorPositions);
        CreateWalls(floorPositions);
    }
    private void ClearMap()
    {
        tilemap.ClearAllTiles();
    }
    private IEnumerator PaintMapBySteps(IEnumerable<Vector2Int> floorPositions) 
    {
        foreach(var pos in floorPositions)
        {
            PaintSingleTile(pos, floorTile);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator PaintWallsBySteps(IEnumerable<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var pos in floorPositions)
        {
            foreach (var direction in Directions)
            {
                Vector2Int neighborPos = pos + direction;
                if (!floorPositions.Contains(neighborPos))
                {
                    wallPositions.Add(neighborPos);
                }
            }
            foreach (var wallPos in wallPositions)
            {
                PaintSingleTile(wallPos, wallTile);
                yield return new WaitForSeconds(0.01f); 
            }
        }
    }
    private HashSet<Vector2Int> RandomWalk(Vector2Int start, int walkLength)
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
    private Vector2Int GetRandomDirection()
    {
        return Directions[Random.Range(0, Directions.Count)];
    }
}
