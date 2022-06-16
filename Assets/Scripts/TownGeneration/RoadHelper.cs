using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownGeneration
{
    public class RoadHelper : MonoBehaviour
    {
        public GameObject roadStraight;
        public GameObject roadCorner;
        public GameObject roadThreeWay;
        public GameObject roadFourWay;
        public GameObject roadEnd;
        
        //using int because cant compare floats as easily
        private Dictionary<Vector3Int,GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
        //Need to know where the road corners are
        private HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

        public void PlaceStreetPositions(Vector3 startPosition,Vector3Int direction, float length)
        {
            var rotation = Quaternion.identity;
            if (direction.x==0)
            {
                rotation=Quaternion.Euler(0,90,0);
            }

            for (var i = 0; i < length; i++)
            {
                var position = Vector3Int.RoundToInt(startPosition + direction * i);
                //if road already exists on that position, next loop
                if (roadDictionary.ContainsKey(position))
                {
                    continue;
                }
                var road = Instantiate(roadStraight, position, rotation, transform);
                roadDictionary.Add(position,road);
                //these are where we assume road twists to be
                if (i==0||i==length-1)
                {
                    fixRoadCandidates.Add(position);
                }
            }
        }
        
        public void ClearGeneration()
        {
            fixRoadCandidates = new HashSet<Vector3Int>();
            roadDictionary = new Dictionary<Vector3Int, GameObject>();
            
            var children = transform.Cast<Transform>().ToList();

            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
