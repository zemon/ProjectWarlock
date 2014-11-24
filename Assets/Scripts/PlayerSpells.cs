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
    public GameObject homingPrefab;
    public GameObject tempSpell;

    public LayerMask floorMask;
    public int team;

    void Start()
    {
        spellQ = fireballPrefab;
        spellW = homingPrefab;
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                selectedSpellSlot = "W";
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
        NetworkViewID viewID;
        switch (spellSlot)
        {
            // Spell slot Q
            case "Q":
                viewID = Network.AllocateViewID();
                networkView.RPC("SpawnSpell", RPCMode.AllBuffered, viewID, mouseDirection, team, spellSlot);
                Debug.Log("Q fired" + mouseDirection);
                selectedSpellSlot = "";
                break;
                // Spell slot W
            case "W":
                viewID = Network.AllocateViewID();
                networkView.RPC("SpawnSpell", RPCMode.AllBuffered, viewID, mouseDirection, team, spellSlot);
                Debug.Log("W fired" + mouseDirection);
                selectedSpellSlot = "";
                break;

            default:
                Debug.Log("Spell slot not in use: " + spellSlot);
                break;
        }
    }

    [RPC]
    private void SpawnSpell(NetworkViewID viewID, Vector3 dir, int team, string spellSlot)
    {
        switch(spellSlot)
        {
            case "Q":
                tempSpell = spellQ;
                break;
            case "W":
                tempSpell = spellW;
                break;
            default:
                Debug.Log("Spell slot not in use: " + selectedSpellSlot);
                break;
        }

        tempSpell.GetComponent<SpellVariables>().team = team;
        NetworkView nView;
        nView = tempSpell.GetComponent<NetworkView>();
        nView.viewID = viewID;
        Instantiate(tempSpell, transform.position, Quaternion.LookRotation(dir));
    }
}
