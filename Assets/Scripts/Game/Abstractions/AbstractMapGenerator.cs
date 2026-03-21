using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbstractMapGenerator : MonoBehaviour
{
    [SerializeField] protected MapRenderer mapRenderer = null;
    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;
    
    public void GenerateMap()
    {
        mapRenderer.Clear();
        StartProceduralGeneration();
    }
    public abstract void StartProceduralGeneration();
}
