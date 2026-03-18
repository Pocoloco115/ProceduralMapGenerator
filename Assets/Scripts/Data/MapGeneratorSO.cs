using UnityEngine;

[CreateAssetMenu(fileName = "MapGeneratorSO", menuName = "Scriptable Objects/MapGeneratorSO")]
public class MapGeneratorSO : ScriptableObject
{
    public int walkLenght;
    public int iterations;
    public bool startRandomlyEachIteration;


}
