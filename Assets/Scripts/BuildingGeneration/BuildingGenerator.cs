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

        [Header("Building Values")] [SerializeField]
        private Vector3 offsetAmounts;

        [SerializeField] private int wallWidth = 1;
        [SerializeField] private int wallLength = 1;
        [SerializeField] private int wallHeight = 1;

        #endregion

        #region Private Fields

        private float yOffset;
        private float xOffset;
        private float zOffset;

        #endregion

        public void Generate()
        {
            ClearBuilding();

            xOffset = 0;
            yOffset = 0;
            zOffset = 0;

            //F1
            //Create starting foundation
            CreateWalls(foundationWalls);

            //Surround with pillars (can be front or surround)
            //add access point stairs (can be multiple)

            //F2
            //select a doorway (only 1)

            //add walls to foundation

            #region wall creation

            for (var b = 0; b < wallHeight; b++)
            {
                CreateWalls(buildingWalls);
            }

            #endregion

            //add wooden floor + pillars to existing in pillars

            //F3
            //Add roof
            CreateRoofs();
        }

        public void ClearBuilding()
        {
            var children = transform.Cast<Transform>().ToList();

            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void CreateWalls(IReadOnlyList<GameObject> walls)
        {
            Vector3 pos;
            var transform1 = transform;
            var position = transform1.position;
            GameObject tempGameObject;

            //1st wall
            for (var i = 0; i < wallWidth; i++)
            {
                pos = new Vector3(xOffset, yOffset, zOffset);
                Instantiate(walls[0], pos + position, Quaternion.identity, transform1);
                zOffset += offsetAmounts.z;
            }

            zOffset -= offsetAmounts.z;

            //2nd wall

            for (var i = 0; i < wallLength; i++)
            {
                pos = new Vector3(xOffset, yOffset, zOffset);
                tempGameObject = Instantiate(walls[0], pos + position, Quaternion.identity, transform1);
                tempGameObject.transform.Rotate(0, 90, 0);
                xOffset += offsetAmounts.x;
            }

            xOffset -= offsetAmounts.x;

            //3rd wall

            for (var i = 0; i < wallWidth; i++)
            {
                pos = new Vector3(xOffset, yOffset, zOffset);
                tempGameObject = Instantiate(walls[0], pos + position, Quaternion.identity, transform1);
                tempGameObject.transform.Rotate(0, 180, 0);
                zOffset -= offsetAmounts.z;
            }

            zOffset += offsetAmounts.z;

            //4th wall

            for (var i = 0; i < wallLength; i++)
            {
                pos = new Vector3(xOffset, yOffset, zOffset);
                tempGameObject = Instantiate(walls[0], pos + position, Quaternion.identity, transform1);
                tempGameObject.transform.Rotate(0, 270, 0);
                xOffset -= offsetAmounts.x;
            }

            xOffset = 0;
            zOffset = 0;
            yOffset += offsetAmounts.y;
        }

        private void CreateRoofs()
        {
            Vector3 pos;
            var transform1 = transform;
            var position = transform1.position;
            var rotAmount = 90;
            GameObject tempGameObject;
            
            for (var i = 0; i < wallWidth; i++)
            {
                for (var j = 0; j < wallLength; j++)
                {
                    pos = new Vector3(xOffset, yOffset, zOffset);
                    tempGameObject = Instantiate(sideRoofs[0], pos + position, Quaternion.identity, transform1);
                    tempGameObject.transform.Rotate(0, rotAmount, 0);
                    xOffset += offsetAmounts.x;
                }
                
                //if uneven amount

                //if at transition point
                if (i==wallWidth/2-1)
                {
                    rotAmount = 270; 
                }
                //if go up
                else if (i<wallWidth/2)
                {
                    yOffset += offsetAmounts.y;
                }
                //if go down
                else
                {
                    yOffset -= offsetAmounts.y;
                }

                xOffset = 0;
                zOffset += offsetAmounts.z;
            }
        }
    }
}