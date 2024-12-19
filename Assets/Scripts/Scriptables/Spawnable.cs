using UnityEngine;

public class Spawnable : ScriptableObject
{
    public GameObject prefab;

    public MovementEnum movementDirection = MovementEnum.Left;

    public bool collectable = false;

    public int verticalSize = 1;

    public int spawnHeight = 0;
}