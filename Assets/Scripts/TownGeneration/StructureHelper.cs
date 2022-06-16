using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownGeneration
{
    public class StructureHelper : MonoBehaviour
    {
        public GameObject prefab;
        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

        public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
        {
            var freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
            foreach (var freeSpot in freeEstateSpots)
            {
                var rotation = Quaternion.identity;
                switch (freeSpot.Value)
                {
                    case Direction.Up:
                        rotation=Quaternion.Euler(0,90,0);
                        break;
                    case Direction.Down:
                        rotation = Quaternion.Euler(0,-90,0);
                        break;
                    case Direction.Right:
                        rotation = Quaternion.Euler(0,180,0);
                        break;
                    case Direction.Left:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Instantiate(prefab, freeSpot.Key, rotation, transform);
            }
        }

        private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
        {
            var freeSpaces = new Dictionary<Vector3Int, Direction>();
            foreach (var position in roadPositions)
            {
                var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (neighbourDirections.Contains(direction) == false)
                    {
                        var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                        if (freeSpaces.ContainsKey(newPosition))
                        {
                            continue;
                        }

                        freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                    }
                }
            }

            return freeSpaces;
        }
    }
}