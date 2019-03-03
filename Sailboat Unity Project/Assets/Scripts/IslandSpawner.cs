using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    public GameObject IslandPrefab; //Obs can also be a floating log, that works the same
    public Vector3 islandVelocity;
    public float timeBetween;
    public float timeOffset;
    public float xLength, ylength;

    // Use this for initialization
    void Start()
    {
        InitialIslands();
        StartCoroutine(SpawnIslands());
    }

    //Makes 3 islands like if SpawnIslands had been running before start
    private void InitialIslands()
    {
        for (int i = 0; i < 3; i++)
        {
        GameObject island = SpawnIsland();
        Transform islandTransform = island.GetComponent<Transform>();
        islandTransform.position += islandVelocity * (timeBetween * i + timeOffset);
        }

    }

    IEnumerator SpawnIslands()
    {
        yield return new WaitForSeconds(timeOffset);
        while (true)
        {
            SpawnIsland();
            yield return new WaitForSeconds(timeBetween);
        }
    }

    private GameObject SpawnIsland()
    {
        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = Quaternion.identity;
        GameObject island = Instantiate(IslandPrefab, spawnPosition, spawnRotation);
        island.transform.localScale = new Vector3(xLength, ylength, 1f);
        Rigidbody islandRB = island.GetComponent<Rigidbody>();
        islandRB.velocity = islandVelocity;
        return island;
    }
}