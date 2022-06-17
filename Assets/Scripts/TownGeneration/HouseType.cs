using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class HouseType
{
    [SerializeField]private GameObject[] prefabs;
    public int sizeRequired;
    public int quantity;
    public int quantityAlreadyPlaced;
    
    [Header("Building Values")]

    public Vector2Int wallWidthRange = new Vector2Int(2,3);
    public Vector2Int wallLengthRange = new Vector2Int(4,6);
    public Vector2Int wallHeightRange = new Vector2Int(2,3);

    public Vector2Int maxDoorsRange = new Vector2Int(1,2);
    [Range(0, 1)] public float doorSpawnChance = 0.5f;
    public Vector2Int maxWindowsRange = new Vector2Int(2,4);
    [Range(0, 1)] public float windowSpawnChance = 0.5f;

    public GameObject GetPrefab()
    {
        quantityAlreadyPlaced++;
        if (prefabs.Length <= 1) return prefabs[0];
        var random = Random.Range(0, prefabs.Length);
        return prefabs[random];

    }

    public bool IsBuildingAvailable()
    {
        return quantityAlreadyPlaced < quantity;
    }

    public void Reset()
    {
        quantityAlreadyPlaced = 0;
    }
}
