using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorCreator : MonoBehaviour 
{
    [SerializeField]
    private List<DownFloor> floorPrefabs;

    [SerializeField]
    private DownFloor stationaryFloorPrefab;

    [SerializeField]
    private DownFloor winFloorPrefab;

    [SerializeField]
    private int rows = 10;

    [SerializeField]
    private int columns = 10;

    private DownFloor[,] floorGrid;
    private Queue<GridPosition> gridPositions;
    private List<DownFloor> floorList;
    private float offsetPosition = 2.25f;
    private GameObject floorGridParentObject;
    private bool stationarySet = false;

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
        floorList = new List<DownFloor>();
    }

    public void SetUpFloors()
    {
        stationarySet = false;
        CreateFloor();
        ConnectFloor();
    }

    public void FloorsGoDown()
    {
        for(int index = 0; index < floorList.Count; index++)
        {
            floorList[index].HandleDownFloor();
        }
    }

    public void RearrangeFloors()
    {
        floorList = floorList.Where(floor => floor != null).ToList();
    }

    public DownFloor GetFloor()
    {
        var floor = floorList[Random.Range(0, floorList.Count)];
        return floor;
    }

    public void SetUpLoseFloors()
    {
        for(int index = 0; index < floorList.Count; index++)
        {
            var floor = floorList[index];
            if(!floor.IsWinningFloor())
            {
                floor.SetFloorType(DownFloor.DownFloorType.Lose);
            }
        }
    }

    private void CreateFloor()
    {
        floorGrid = new DownFloor[columns, rows];
        floorList.Clear();

        for(int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                floorGrid[columnIndex, rowIndex] = null;
            }
        }

        gridPositions = new Queue<GridPosition>();

        int initialColPos = columns / 2;
        int initialRowPos = rows / 2;

        floorGridParentObject = new GameObject("FloorParent");

        floorGrid[initialColPos, initialRowPos] = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Count)], Vector3.zero, Quaternion.identity);
        floorGrid[initialColPos, initialRowPos].transform.parent = floorGridParentObject.transform;
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
            upPosition.z += offsetPosition;
            PlaceFloor(up, upPosition, Random.Range(0, 75 + Random.Range(1, 9)) >= chance);

            var downPosition = currentFloor.transform.position;
            downPosition.z -= offsetPosition;
            PlaceFloor(down, downPosition, Random.Range(0, 75 + Random.Range(1, 9)) >= chance);

            var leftPosition = currentFloor.transform.position;
            leftPosition.x -= offsetPosition;
            PlaceFloor(left, leftPosition, Random.Range(0, 75 + Random.Range(1, 9)) >= chance);

            var rightPosition = currentFloor.transform.position;
            rightPosition.x += offsetPosition;
            PlaceFloor(right, rightPosition, Random.Range(0, 75 + Random.Range(1, 9)) >= chance);

            chance += 2;
        }


        DownFloor randomFloor;
        var randomRow = 0;
        var randomCol = 0;
        do
        {
            randomRow = Random.Range(0, rows);
            randomCol = Random.Range(0, columns);
            randomFloor = floorGrid[randomCol, randomRow];
        }
        while(randomFloor == null);

        floorGrid[randomCol, randomRow] = Instantiate(winFloorPrefab, randomFloor.transform.position, Quaternion.identity);
        floorGrid[randomCol, randomRow].transform.parent = floorGridParentObject.transform;
        Destroy(randomFloor.gameObject);


        for(int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                var currentFloor = floorGrid[columnIndex, rowIndex];
                if(currentFloor != null)
                {
                    floorList.Add(currentFloor);
                }
            }
        }
    }

    private void PlaceFloor(GridPosition floorPos, Vector3 pos, bool place)
    {
        if(!place)
        {
            return;
        }

        if(floorPos.x >= 0 && floorPos.x < columns && floorPos.y >= 0 && floorPos.y < rows)
        {
            if(floorGrid[floorPos.x, floorPos.y] == null)
            {
                stationarySet = Random.Range(0, 99) >= 92;
                floorGrid[floorPos.x, floorPos.y] = Instantiate(stationarySet ? stationaryFloorPrefab : floorPrefabs[Random.Range(0, floorPrefabs.Count)], pos, Quaternion.identity);
                floorGrid[floorPos.x, floorPos.y].transform.parent = floorGridParentObject.transform;
                gridPositions.Enqueue(floorPos);
            }
        }
    }

    private void ConnectFloor()
    {
        for(int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for(int columnIndex = 0; columnIndex < columns; columnIndex++)
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
        if(floorPos.x >= 0 && floorPos.x < columns && floorPos.y >= 0 && floorPos.y < rows)
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
