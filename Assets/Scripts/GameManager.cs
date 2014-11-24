using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public bool freeForAll;
	// Use this for initialization
	void Start ()
    {
        Invoke("SetupTeams", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {

    }

    private void SetupTeams()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (freeForAll)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerSpells>().team = i;
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
    }
}
