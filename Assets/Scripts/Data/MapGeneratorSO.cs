using UnityEngine;

[CreateAssetMenu(fileName = "MapGeneratorSO", menuName = "Scriptable Objects/MapGeneratorSO")]
public class MapGeneratorSO : ScriptableObject
{
    public int walkLength;
    public int iterations;
    public bool startRandomlyEachIteration;
}
