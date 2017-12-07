using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCreator : MonoBehaviour 
{
    public List<DownFloor> FloorPrefabs;
    public int Rows = 10;
    public int Columns = 10;

    private DownFloor[,] floorGrid;
    private Queue<GridPosition> gridPositions;

    private float offset = 2.25f;

    private struct GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }
    }

    private void Start()
    {
        floorGrid = new DownFloor[Columns, Rows];

        for(int rowIndex = 0; rowIndex < Rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < Columns; columnIndex++)
            {
                floorGrid[columnIndex, rowIndex] = null;
            }
        }

        gridPositions = new Queue<GridPosition>();

        int initialColPos = Columns / 2;
        int initialRowPos = Rows / 2;

        floorGrid[initialColPos, initialRowPos] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], Vector3.zero, Quaternion.identity);
        gridPositions.Enqueue(new GridPosition(initialColPos, initialRowPos));

        var chance = 0;
        while(gridPositions.Count > 0)
        {
            var currentFloorPosition = gridPositions.Dequeue();
            var currentFloor = floorGrid[currentFloorPosition.x, currentFloorPosition.y];
            var up = new GridPosition(currentFloorPosition.x, currentFloorPosition.y - 1);
            var down = new GridPosition(currentFloorPosition.x, currentFloorPosition.y + 1);
            var left = new GridPosition(currentFloorPosition.x - 1, currentFloorPosition.y);
            var right = new GridPosition(currentFloorPosition.x + 1, currentFloorPosition.y);

            if(up.x >= 0 && up.x < Columns && up.y >= 0 && up.y < Rows)
            {
                var upChange = Random.Range(0, 79);
                if(floorGrid[up.x, up.y] == null && upChange >= chance)
                {
                    var pos = currentFloor.transform.position;
                    pos.z = pos.z - offset;
                    floorGrid[up.x, up.y] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], pos, Quaternion.identity);
                    gridPositions.Enqueue(up);
                }
            }

            if(down.x >= 0 && down.x < Columns && down.y >= 0 && down.y < Rows)
            {
                var downChange = Random.Range(0, 79);
                if(floorGrid[down.x, down.y] == null && downChange >= chance)
                {
                    var pos = currentFloor.transform.position;
                    pos.z = pos.z + offset;
                    floorGrid[down.x, down.y] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], pos, Quaternion.identity);
                    gridPositions.Enqueue(down);
                }
            }

            if(left.x >= 0 && left.x < Columns && left.y >= 0 && left.y < Rows)
            {
                var leftChange = Random.Range(0, 79);
                if(floorGrid[left.x, left.y] == null && leftChange >= chance)
                {
                    var pos = currentFloor.transform.position;
                    pos.x = pos.x - offset;
                    floorGrid[left.x, left.y] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], pos, Quaternion.identity);
                    gridPositions.Enqueue(left);
                }
            }

            if(right.x >= 0 && right.x < Columns && right.y >= 0 && right.y < Rows)
            {
                var rightChange = Random.Range(0, 79);
                if(floorGrid[right.x, right.y] == null && rightChange >= chance)
                {
                    var pos = currentFloor.transform.position;
                    pos.x = pos.x + offset;
                    floorGrid[right.x, right.y] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], pos, Quaternion.identity);
                    gridPositions.Enqueue(right);
                }
            }

            chance += 2;
        }
    }
}
