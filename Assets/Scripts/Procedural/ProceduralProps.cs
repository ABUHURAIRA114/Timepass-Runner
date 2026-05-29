using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralProps : MonoBehaviour
{
    [SerializeField] private List<Transform> propsPrefab, spawnPoints;
    [SerializeField] private int minProps, maxProps;

    void Start()
    {
        SpawnProps();
    }

    private void SpawnProps()
    {
        int randomProps = Random.Range(minProps, maxProps);
        HashSet<int> usedSpawnPoints = new();

        for (int i = 0; i < randomProps; i++)
        {
            int randomProp = Random.Range(0, propsPrefab.Count);
            int randomPoint = GetRandomUnusedSpawnPoint(usedSpawnPoints);

            Instantiate(propsPrefab[randomProp], spawnPoints[randomPoint]);

            usedSpawnPoints.Add(randomPoint);
        }
    }

    private int GetRandomUnusedSpawnPoint(HashSet<int> usedSpawnPoints)
    {
        int randomPoint = Random.Range(0, spawnPoints.Count);

        while (usedSpawnPoints.Contains(randomPoint))
        {
            randomPoint = Random.Range(0, spawnPoints.Count);
        }

        return randomPoint;
    }
}
