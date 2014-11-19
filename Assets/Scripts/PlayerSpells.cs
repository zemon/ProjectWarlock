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
    public GameObject emptyPrefab;

    public LayerMask floorMask;

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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedSpellSlot = "Q";
                Debug.Log("Q selected");
            }
        }
	}

    void CastSpell(string spellSlot)
    {
        Vector3 mouseDirection = new Vector3();
        Ray camRay = Camera.main.ScreenPointToRay(InputSettings.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask))
        {
            mouseDirection = floorHit.point - transform.position;
            mouseDirection.y = 0;
        }
        switch (spellSlot)
        {
            // Spell slot Q
            case "Q":

                Network.Instantiate(spellQ, transform.position, Quaternion.LookRotation(mouseDirection),0);
                Debug.Log("Q fired" + mouseDirection);
                selectedSpellSlot = "";
                break;

            default:
                Debug.Log("Spell slot not in use: " + spellSlot);
                break;
        }
    }
}
