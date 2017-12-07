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
    private GameObject floorGridParent;

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

    private void Awake()
    {
        CreateFloor();
        ConnectFloor();
    }

    public List<DownFloor> GetFloors()
    {
        var result = new List<DownFloor>();
        for(int rowIndex = 0; rowIndex < Rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < Columns; columnIndex++)
            {
                var currentFloor = floorGrid[columnIndex, rowIndex];
                if(currentFloor != null)
                {
                    result.Add(currentFloor);
                }
            }
        }

        return result;
    }

    private void CreateFloor()
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

        floorGridParent = new GameObject("FloorParent");

        floorGrid[initialColPos, initialRowPos] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], Vector3.zero, Quaternion.identity);
        floorGrid[initialColPos, initialRowPos].transform.parent = floorGridParent.transform;
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

            var upPosition = currentFloor.transform.position;
            upPosition.z += offset;
            PlaceFloor(up, upPosition, Random.Range(0, 77) >= chance);

            var downPosition = currentFloor.transform.position;
            downPosition.z -= offset;
            PlaceFloor(down, downPosition, Random.Range(0, 77) >= chance);

            var leftPosition = currentFloor.transform.position;
            leftPosition.x -= offset;
            PlaceFloor(left, leftPosition, Random.Range(0, 77) >= chance);

            var rightPosition = currentFloor.transform.position;
            rightPosition.x += offset;
            PlaceFloor(right, rightPosition, Random.Range(0, 77) >= chance);

            chance += 2;
        }
    }

    private void PlaceFloor(GridPosition floorPos, Vector3 pos, bool place)
    {
        if(!place)
        {
            return;
        }

        if(floorPos.x >= 0 && floorPos.x < Columns && floorPos.y >= 0 && floorPos.y < Rows)
        {
            if(floorGrid[floorPos.x, floorPos.y] == null)
            {
                floorGrid[floorPos.x, floorPos.y] = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], pos, Quaternion.identity);
                floorGrid[floorPos.x, floorPos.y].transform.parent = floorGridParent.transform;
                gridPositions.Enqueue(floorPos);
            }
        }
    }

    private void ConnectFloor()
    {
        for(int rowIndex = 0; rowIndex < Rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < Columns; columnIndex++)
            {
                var currentFloor = floorGrid[columnIndex, rowIndex];
                if(currentFloor != null)
                {
                    var up = new GridPosition(columnIndex, rowIndex - 1);
                    var down = new GridPosition(columnIndex, rowIndex + 1);
                    var left = new GridPosition(columnIndex - 1, rowIndex);
                    var right = new GridPosition(columnIndex + 1, rowIndex);


                    ConnectIndividuals(up, currentFloor, 0);
                    ConnectIndividuals(down, currentFloor, 1);
                    ConnectIndividuals(left, currentFloor, 2);
                    ConnectIndividuals(right, currentFloor, 3);
                }
            }
        }
    }

    private void ConnectIndividuals(GridPosition floorPos, DownFloor currentFloor, int direction)
    {
        if(floorPos.x >= 0 && floorPos.x < Columns && floorPos.y >= 0 && floorPos.y < Rows)
        {
            switch(direction)
            {
                case 0:
                    currentFloor.UpDirection = floorGrid[floorPos.x, floorPos.y];
                    break;
                case 1:
                    currentFloor.DownDirection = floorGrid[floorPos.x, floorPos.y];
                    break;
                case 2:
                    currentFloor.LeftDirection = floorGrid[floorPos.x, floorPos.y];
                    break;
                case 3:
                    currentFloor.RightDirection = floorGrid[floorPos.x, floorPos.y];
                    break;
                default:
                    break;
            }
        }
    }
}
