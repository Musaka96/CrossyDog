using UnityEngine;

[CreateAssetMenu(fileName = "Spawnable", menuName = "ScriptableObjects/Terrain", order = 1)]
public class Terrain : Spawnable
{
    public int minSpawnStackNumber = 1;
    public int maxSpawnStackNumber = 3;

    public int maxObstaclesNumber = 4;
}