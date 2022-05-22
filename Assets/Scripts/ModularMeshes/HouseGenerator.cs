using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModularMeshes
{
    public class HouseGenerator : Shape
    {
        #region SerializedFields

        [Header("Build Settings")] [SerializeField]
        private int width;

        [SerializeField] private int depth;

        [Header("Foundations")] [SerializeField]
        private GameObject[] foundationWalls;

        [SerializeField] private GameObject[] outerPillars;
        [SerializeField] private GameObject[] stairs;

        [Header("OutsideFlooring")] [SerializeField]
        private GameObject[] floorCorner;

        [SerializeField] private GameObject[] floorSide;

        [Header("Roofing")] [SerializeField] private GameObject[] gableRoofs;
        [SerializeField] private GameObject[] innerCornerRoofs;
        [SerializeField] private GameObject[] outerCornerRoofs;
        [SerializeField] private GameObject[] sideRoofs;

        #endregion

        #region PrivateFields
        
        private const float StockContinueChance = 0.5f;
        
        #endregion

        public void Initialize(int width, int depth, GameObject[] foundationWalls, GameObject[] gableRoofs)
        {
            this.width=width;
            this.depth=depth;
            this.foundationWalls=foundationWalls;
            this.gableRoofs=gableRoofs;
        }

        protected override void Execute()
        {
            // Create four walls:
            for (var i = 0; i<4; i++) {
                var localPosition = i switch
                {
                    0 => new Vector3(-(width - 1) * 0.5f, 0, 0) // left
                    ,
                    1 => new Vector3(0, 0, (depth - 1) * 0.5f) // back
                    ,
                    2 => new Vector3((width - 1) * 0.5f, 0, 0) // right
                    ,
                    3 => new Vector3(0, 0, -(depth - 1) * 0.5f) // front
                    ,
                    _ => new Vector3()
                };
                var newRow = CreateSymbol<SimpleRow>("wall", localPosition, Quaternion.Euler(0, i*90, 0));
                newRow.Initialize(i%2==1 ? width : depth,foundationWalls);
                newRow.Generate();
            }		

            // Continue with a stock or with a roof (random choice):
            var randomValue = RandomFloat();
            if (randomValue < StockContinueChance) {
                var nextStock = CreateSymbol<HouseGenerator>("stock", new Vector3(0, 1, 0));
                nextStock.Initialize(width, depth, foundationWalls, gableRoofs);
                nextStock.Generate();
            } else {
                var nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(0, 1, 0));
                nextRoof.Initialize(width, depth, gableRoofs,foundationWalls);
                nextRoof.Generate();
            }
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
/*
 * // grammar rule probabilities:
        private const float StockContinueChance = 0.5f;

        // shape parameters:
        [SerializeField] private int width;
        [SerializeField] private int depth;

        [SerializeField] private GameObject[] wallStyle;
        [SerializeField] private GameObject[] roofStyle;

        private const float BuildDelay = 0.1f;
		
        public void Initialize(int width, int depth, GameObject[] wallStyle, GameObject[] roofStyle) {
            this.width=width;
            this.depth=depth;
            this.wallStyle=wallStyle;
            this.roofStyle=roofStyle;
        }

        protected override void Execute() {
            // Create four walls:
            for (var i = 0; i<4; i++) {
                var localPosition = i switch
                {
                    0 => new Vector3(-(width - 1) * 0.5f, 0, 0) // left
                    ,
                    1 => new Vector3(0, 0, (depth - 1) * 0.5f) // back
                    ,
                    2 => new Vector3((width - 1) * 0.5f, 0, 0) // right
                    ,
                    3 => new Vector3(0, 0, -(depth - 1) * 0.5f) // front
                    ,
                    _ => new Vector3()
                };
                var newRow = CreateSymbol<SimpleRow>("wall", localPosition, Quaternion.Euler(0, i*90, 0));
                newRow.Initialize(i%2==1 ? width : depth,wallStyle);
                newRow.Generate();
            }		

            // Continue with a stock or with a roof (random choice):
            var randomValue = RandomFloat();
            if (randomValue < StockContinueChance) {
                var nextStock = CreateSymbol<SimpleStock>("stock", new Vector3(0, 1, 0));
                nextStock.Initialize(width, depth, wallStyle, roofStyle);
                nextStock.Generate(BuildDelay);
            } else {
                var nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(0, 1, 0));
                nextRoof.Initialize(width, depth, roofStyle,wallStyle);
                nextRoof.Generate(BuildDelay);
            }
        }
*/