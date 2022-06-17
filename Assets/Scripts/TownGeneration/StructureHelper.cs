using System;
using System.Collections.Generic;
using BuildingGeneration;
using UnityEngine;

namespace TownGeneration
{
    public class StructureHelper : MonoBehaviour
    {
        public RandomGenerator randomGenerator;
        public HouseType[] buildingTypes;
        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

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
                randomGenerator.IntBetweenRangeInclusive(buildingType.wallWidthRange.x, buildingType.wallWidthRange.y),
                randomGenerator.IntBetweenRangeInclusive(buildingType.wallLengthRange.x, buildingType.wallLengthRange.y),
                randomGenerator.IntBetweenRangeInclusive(buildingType.wallHeightRange.x, buildingType.wallHeightRange.y),
                DetermineRotation(direction),
                randomGenerator.IntBetweenRangeInclusive(buildingType.maxDoorsRange.x, buildingType.maxDoorsRange.y),
                buildingType.doorSpawnChance,
                randomGenerator.IntBetweenRangeInclusive(buildingType.maxWindowsRange.x, buildingType.maxWindowsRange.y),
                buildingType.windowSpawnChance);

            buildingGenerator.buildingYRotation = DetermineRotation(direction);

            buildingGenerator.Generate();

            return newStructure;
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
                Destroy(item);
            }

            structuresDictionary.Clear();

            foreach (var buildingType in buildingTypes)
            {
                buildingType.Reset();
            }
        }
    }
}