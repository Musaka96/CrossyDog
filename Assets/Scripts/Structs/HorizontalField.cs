// If you're questioning why this is a struct, look up Data Oriented Design ^^
using System.Collections.Generic;
using UnityEngine;

public struct HorizontalField
{
    public Terrain terrainType;
    GameObject terrain;

    public int position;
    int numberOfObsticles;

    List<GameObject> obstacles;

    public HorizontalField(Terrain terrainType, GameObject terrain, int position) : this() {
        this.terrainType = terrainType;
        this.terrain = terrain;
        this.position = position;
    }

    public void AddObsticles(List<GameObject> obsticles) {
        this.obstacles = new List<GameObject>();
        this.obstacles.AddRange(obsticles);
    }
}
