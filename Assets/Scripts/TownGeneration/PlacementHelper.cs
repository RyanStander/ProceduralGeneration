using System.Collections.Generic;
using UnityEngine;

namespace TownGeneration
{
    public static class PlacementHelper
    {
        public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection)
        {
            var neighbourDirections = new List<Direction>();
            if (collection.Contains(position + new Vector3Int(1, 0, 0)))
            {
                neighbourDirections.Add(Direction.Right);
            }
            if (collection.Contains(position - new Vector3Int(1, 0, 0)))
            {
                neighbourDirections.Add(Direction.Left);
            }
            if (collection.Contains(position + new Vector3Int(0, 0, 1)))
            {
                neighbourDirections.Add(Direction.Up);
            }
            if (collection.Contains(position - new Vector3Int(0, 0, 1)))
            {
                neighbourDirections.Add(Direction.Down);
            }

            return neighbourDirections;
        }
    }
}
