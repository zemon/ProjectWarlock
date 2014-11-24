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
    public int team;

    void Start()
    {
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
                NetworkViewID viewID = Network.AllocateViewID();
                networkView.RPC("SpawnSpell", RPCMode.AllBuffered, viewID, mouseDirection, team, PlayerStats.speedFireball, PlayerStats.forceFireball, PlayerStats.rangeFireball);
                Debug.Log("Q fired" + mouseDirection);
                selectedSpellSlot = "";
                break;

            default:
                Debug.Log("Spell slot not in use: " + spellSlot);
                break;
        }
    }

    [RPC]
    private void SpawnSpell(NetworkViewID viewID, Vector3 dir, int team, int speed, int force, int range)
    {
        tempSpell = spellQ;
        tempSpell.GetComponent<Fireball>().team = team;
        NetworkView nView;
        nView = tempSpell.GetComponent<NetworkView>();
        nView.viewID = viewID;
        Instantiate(tempSpell, transform.position, Quaternion.LookRotation(dir));
    }
}
