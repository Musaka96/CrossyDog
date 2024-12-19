using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoSingleton<Spawner>
{
    [SerializeField]
    List<Terrain> terrains = new List<Terrain>();

    [SerializeField]
    List<Obstacle> obstacles = new List<Obstacle>();

    List<HorizontalField> spawned = new List<HorizontalField>();

    [SerializeField]
    int spawnRange = 10;

    private void Start() {
        this.InitialSpawn();
    }

    public void InitialSpawn() {
        if (this.terrains.Count < 1) return;

        bool firstOne = true;

        int randomSpawnable = Random.Range(0, this.terrains.Count);

        // As long as we have range
        for (int distanceIndex = 0; distanceIndex < this.spawnRange;) {
            if (firstOne) {
                distanceIndex += this.SpawnTerrain(this.terrains[randomSpawnable], distanceIndex);
                firstOne = false;
                continue;
            }

            // We get a random element without repeating
            randomSpawnable = GetRandomIntInRangeWithout(randomSpawnable, this.terrains.Count);
            // And spawn it for a random amount within limits
            distanceIndex += this.SpawnTerrain(this.terrains[randomSpawnable], distanceIndex);
        }
    }

    /// <returns> Distance in blocks covered </returns>
    private int SpawnTerrain(Terrain terrain, int distance) {
        int amountToSpawn = terrain.minSpawnStackNumber + Random.Range(0, terrain.maxSpawnStackNumber - terrain.minSpawnStackNumber + 1);

        Vector3 spawnPosition = new Vector3(distance, terrain.spawnHeight, 0);

        for (int i = 0; i < amountToSpawn; i++) {
            GameObject terrainObj = Instantiate(terrain.prefab, spawnPosition, new Quaternion());

            HorizontalField field = new HorizontalField(terrain, terrainObj, distance);
            this.spawned.Add(field);

            this.SpawnObstacles(field);

            spawnPosition = new Vector3(spawnPosition.x + terrain.verticalSize, terrain.spawnHeight, 0);
            distance++;
        }

        return terrain.verticalSize * amountToSpawn;
    }

    private void SpawnObstacles(HorizontalField field) {
        // find every obstacle that can go on it
        List<Obstacle> validObstacles = this.obstacles.Where(obstacle => obstacle.spawnableTerrains.Contains(field.terrainType)).ToList();

        if (validObstacles.Count < 1) return;

        int obstacleCount = 0;
        // find the max number of obstacles, spawn po redu
        for (int i = 0; i < validObstacles.Count;) {
            if (obstacleCount > field.terrainType.maxObstaclesNumber) break;

            Obstacle obstacle = validObstacles[Random.Range(0, validObstacles.Count)];
            int amountToSpawn = obstacle.minHorizontalSpawnNumber
                + Random.Range(0, obstacle.maxHorizontalSpawnNumber - obstacle.minHorizontalSpawnNumber + 1);

            // Decrease amount untill it can fit ;)
            while (!CanItFit(GameManager.horizontalSize, amountToSpawn, obstacle.horizontalSize) && amountToSpawn > 0) {
                amountToSpawn--;
            }

            if (amountToSpawn < 1) continue;

            //List<int> horizontalPositions = GetRandomisedPositionsInRange(GameManager.horizontalSize, amountToSpawn, obstacle.horizontalSize);
            List<int> horizontalPositions = GetRandomUniqueNumbersWithNegative(GameManager.horizontalSize, amountToSpawn);

            // Spawn obstacles while checking not do overdo it
            for (int j = 0; j < amountToSpawn; j++) {
                if (obstacleCount > field.terrainType.maxObstaclesNumber) break;

                Vector3 spawnPosition = new Vector3(field.position, obstacle.spawnHeight, horizontalPositions[0]);
                GameObject terrainObj = Instantiate(obstacle.prefab, spawnPosition, new Quaternion());

                horizontalPositions.RemoveAt(0);
                obstacleCount++;
            }
        }
    }

    private static int GetRandomIntInRangeWithout(int withoutThisInt, int maxRangeExclusive) {
        if (maxRangeExclusive == 0) return 0;

        int randomNumber = Random.Range(0, maxRangeExclusive);
        if (randomNumber != withoutThisInt) {
            return randomNumber;
        }

        return GetRandomIntInRangeWithout(withoutThisInt, maxRangeExclusive);
    }

    private static List<int> GetRandomUniqueNumbersWithNegative(int maxRangeExclusive, int amount) {
        List<int> list = new List<int>();

        if (amount < 1) return list;

        while (list.Count != amount) {
            int random = Random.Range(-maxRangeExclusive + 1, maxRangeExclusive);
            if (!list.Contains(random)) {
                list.Add(random);
            }
        }

        return list;
    }

    /// <summary>
    /// For a given range, the number of positions and spacing, return a list of spaces that can fit (spacing),
    /// with a random offset in between ^^
    /// </summary>
    private static List<int> GetRandomisedPositionsInRange(int maxRangeExclusive, int amount, int spacing) {
        List<int> list = new List<int>();

        // Double range is positive representation of doublesided range (going from 0 to - and + equally)
        int doubleRange = (maxRangeExclusive * 2) - 1;

        if (!CanItFit(doubleRange, amount, spacing)) return default;

        // Spawn the first one
        for (int i = 0; i < doubleRange; i++) {
            if (IsNumberSpacedInList(list, i, spacing)) {
                list.Add(i);
                break;
            }
        }

        if (list.Count < 1) return default;

        for (int i = 1; i < amount; i++) {
            list.Add(list[list.Count - 1] + Random.Range(0, spacing));
        }

        // Get Back doublesided values
        for (int i = 0; i < list.Count; i++) {
            list[i] = list[i] - maxRangeExclusive;
        }

        return list;
    }

    private static bool CanItFit(int size, int amount, int spacing) {
        int randomOffsetMax = size / amount;
        int neededSpaceForAmount = (amount * spacing) + ((amount - 1) * randomOffsetMax);

        // There needs to be more space for a random with the spacing and the amount for the given range
        if (size < neededSpaceForAmount) return false;

        return true;
    }

    private static bool IsNumberSpacedInList(List<int> list, int number, int spacing) {
        foreach (var pos in list) {
            if (Math.Abs(number - pos) < spacing) return false;
        }

        return true;
    }
}
