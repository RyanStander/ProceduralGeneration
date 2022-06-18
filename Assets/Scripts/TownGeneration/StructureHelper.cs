using System;
using System.Collections.Generic;
using BuildingGeneration;
using UnityEngine;

namespace TownGeneration
{
    public class StructureHelper : MonoBehaviour
    {
        public RandomGenerator randomGenerator;
        [Header("Decoration")] public GameObject[] decorationPrefabs;
        public bool randomDecorationsPlacement;
        [Range(0, 1)] public float randomDecorationsPlacementThreshold = 0.3f;
        public Dictionary<Vector3Int, GameObject> decorationsDictionary = new Dictionary<Vector3Int, GameObject>();
        [Header("Buildings")] public HouseType[] buildingTypes;
        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

        private void OnValidate()
        {
            if (randomGenerator==null)
                randomGenerator = GetComponent<RandomGenerator>();
        }
        
        public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
        {
            var freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
            var blockedPositions = new List<Vector3Int>();
            foreach (var freeSpot in freeEstateSpots)
            {
                if (blockedPositions.Contains(freeSpot.Key))
                {
                    continue;
                }

                var rotation = Quaternion.Euler(0, DetermineRotation(freeSpot.Value), 0);


                for (var i = 0; i < buildingTypes.Length; i++)
                {
                    if (buildingTypes[i].quantity == -1)
                    {
                        //handle decoration placement
                        if (randomDecorationsPlacement)
                        {
                            var random = randomGenerator.NextDouble();
                            if (random < randomDecorationsPlacementThreshold)
                            {
                                PlaceDecoration(rotation,freeSpot,ref freeEstateSpots, ref blockedPositions);
                                break;
                            }
                        }

                        PlaceBuilding(i, rotation, freeSpot, ref freeEstateSpots, ref blockedPositions);
                        break;
                    }

                    if (!buildingTypes[i].IsBuildingAvailable()) continue;

                    PlaceBuilding(i, rotation, freeSpot, ref freeEstateSpots, ref blockedPositions);
                    break;
                }
            }
        }

        private void PlaceBuilding(
            int index,
            Quaternion rotation,
            KeyValuePair<Vector3Int, Direction> freeSpot,
            ref Dictionary<Vector3Int, Direction> freeEstateSpots,
            ref List<Vector3Int> blockedPositions
        )
        {
            if (buildingTypes[index].sizeRequired > 1)
            {
                var halfSize = Mathf.CeilToInt(buildingTypes[index].sizeRequired / 2.0f);
                var tempPositionsBlocked = new List<Vector3Int>();
                if (!VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions,
                    ref tempPositionsBlocked)) return;

                blockedPositions.AddRange(tempPositionsBlocked);
                var building = SpawnPrefab(buildingTypes[index], freeSpot.Key, rotation, freeSpot.Value);
                structuresDictionary.Add(freeSpot.Key, building);
                foreach (var pos in tempPositionsBlocked)
                {
                    structuresDictionary.Add(pos, building);
                }
            }
            else
            {
                var building = SpawnPrefab(buildingTypes[index], freeSpot.Key, rotation, freeSpot.Value);
                structuresDictionary.Add(freeSpot.Key, building);
            }
        }

        private void PlaceDecoration(Quaternion rotation,
            KeyValuePair<Vector3Int, Direction> freeSpot,
            ref Dictionary<Vector3Int, Direction> freeEstateSpots,
            ref List<Vector3Int> blockedPositions)
        {
            var halfSize = Mathf.CeilToInt(1 / 2.0f);
            var tempPositionsBlocked = new List<Vector3Int>();
            if (!VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions,
                ref tempPositionsBlocked)) return;
            
            blockedPositions.AddRange(tempPositionsBlocked);
            var decoration =
                SpawnDecoration(
                    decorationPrefabs[randomGenerator.Next(0, decorationPrefabs.Length, false)],
                    freeSpot.Key, rotation);
            decorationsDictionary.Add(freeSpot.Key, decoration);
        }

        private static bool VerifyIfBuildingFits(int halfSize,
            IReadOnlyDictionary<Vector3Int, Direction> freeEstateSpots,
            KeyValuePair<Vector3Int, Direction> freeSpot,
            ICollection<Vector3Int> blockedPositions,
            ref List<Vector3Int> tempPositionsBlocked)
        {
            Vector3Int direction;
            if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
            {
                direction = Vector3Int.right;
            }
            else
            {
                direction = new Vector3Int(0, 0, 1);
            }

            for (var i = 1; i <= halfSize; i++)
            {
                var pos1 = freeSpot.Key + direction * i;
                var pos2 = freeSpot.Key - direction * i;
                if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2) ||
                    blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
                {
                    return false;
                }

                tempPositionsBlocked.Add(pos1);
                tempPositionsBlocked.Add(pos2);
            }

            return true;
        }

        private GameObject SpawnPrefab(HouseType buildingType, Vector3Int position,
            Quaternion rotation, Direction direction)
        {
            var newStructure = Instantiate(buildingType.GetPrefab(), position, rotation, transform);
            var buildingGenerator = newStructure.GetComponentInChildren<BuildingGenerator>();

            buildingGenerator.SetBuildingGeneratorValues(
                randomGenerator.Next(buildingType.wallWidthRange.x, buildingType.wallWidthRange.y),
                randomGenerator.Next(buildingType.wallLengthRange.x, buildingType.wallLengthRange.y),
                randomGenerator.Next(buildingType.wallHeightRange.x, buildingType.wallHeightRange.y),
                DetermineRotation(direction),
                randomGenerator.Next(buildingType.maxDoorsRange.x, buildingType.maxDoorsRange.y),
                buildingType.doorSpawnChance,
                randomGenerator.Next(buildingType.maxWindowsRange.x, buildingType.maxWindowsRange.y),
                buildingType.windowSpawnChance);

            buildingGenerator.buildingYRotation = DetermineRotation(direction);

            buildingGenerator.Generate(randomGenerator);

            return newStructure;
        }

        private GameObject SpawnDecoration(GameObject prefab, Vector3Int position, Quaternion rotation)
        {
            var newDecoration = Instantiate(prefab, position, rotation, transform);

            return newDecoration;
        }

        private static Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
        {
            var freeSpaces = new Dictionary<Vector3Int, Direction>();
            foreach (var position in roadPositions)
            {
                var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (neighbourDirections.Contains(direction)) continue;

                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);

                    if (freeSpaces.ContainsKey(newPosition)) continue;

                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }

            return freeSpaces;
        }

        private static int DetermineRotation(Direction direction)
        {
            return direction switch
            {
                Direction.Up => 180,
                Direction.Right => 270,
                Direction.Down => 0,
                Direction.Left => 90,
                _ => 0
            };
        }

        public void Reset()
        {
            foreach (var item in structuresDictionary.Values)
            {
                DestroyImmediate(item);
            }
            
            foreach (var item in decorationsDictionary.Values)
            {
                DestroyImmediate(item);
            }

            structuresDictionary.Clear();
            decorationsDictionary.Clear();

            foreach (var buildingType in buildingTypes)
            {
                buildingType.Reset();
            }
        }
    }
}