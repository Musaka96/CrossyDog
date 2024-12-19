using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawnable", menuName = "ScriptableObjects/Obstacle", order = 1)]
public class Obstacle : Spawnable
{
    public bool breakable = false;

    public int minHorizontalSpawnNumber = 1;
    public int maxHorizontalSpawnNumber = 3;

    public int horizontalSize = 1;

    public List<Spawnable> spawnableTerrains = new List<Spawnable>();
}