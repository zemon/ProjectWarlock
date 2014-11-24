using UnityEngine;
using System.Collections;

public class HomingSpell : MonoBehaviour {

    private int speed;
    private int force;
    private int range;
    private float turnSpeed;
    GameObject[] player;

    private float nearestDistance = Mathf.Infinity;
    private float distanceSqr;
    Transform target;
	// Use this for initialization
	void Start () 
    {
        speed = PlayerStats.speedHoming;
        force = PlayerStats.forceHoming;
        range = PlayerStats.rangeHoming;
        turnSpeed = PlayerStats.turnSpeedHoming;
        player = GameObject.FindGameObjectsWithTag("Player");
        //FindTarget();
        Destroy(gameObject, range);
        StartCoroutine("FindTarget");
	}
    IEnumerator FindTarget()
    {
        for (; ; )
        {
            foreach (GameObject go in player)
            {
                if (go.GetComponent<PlayerSpells>().team != this.GetComponent<SpellVariables>().team)
                {
                    distanceSqr = (go.transform.position - this.transform.position).sqrMagnitude;
                    if (nearestDistance > distanceSqr)
                    {
                        target = go.transform;
                        Debug.Log("Target Found");
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }

	// Update is called once per frame
	void Update () 
    {
        //Quaternion.LookRotation(target.position);
        if (target != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), turnSpeed);

        }
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
