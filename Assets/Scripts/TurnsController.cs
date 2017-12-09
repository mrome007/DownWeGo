using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnsController : MonoBehaviour 
{
    private int numberOfActions = 3;
    private int enemyNumberOfActions = 7;
    [SerializeField]
    private DownPlayer playerPrefab;

    [SerializeField]
    private DownEnemy enemyPrefab;

    [SerializeField]
    private FloorCreator floorCreator;

    [SerializeField]
    private int possibleNumberOfPlayers;

    [SerializeField]
    private int possibleNumberOfEnemies;

    private DownPlayer selectedPlayer;
    private int playerLayerMask;
    private Vector3 mousePosition;
    private bool playTurn = true;
    private List<DownFloor> fallingFloors;
    private List<DownEnemy> enemies;

    private void Start()
    {
        selectedPlayer = null;
        playTurn = true;
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        fallingFloors = floorCreator.GetFloors();
        enemies = new List<DownEnemy>();
        CreatePlayers();
        CreateEnemies();

        enemyNumberOfActions = 5;
        numberOfActions = 3;
    }

    private void Update()
    {
        if(!playTurn)
        {
            return;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            SelectPlayer();
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(selectedPlayer != null)
            {
                var mousePositionLast = Input.mousePosition;
                var yDifference = mousePositionLast.y - mousePosition.y;
                var xDifference = mousePositionLast.x - mousePosition.x;
                if(Mathf.Abs(yDifference) > 1.0f || Mathf.Abs(xDifference) > 1.0f)
                {
                    var success = false;
                    if(Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
                    {
                        success = selectedPlayer.MovePlayer(yDifference > 0 ? 0 : 1);
                    }
                    else
                    {
                        success = selectedPlayer.MovePlayer(xDifference > 0 ? 3 : 2);
                    }

                    if(success)
                    {
                        numberOfActions--;
                        if(numberOfActions == 0)
                        {
                            numberOfActions = 3;
                            StartCoroutine(NextTurn());
                        }
                    }
                }
            }
        }



        if(selectedPlayer != null)
        {
            var moveIndex = -1;
            if(Input.GetKeyDown(KeyCode.A))
            {
                moveIndex = 2;
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                moveIndex = 3;
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                moveIndex = 0;
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                moveIndex = 1;
            }

            var success = selectedPlayer.MovePlayer(moveIndex);
            if(success)
            {
                numberOfActions--;
                if(numberOfActions == 0)
                {
                    numberOfActions = 3;
                    StartCoroutine(NextTurn());
                }
            }
        }
    }

    private IEnumerator EnemyTurn()
    {
        for(var index = 0; index < enemies.Count;)
        {
            var enemy = enemies[index];
            if(enemies[index] != null)
            {
                enemies[index].MoveEnemy();
            }
            yield return new WaitForSeconds(0.5f);
            enemyNumberOfActions--;
            if(enemyNumberOfActions == 0)
            {
                enemyNumberOfActions = 7;
                break;
            }
            index++;
            index %= enemies.Count;
        }
        yield return null;
    }

    private IEnumerator NextTurn()
    {
        playTurn = false;

        yield return StartCoroutine(EnemyTurn());

        for(int index = 0; index < fallingFloors.Count; index++)
        {
            fallingFloors[index].HandleNextTurn();
        }

        yield return new WaitForSeconds(2f);
        yield return null;

        fallingFloors = fallingFloors.Where(floor => floor != null).ToList();
        playTurn = true;
    }

    private void SelectPlayer()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, playerLayerMask))
        {
            selectedPlayer = hit.collider.GetComponent<DownPlayer>();
        }
    }

    private void CreatePlayers()
    {
        var numPlayers = UnityEngine.Random.Range(1, possibleNumberOfPlayers);

        for(int index = 0; index < numPlayers; index++)
        {
            var thePlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
            var randomFloor = fallingFloors[UnityEngine.Random.Range(0, fallingFloors.Count)];
            thePlayer.transform.position = randomFloor.transform.position;
            randomFloor.InsertPlayer(thePlayer);
            thePlayer.SetCurrentFloor(randomFloor);
            thePlayer.transform.parent = randomFloor.transform;
        }
    }

    private void CreateEnemies()
    {
        var numEnemies = UnityEngine.Random.Range(1, possibleNumberOfEnemies);

        for(int index = 0; index < numEnemies; index++)
        {
            var theEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemies.Add(theEnemy);
            var randomFloor = fallingFloors[UnityEngine.Random.Range(0, fallingFloors.Count)];
            theEnemy.transform.position = randomFloor.transform.position;
            randomFloor.InsertEnemy(theEnemy);
            theEnemy.SetCurrentFloor(randomFloor);
            theEnemy.transform.parent = randomFloor.transform;
        }
    }
}
