using UnityEngine;

namespace Demo
{
    /// <summary>
    /// This is the component that holds the grid data, used by the MarchingSquares component.
    /// It is also responsible for mapping world positions to grid cell positions.
    /// Other scripts can call SetCell to modify the grid, before running the MarchingSquares algorithm.
    /// 
    /// You can try out different ways to initialize the grid here, in the InitializeGrid method.
    /// </summary>
    public class ValueGrid : MonoBehaviour
    {
        public int Width => width;

        public int Depth => depth;

        [SerializeField] private int width = 10;
        [SerializeField] private int depth = 10;

        public float cellSize = 1;

        private float[,] grid = null;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.G)) return;
            InitializeGrid();
            Debug.Log("ValueGrid: newly initialized. Press the BuildTrigger key to regenerate game objects");
        }

        private void InitializeGrid()
        {
            grid = new float[width, depth];

            // TODO: try out some interesting ways to initialize the grid here:
            var xOffset = Random.value;
            var yOffset = Random.value;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < depth; j++)
                {
                    // Initialize empty:
                    //grid[i, j]=0; 

                    // Perlin noise with random offset:
                    grid[i, j] = Mathf.Round(Mathf.PerlinNoise(i * 0.1f + xOffset, j * 0.1f + yOffset));

                    //Rows of roads
                    //var c=0;
                    //if (j % 2 == 0)
                    //{
                    //    c = 1;
                    //}
                    //grid[i, j] = c;
                }
            }
        }

        private bool GetRowCol(Vector3 worldPosition, out int row, out int col)
        {
            var localHit = transform.InverseTransformPoint(worldPosition);

            row = (int) Mathf.Round(localHit.x / cellSize);
            col = (int) Mathf.Round(localHit.z / cellSize);
            return InRange(row, col);
        }

        private bool InRange(int row, int col)
        {
            if (grid == null)
            {
                InitializeGrid();
            }

            return row >= 0 && row < grid.GetLength(0) && col >= 0 && col < grid.GetLength(1);
        }

        private void SetCell(int row, int col, float value)
        {
            if (grid == null)
            {
                InitializeGrid();
            }

            if (InRange(row, col))
            {
                grid[row, col] = value;
            }
        }

        public float GetCell(int row, int col)
        {
            if (grid == null)
            {
                InitializeGrid();
            }

            if (InRange(row, col))
            {
                return grid[row, col];
            }

            return 0;
        }

        public void SetCell(Vector3 worldPosition, float value)
        {
            if (GetRowCol(worldPosition, out int row, out int col))
            {
                SetCell(row, col, value);
            }
        }

        public float GetCell(Vector3 worldPosition, float value)
        {
            if (GetRowCol(worldPosition, out int row, out int col))
            {
                return GetCell(row, col);
            }

            return 0;
        }
    }
}