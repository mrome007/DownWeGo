using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnsController : MonoBehaviour 
{
    [SerializeField]
    private FloorCreator floorCreator;

    [SerializeField]
    private PlayersController playersController;

    [SerializeField]
    private int possibleNumberOfPlayers;

    [SerializeField]
    private int possibleNumberOfEnemies;

    [SerializeField]
    private Indicator indicatorPrefab;

    private int numberOfActions = 3;
    private int enemyNumberOfActions = 7;
    private Player selectedPlayer;
    private int playerLayerMask;
    private Vector3 mousePosition;
    private bool playTurn = true;
    private Indicator indicator;

    private void Start()
    {
        selectedPlayer = null;
        indicator = null;
        playTurn = true;
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        floorCreator.SetUpFloors();

        playersController.CreatePlayersAndEnemies(possibleNumberOfPlayers, possibleNumberOfEnemies);

        enemyNumberOfActions = 7;
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            numberOfActions = 3;
            StartCoroutine(NextTurn());
        }
    }

    private IEnumerator NextTurn()
    {
        playTurn = false;

        if(indicator != null)
        {
            indicator.ShowIndicator(false);
        }
            
        yield return StartCoroutine(playersController.EnemyTurn(enemyNumberOfActions));

        floorCreator.FloorsGoDown();

        yield return new WaitForSeconds(2f);

        yield return null;

        floorCreator.RearrangeFloors();
        playersController.RearrangePlayers();

        playersController.MovePlayersToWinners();

        playTurn = true;

        if(indicator != null)
        {
            indicator.ShowIndicator(true);
        }
    }

    private void SelectPlayer()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, playerLayerMask))
        {
            selectedPlayer = hit.collider.GetComponent<Player>();
            if(indicator == null)
            {
                indicator = Instantiate(indicatorPrefab, hit.collider.transform.position, Quaternion.identity);
            }
            else
            {
                indicator.MoveIndicator(hit.collider.transform.position);
            }
            indicator.transform.parent = selectedPlayer.transform;
        }
    }
}
