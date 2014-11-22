using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public bool freeForAll;
    public int players = 0;
    public GameObject playerPrefab;
	// Use this for initialization
	void Start ()
    {
        networkView.RPC("SpawnPlayer", RPCMode.Server, players);
        //Invoke("SetupTeams", 1); 
	}

    [RPC]
    private void SpawnPlayer(int playerID)
    {
        players++;
        GameObject newPlayer = (GameObject)Network.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity,0);
        newPlayer.GetComponent<PlayerSpells>().pTeam = playerID;
        switch (playerID)
        {
            case 0:
                newPlayer.renderer.material.color = Color.green;
                break;
            case 1:
                newPlayer.renderer.material.color = Color.blue;
                break;
            case 2:
                newPlayer.renderer.material.color = Color.red;
                break;
            case 3:
                newPlayer.renderer.material.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    [RPC]
    private void AddPlayer()
    {
        players++;
    }



    /*private void SetupTeams()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (freeForAll)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerSpells>().pTeam = i;
                switch (i)
                {
                    case 0:
                        players[i].renderer.material.color = Color.green;
                        break;
                    case 1:
                        players[i].renderer.material.color = Color.blue;
                        break;
                    case 2:
                        players[i].renderer.material.color = Color.red;
                        break;
                    case 3:
                        players[i].renderer.material.color = Color.yellow;
                        break;
                    default:
                        break;
                }
            }
        }
    }*/
}
