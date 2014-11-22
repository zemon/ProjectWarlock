using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

    private int speed;
    private int force;
    private int range;

    public int team;

	// Use this for initialization
	void Start ()
    {
        speed = PlayerStats.speedFireball;
        force = PlayerStats.forceFireball;
        range = PlayerStats.rangeFireball;

        ChangeColor(team);
        Destroy(gameObject,range);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
	}

    private void ChangeColor(int t)
    {
        switch (t)
        {
            case 0:
                gameObject.renderer.material.color = Color.green;
                break;
            case 1:
                gameObject.renderer.material.color = Color.blue;
                break;
            case 2:
                gameObject.renderer.material.color = Color.red;
                break;
            case 3:
                gameObject.renderer.material.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Trigger");
        if (c.CompareTag("Player") && c.GetComponent<PlayerSpells>().pTeam != team)
        {
            if (!networkView.isMine)
            {
                Debug.Log("player hit");
                Vector3 dir = (c.ClosestPointOnBounds(transform.position) - transform.position).normalized;
                c.GetComponent<PlayerMovement>().Push(dir, force);
                networkView.RPC("DestroySelf", RPCMode.Server, GetComponent<NetworkView>().viewID);
            }
        }
        else if (c.CompareTag("Spell") && c.GetComponent<Spell>().team != team)
        {
            if (Network.isServer)
            {
                Network.Destroy(c.GetComponent<NetworkView>().viewID);
                Network.Destroy(GetComponent<NetworkView>().viewID);
            }
        }
        else if (c.CompareTag("Pushable"))
        {
            Vector3 dir = (c.ClosestPointOnBounds(transform.position) - transform.position).normalized;
            c.GetComponent<ObjectPushable>().Push(dir, force);
            Network.Destroy(GetComponent<NetworkView>().viewID);
        }
    }

    [RPC]
    private void DestroySelf(NetworkViewID viewID)
    {
        Network.Destroy(viewID);
    }
}
