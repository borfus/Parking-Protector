using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance;
    public GameObject CatapaultPrefab;
    public Transform CatapaultPrefabSpawn;

    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    public void SpawnCatapault()
    {
        Instantiate(CatapaultPrefab, CatapaultPrefabSpawn.transform.position, Quaternion.identity);
    }
}
