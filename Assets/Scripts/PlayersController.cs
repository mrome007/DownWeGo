using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersController : MonoBehaviour 
{
    [SerializeField]
    private FloorCreator floorCreator;

    [SerializeField]
    private Player playerPrefab;

    [SerializeField]
    private Player enemyPrefab;

    [SerializeField]
    private Transform winnerBlock;

    private List<Player> enemyList;
    private List<Player> playerList;

    private void Awake()
    {
        enemyList = new List<Player>();
        playerList = new List<Player>();
    }

    private void CreatePlayers(int minimumNumOfPlayers, int possibleNumberOfPlayers, bool isPlayer)
    {
        var numPlayers = UnityEngine.Random.Range(minimumNumOfPlayers, possibleNumberOfPlayers);

        for(int index = 0; index < numPlayers; index++)
        {
            var prefab = isPlayer ? playerPrefab : enemyPrefab;
            var player = Instantiate(prefab, Vector3.zero, isPlayer ? Quaternion.Euler(new Vector3(0f, 180f, 0f)) : Quaternion.identity);
            if(isPlayer)
            {
                player.PlayerSelectable(true);
                playerList.Add(player);
            }
            else
            {
                enemyList.Add(player);
            }
            var randomFloor = floorCreator.GetFloor();
            randomFloor.InsertPlayer(player);
            player.SetCurrentFloor(randomFloor);
            player.transform.position = randomFloor.transform.position;
            player.transform.parent = randomFloor.transform;
        }
    }

    public void CreatePlayersAndEnemies(int numPlayers, int numEnemies)
    {
        CreatePlayers(4, numPlayers, true);
        CreatePlayers(7, numEnemies, false);
    }

    public void RearrangePlayers()
    {
        enemyList = enemyList.Where(enemy => enemy != null).ToList();
        playerList = playerList.Where(player => player != null).ToList();
    }

    public IEnumerator EnemyTurn(int numActions)
    {
        for(var index = 0; index < enemyList.Count;)
        {
            var enemy = enemyList[index];
            if(enemyList[index] != null)
            {
                enemyList[index].MovePlayer();
            }
            yield return new WaitForSeconds(0.5f);
            numActions--;
            if(numActions == 0)
            {
                break;
            }

            index++;
            index %= enemyList.Count;
        }
        yield return null;
    }

    public void MovePlayersToWinners()
    {
        for(int index = 0; index < playerList.Count; index++)
        {
            var player = playerList[index];
            if(player.CurrentFloor.IsWinningFloor())
            {
                if(player.transform.parent != winnerBlock)
                {
                    player.PlayerSelectable(false);
                    player.transform.rotation = winnerBlock.rotation;
                    player.transform.position = winnerBlock.position;
                    player.transform.rotation = Quaternion.Euler(new Vector3(winnerBlock.transform.localRotation.x, Random.Range(180f, 360f), winnerBlock.transform.localRotation.z));
                    player.transform.parent = winnerBlock;
                    player.transform.localPosition += new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                    player.SetCurrentFloor(null);
                }
            }
        }

        playerList = playerList.Where(player => player.CurrentFloor != null).ToList();
    }

    public bool HavePlayers()
    {
        return playerList.Count > 0;
    }
}
