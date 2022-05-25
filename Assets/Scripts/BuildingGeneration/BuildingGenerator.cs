using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuildingGeneration
{
    public class BuildingGenerator : MonoBehaviour
    {
        #region Serialized Fields

        [Header("First Floor")] [SerializeField]
        private List<GameObject> foundationWalls;
        [SerializeField] private List<GameObject> stairs;
        [SerializeField] private List<GameObject> pillars;

        [Header("Second Floor")] [SerializeField]
        private List<GameObject> buildingWalls;
        [SerializeField] private List<GameObject> floorCornerFences;
        [SerializeField] private List<GameObject> floorSideFences;
        [SerializeField] private List<GameObject> floorSidePillars;

        [Header("Third Floor")] [SerializeField]
        private List<GameObject> gableRoofs;
        [SerializeField] private List<GameObject> innerCornerRoof;
        [SerializeField] private List<GameObject> outerCornerRoofs;
        [SerializeField] private List<GameObject> sideRoofs;

        #endregion

        #region Private Fields

        

        #endregion
        
        public void Generate()
        {
            Instantiate(buildingWalls[0], transform);
            //F1
            //Create starting foundation
            //Surround with pillars (can be front or surround)
            //add access point stairs (can be multiple)

            //F2
            //select a doorway (only 1)
            //add walls to foundation
            //add wooden floor + pillars to exist in pillars

            //F3
            //Add roof
        }

        public void ClearBuilding()
        {
            var children = transform.Cast<Transform>().ToList();

            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}