using System.Collections.Generic;
using UnityEngine;

public class RandomBoils : MonoBehaviour
{
    [Header("Boil Settings")]
    public GameObject[] boilPrefabs; // Different boil variations
    public Transform[] spawnBones;   // Bone locations for possible boils
    [Range(3, 15)] public int minBoils = 5;
    [Range(3, 15)] public int maxBoils = 8;
 //   public int randomSeed = 12345;

    [Header("Debug")]
    public bool spawnOnStart = true;

    private List<GameObject> spawnedBoils = new List<GameObject>();

    void Start()
    {
        if (spawnOnStart)
            SpawnBoils();
    }

    public void SpawnBoils()
    {
        // Clear previous boils if needed
        foreach (var boil in spawnedBoils)
        {
            if (boil != null) Destroy(boil);
        }
        spawnedBoils.Clear();

        // Apply seed for consistent results
     //   Random.InitState(randomSeed);

        int boilCount = Random.Range(minBoils, maxBoils + 1);
        List<int> usedBones = new List<int>();

        for (int i = 0; i < boilCount; i++)
        {
            // Pick random unused bone
            int boneIndex;
            do
            {
                boneIndex = Random.Range(0, spawnBones.Length);
            } while (usedBones.Contains(boneIndex));

            usedBones.Add(boneIndex);

            Transform bone = spawnBones[boneIndex];

            // Pick random boil prefab
            GameObject boilPrefab = boilPrefabs[Random.Range(0, boilPrefabs.Length)];

            // Slightly offset the boil so it’s not always at bone center
            Vector3 offset = new Vector3(
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f)
            );

            GameObject boil = Instantiate(boilPrefab, bone.position + offset, bone.rotation, bone);
            spawnedBoils.Add(boil);
        }
    }
}
