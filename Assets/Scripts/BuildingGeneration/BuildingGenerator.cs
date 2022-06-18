using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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

        [SerializeField] private List<GameObject> buildingDoors;
        [SerializeField] private List<GameObject> buildingWindows;


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

        [SerializeField] private int maxDoors = 1;
        [Range(0, 1)] [SerializeField] private float doorSpawnChance = 0.5f;
        [SerializeField] private int maxWindows = 2;
        [Range(0, 1)] [SerializeField] private float windowSpawnChance = 0.5f;

        public int buildingYRotation = 90;

        #endregion

        #region Private Fields

        private float yOffset;
        private float xOffset;
        private float zOffset;

        [SerializeField]private RandomGenerator randomGenerator;

        private int doorsLeftToSpawn;
        private int windowsLeftToSpawn;

        #endregion

        public void Generate(RandomGenerator givenRandomGenerator=null)
        {
            ClearBuilding();

            if (givenRandomGenerator!=null)
            {
                randomGenerator=givenRandomGenerator;
            }
            
            xOffset = -(wallLength*offsetAmounts.x)/2; 
            zOffset = 0;
            yOffset = 0;


            doorsLeftToSpawn = maxDoors;
            windowsLeftToSpawn = maxWindows;

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
                CreateWalls(buildingWalls, (b==0), true);
            }

            #endregion

            //add wooden floor + pillars to existing in pillars

            //F3
            //Add roof
            CreateRoofs();
            
            transform.Rotate(0,buildingYRotation,0);
        }

        public void SetBuildingGeneratorValues(int givenWallWidth, int givenWallLength, int givenWallHeight,int givenBuildingYRotation,int givenMaxDoors=1,float givenDoorSpawnChance=0.5f, int givenMaxWindows=2,float givenWindowSpawnChance=0.25f)
        {
            wallWidth = givenWallWidth;
            wallLength = givenWallLength;
            wallHeight = givenWallHeight;
            maxDoors = givenMaxDoors;
            doorSpawnChance = givenDoorSpawnChance;
            maxWindows = givenMaxWindows;
            windowSpawnChance = givenWindowSpawnChance;
            buildingYRotation = givenBuildingYRotation;
        }

        public void ClearBuilding()
        {
            var children = transform.Cast<Transform>().ToList();

            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        #region Wall functions

        private void CreateWalls(IReadOnlyList<GameObject> walls, bool createDoors = false, bool createWindows = false)
        {
            var transform1 = transform;
            var position = transform1.position;

            //1st wall
            for (var i = 0; i < wallWidth; i++)
            {
                DetermineWallTypeToSpawn(walls, transform1, position, 0, createDoors, createWindows);
                zOffset += offsetAmounts.z;
            }

            zOffset -= offsetAmounts.z;

            //2nd wall

            for (var i = 0; i < wallLength; i++)
            {
                DetermineWallTypeToSpawn(walls, transform1, position, 90, createDoors, createWindows);
                xOffset += offsetAmounts.x;
            }

            xOffset -= offsetAmounts.x;

            //3rd wall

            for (var i = 0; i < wallWidth; i++)
            {
                DetermineWallTypeToSpawn(walls, transform1, position, 180, createDoors, createWindows);
                zOffset -= offsetAmounts.z;
            }

            zOffset += offsetAmounts.z;

            //4th wall

            for (var i = 0; i < wallLength; i++)
            {
                DetermineWallTypeToSpawn(walls, transform1, position, 270, createDoors, createWindows);
                xOffset -= offsetAmounts.x;
            }

            xOffset = -(wallLength*offsetAmounts.x)/2; 
            zOffset = 0;
            yOffset += offsetAmounts.y;
        }

        private void SpawnWall(IReadOnlyList<GameObject> walls, Transform transform1, Vector3 position, float rotation)
        {
            var pos = new Vector3(xOffset, yOffset, zOffset);
            var tempGameObject = Instantiate(walls[0], pos + position, Quaternion.identity, transform1);
            tempGameObject.transform.Rotate(0, rotation, 0);
        }

        private void DetermineWallTypeToSpawn(IReadOnlyList<GameObject> walls, Transform transform1, Vector3 position,
            float rotation, bool createDoors, bool createWindows)
        {
            if (doorsLeftToSpawn > 0 && randomGenerator.NextDouble() <= doorSpawnChance && createDoors)
            {
                doorsLeftToSpawn--;
                SpawnWall(buildingDoors, transform1, position, rotation);
            }
            else if (windowsLeftToSpawn > 0 && randomGenerator.NextDouble() <= windowSpawnChance && createWindows)
            {
                windowsLeftToSpawn--;
                SpawnWall(buildingWindows, transform1, position, rotation);
            }
            else
            {
                SpawnWall(walls, transform1, position, rotation);
            }
        }

        #endregion

        private void CreateRoofs()
        {
            Vector3 pos;
            var transform1 = transform;
            var position = transform1.position;
            GameObject tempGameObject;

            for (var i = 0; i < wallWidth / 2; i++)
            {
                //create side roof
                for (var j = 0; j < wallLength; j++)
                {
                    pos = new Vector3(xOffset, yOffset, zOffset);
                    tempGameObject = Instantiate(sideRoofs[0], pos + position, Quaternion.identity, transform1);
                    tempGameObject.transform.Rotate(0, 90, 0);
                    xOffset += offsetAmounts.x;
                }

                zOffset += offsetAmounts.z;
                xOffset -= offsetAmounts.x;

                //create side wall
                for (var j = 0; j < wallWidth; j++)
                {
                    if (j <= i || j >= wallWidth - i - 1) continue;

                    pos = new Vector3(xOffset, yOffset, zOffset);
                    tempGameObject = Instantiate(buildingWalls[0], pos + position, Quaternion.identity, transform1);
                    tempGameObject.transform.Rotate(0, 180, 0);
                    zOffset += offsetAmounts.z;
                }

                //create side roof
                for (var j = 0; j < wallLength; j++)
                {
                    pos = new Vector3(xOffset, yOffset, zOffset);
                    tempGameObject = Instantiate(sideRoofs[0], pos + position, Quaternion.identity, transform1);
                    tempGameObject.transform.Rotate(0, 270, 0);
                    xOffset -= offsetAmounts.x;
                }

                zOffset -= offsetAmounts.z;
                xOffset += offsetAmounts.x;

                //create side wall
                for (var j = 0; j < wallWidth; j++)
                {
                    if (j <= i || j >= wallWidth - i - 1) continue;

                    pos = new Vector3(xOffset, yOffset, zOffset);
                    tempGameObject = Instantiate(buildingWalls[0], pos + position, Quaternion.identity, transform1);
                    tempGameObject.transform.Rotate(0, 0, 0);
                    zOffset -= offsetAmounts.z;
                }

                yOffset += offsetAmounts.y;
                zOffset += offsetAmounts.z;

                if (i == wallWidth / 2 - 1 && wallWidth % 2 != 0)
                {
                    for (var j = 0; j < wallLength; j++)
                    {
                        pos = new Vector3(xOffset, yOffset, zOffset);
                        tempGameObject = Instantiate(gableRoofs[0], pos + position, Quaternion.identity, transform1);
                        tempGameObject.transform.Rotate(0, 0, 0);
                        xOffset += offsetAmounts.x;
                    }
                }

                xOffset = -(wallLength*offsetAmounts.x)/2; 
            }

            //if the width is 1 we want to just add a gable roof to the top
            if (wallWidth != 1) return;

            for (var j = 0; j < wallLength; j++)
            {
                pos = new Vector3(xOffset, yOffset, zOffset);
                tempGameObject = Instantiate(gableRoofs[0], pos + position, Quaternion.identity, transform1);
                tempGameObject.transform.Rotate(0, 0, 0);
                xOffset += offsetAmounts.x;
            }
        }
    }
}