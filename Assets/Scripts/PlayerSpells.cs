using UnityEngine;
using System.Collections;

public class PlayerSpells : MonoBehaviour {

    // Spell slots
    private GameObject spellQ;
    private GameObject spellW;
    private GameObject spellE;

    // Current spell selcted ("Q","W","E")
    private string selectedSpellSlot = "";

    // All the different spell prefabs
    public GameObject fireballPrefab;
    public GameObject tempSpell;

    public LayerMask floorMask;
    public int pTeam;
    private PlayerMovement MovementScript;

    void Start()
    {
        MovementScript = gameObject.GetComponent<PlayerMovement>();
        spellQ = fireballPrefab;
    }

	void Update ()
    {
        if (networkView.isMine)
        {
            if (Input.GetMouseButtonDown(0) && selectedSpellSlot != "")
            {
                CastSpell(selectedSpellSlot);
            }
            if (Input.GetMouseButtonDown(1) && selectedSpellSlot != "")
            {
                selectedSpellSlot = "";
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedSpellSlot = "Q";
            }
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if (networkView.isMine)
        {
            if (c.CompareTag("Spell") && c.gameObject.GetComponent<Spell>().team != pTeam)
            {
                Spell spellStats = c.gameObject.GetComponent<Spell>();
                Vector3 hitPoint = c.ClosestPointOnBounds(transform.position);
                Vector3 forceDir = (hitPoint - c.transform.position).normalized;
                MovementScript.Push(forceDir, spellStats.force, c.gameObject);
            }
        }
    }

    private void CastSpell(string spellSlot)
    {
        Vector3 mouseDirection = new Vector3();
        Ray camRay = Camera.main.ScreenPointToRay(InputSettings.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask))
        {
            mouseDirection = (floorHit.point - transform.position).normalized;
            mouseDirection.y = 0;
        }
        switch (spellSlot)
        {
            // Spell slot Q
            case "Q":
                networkView.RPC("SpawnSpell", RPCMode.AllBuffered, mouseDirection, pTeam, PlayerStats.speedFireball, PlayerStats.forceFireball, PlayerStats.rangeFireball);
                Debug.Log("Q fired" + mouseDirection);
                selectedSpellSlot = "";
                break;

            default:
                Debug.Log("Spell slot not in use: " + spellSlot);
                break;
        }
    }

    [RPC]
    private void SpawnSpell(Vector3 dir, int team, int speed, int force, int range)
    {
        tempSpell = fireballPrefab;
        tempSpell.GetComponent<Spell>().team = team;
        tempSpell.GetComponent<Spell>().force = force;
        tempSpell.GetComponent<Spell>().speed = speed;
        tempSpell.GetComponent<Spell>().range = range;
        Instantiate(tempSpell, transform.position, Quaternion.LookRotation(dir));
    }
}
