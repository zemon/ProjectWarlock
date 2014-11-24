using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    private int speed;
    private int force;
    private int range;



	// Use this for initialization
	void Start ()
    {
        speed = PlayerStats.speedFireball;
        force = PlayerStats.forceFireball;
        range = PlayerStats.rangeFireball;


        Destroy(gameObject,range);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
	}



    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Trigger");
        if (c.CompareTag("Player") && c.GetComponent<PlayerSpells>().team != this.GetComponent<SpellVariables>().team)
        {
            if (!networkView.isMine)
            {
                Debug.Log("player hit");
                Vector3 dir = (c.ClosestPointOnBounds(transform.position) - transform.position).normalized;
                c.GetComponent<PlayerMovement>().Push(dir, force);
                networkView.RPC("DestroySelf", RPCMode.Server, GetComponent<NetworkView>().viewID);
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
